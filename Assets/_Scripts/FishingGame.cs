using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FishingGame : MonoBehaviour
{
    public enum FishingState { NOT_PLAYING, PLAYING, WON, LOST }

    public float m_sliderSpeed;
    public string[] m_fishList;
    public float m_fadeTime;
    public float m_playerSpeed;
    public float m_playerDeceleration;
    public float m_fishSpeed;
    public float m_wiggleIntensity;
    public float m_fishSpeedOffset;
    public float m_minFishTimeInverval;
    public float m_maxFishTimeInverval;
    public UnityStandardAssets.Characters.FirstPerson.FirstPersonController m_fpsController;
    public Image m_fill;
    public Inventory m_inventory;
    public CollectedItemUI m_collectedItemUI;
    public GameObject m_fishingPanel;
    public Transform m_topAABB;
    public Transform m_bottomAABB;
    public Transform m_rightAABB;
    public Transform m_leftAABB;
    public AudioClip m_winClip;

    private float m_fishMaxY;
    private float m_fishMinY;
    private float m_fishMaxX;
    private float m_fishMinX;
    private bool m_barFilling;
    private AudioSource m_audiosource;
    private RectTransform m_fishTransform;
    private FishingPlayer m_playerScript;
    private Rigidbody2D m_playerRB;
    private Image m_playerImage;
    private Transform m_player;
    private Transform m_fish;
    private Slider m_slider;
    public FishingState State { get; private set; }

    void Start()
    {
        m_player = transform.Find("Player");
        m_playerImage = m_player.GetComponent<Image>();
        m_playerRB = m_player.GetComponent<Rigidbody2D>();
        m_playerScript = m_player.GetComponent<FishingPlayer>();
        m_fish = transform.Find("Fish");
        m_fishTransform = m_fish.GetComponent<RectTransform>();
        m_slider = transform.Find("Slider").GetComponent<Slider>();
        m_barFilling = m_player.GetComponent<Collider2D>().bounds.Intersects(m_fish.GetComponent<Collider2D>().bounds);
        m_audiosource = GetComponent<AudioSource>();

        State = FishingState.NOT_PLAYING;
    }

    public void Initialize()
    {
        m_fish.position = new Vector2(0, 0);
        m_fish.rotation = Quaternion.identity;

        State = FishingState.PLAYING;
        m_slider.value = 0.25f;
        m_fpsController.enabled = false;
        StartCoroutine(FadePanel(true));
    }
    
    void Update()
    {
        if (State == FishingState.PLAYING)
        {
            FillBar();
            CheckWinLoseCondition();
        }
    }

    private void FixedUpdate()
    {
        if (State == FishingState.PLAYING)
        {
            PlayerMovement();
        }
    }

    private void PlayerMovement()
    {
        Vector2 movement = new Vector2
        {
            x = Input.GetAxisRaw("Horizontal"),
            y = Input.GetAxisRaw("Vertical")
        };
        if (m_playerRB.velocity.x == 0 && m_playerRB.velocity.y == 0 && movement.x == 0 && movement.y == 0) { return; }

        float pixelOffset = 2;
        Vector2 velocity = m_playerRB.velocity;
        Vector2 newPosition = m_player.position;
        if (m_player.position.y + m_playerScript.Height/2 >= m_topAABB.position.y)
        {
            newPosition.y = m_topAABB.position.y - m_playerScript.Height / 2 - pixelOffset;
            m_playerRB.velocity = new Vector2(velocity.x * m_playerDeceleration, velocity.y * -m_playerDeceleration);
        }
        else if (m_player.position.y - m_playerScript.Height / 2 <= m_bottomAABB.position.y)
        {
            newPosition.y = m_bottomAABB.position.y + m_playerScript.Height / 2 + pixelOffset;
            m_playerRB.velocity = new Vector2(velocity.x * m_playerDeceleration, velocity.y * -m_playerDeceleration);
        }

        if (m_player.position.x + m_playerScript.Width/2 >= m_rightAABB.position.x)
        {
            newPosition.x = m_rightAABB.position.x - m_playerScript.Width / 2 - pixelOffset;
            m_playerRB.velocity = new Vector2(velocity.x * -m_playerDeceleration, velocity.y * m_playerDeceleration);
        }
        else if (m_player.position.x - m_playerScript.Width / 2 <= m_leftAABB.position.x)
        {
            newPosition.x = m_leftAABB.position.x + m_playerScript.Width / 2 + pixelOffset;
            m_playerRB.velocity = new Vector2(velocity.x * -m_playerDeceleration, velocity.y * m_playerDeceleration);
        }

        m_player.position = newPosition;
        m_playerRB.AddForce(movement * m_playerSpeed);
    }

    private void FillBar()
    {
        Color targetColor;

        if (m_barFilling)
        {
            if (m_slider.value < 0.5)
            {
                targetColor = Color.yellow;
            }
            else
            {
                targetColor = Color.green;
            }
            m_slider.value += Time.deltaTime * m_sliderSpeed;
        }
        else
        {
            if (m_slider.value < 0.5)
            {
                targetColor = Color.red;
            }
            else
            {
                targetColor = Color.yellow;
            }
            m_slider.value -= Time.deltaTime * m_sliderSpeed;
        }
        
        m_fill.color = Color.Lerp(m_fill.color, targetColor, Time.deltaTime * m_slider.value);
    }

    private void CheckWinLoseCondition()
    {
        if (m_slider.value == 0)
        {
            Debug.Log("I LOST!");
            State = FishingState.LOST;

            StopAllCoroutines();
            StartCoroutine(FadePanel(false));
        }
        else if (m_slider.value == 1)
        {
            Debug.Log("I WIN!");
            State = FishingState.WON;
            m_audiosource.PlayOneShot(m_winClip);

            StopAllCoroutines();
            StartCoroutine(FadePanel(false));
        }
    }

    private IEnumerator FishMovement()
    {
        float timeBetweenMovement;
        float timeMovement;
        float speedOffset;
        float elapsed;

        while (State == FishingState.PLAYING)
        {
            timeBetweenMovement = Random.Range(m_minFishTimeInverval, m_maxFishTimeInverval); // Time it remains on the same spot until it starts moving
            elapsed = 0;
            speedOffset = Random.Range(-m_fishSpeedOffset, m_fishSpeedOffset);
            Vector2 nextPosition = new Vector2
            {
                x = Random.Range(m_fishMinX, m_fishMaxX),
                y = Random.Range(m_fishMinY, m_fishMaxY)
            };
            timeMovement = Random.Range(timeBetweenMovement - m_minFishTimeInverval, timeBetweenMovement); // Time it takes to move from A to B
            while (elapsed < timeMovement)
            {
                m_fish.position = Vector2.Lerp(m_fish.position, nextPosition, (m_fishSpeed + speedOffset) * elapsed/timeMovement);
                elapsed += Time.deltaTime;
                yield return null;
            }
            m_fish.position = nextPosition;
            yield return new WaitForSeconds(timeBetweenMovement);
        }
    }

    private IEnumerator FishWiggle()
    {
        while (State == FishingState.PLAYING)
        {
            Vector2 originalPosition = m_fish.position;
            float timeBetweenWiggle = Random.Range(0, 0.05f);
            float pixelDisplacementX = Random.Range(-m_wiggleIntensity, m_wiggleIntensity);
            float pixelDisplacementY = Random.Range(-m_wiggleIntensity, m_wiggleIntensity);

            originalPosition.x += pixelDisplacementX;
            originalPosition.y += pixelDisplacementY;
            m_fish.position = originalPosition;

            yield return new WaitForSeconds(timeBetweenWiggle);

            // ROTATION
            float rotationOffset = Random.Range(-0.01f, 0.01f);
            Quaternion fishRotation = m_fish.rotation;
            fishRotation.z += rotationOffset;
            m_fish.rotation = fishRotation;
        }
    }

    private IEnumerator FadePanel(bool l_fadeIn)
    {
        float time = 0;
        Vector3 currentScale;
        Vector3 targetScale;

        if (l_fadeIn)
        {
            targetScale = new Vector3(1, 1, 1);
            currentScale = new Vector3(0, 0, 0);
            m_fishingPanel.transform.localScale = currentScale;
            while (time < m_fadeTime)
            {
                currentScale = Vector3.Lerp(currentScale, targetScale, time / m_fadeTime);
                m_fishingPanel.transform.localScale = currentScale;

                time += Time.deltaTime;
                yield return null;
            }
            m_fishingPanel.transform.localScale = new Vector3(1, 1, 1);

            SetAABBLimits();
            StartCoroutine(FishMovement());
            StartCoroutine(FishWiggle());
        }
        else
        {
            m_playerRB.velocity = new Vector2(0, 0);

            if (State == FishingState.WON)
            {
                int fishIndex = Random.Range(0, m_fishList.Length);
                string fish = m_fishList[fishIndex];
                Ingredient ingredient = fish;
                m_inventory.AddIngredient(ingredient);

                m_collectedItemUI.SetText(fish);
            }
            
            yield return new WaitForSeconds(2);

            targetScale = new Vector3(0, 0, 0);
            currentScale = new Vector3(1, 1, 1);
            m_fishingPanel.transform.localScale = currentScale;
            while (time < m_fadeTime)
            {
                currentScale = Vector3.Lerp(currentScale, targetScale, time / m_fadeTime);
                m_fishingPanel.transform.localScale = currentScale;

                time += Time.deltaTime;
                yield return null;
            }
            m_fishingPanel.transform.localScale = new Vector3(0, 0, 0);
        }

        if (State != FishingState.PLAYING)
        {
            m_fpsController.enabled = true;
            State = FishingState.NOT_PLAYING;
        //    m_fishingPanel.SetActive(false);
        }
    }

    // Can't do it at Startup because of scale issues
    private void SetAABBLimits()
    {
        m_fishMaxY = m_topAABB.position.y - m_fishTransform.sizeDelta.y / 2;
        m_fishMinY = m_bottomAABB.position.y + m_fishTransform.sizeDelta.y / 2;
        m_fishMaxX = m_rightAABB.position.x - m_fishTransform.sizeDelta.x / 2;
        m_fishMinX = m_leftAABB.position.x + m_fishTransform.sizeDelta.x / 2;
    }

    public void PlayerColliding(bool l_colliding)
    {
        Color color = m_playerImage.color;
        color.a = l_colliding ? 0.95f : 0.7f;
        m_playerImage.color = color;

        m_barFilling = l_colliding;
    }
}
