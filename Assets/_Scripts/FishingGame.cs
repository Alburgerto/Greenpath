using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FishingGame : MonoBehaviour
{
    public enum FishingState { PLAYING, WON, LOST }

    public Image m_fill;
    public float m_sliderSpeed;
    public string[] m_fishList;
    public Inventory m_inventory;
    public float m_fadeTime;
    public CollectedItemUI m_collectedItemUI;
    public float m_playerSpeed;

    private Transform m_player;
    private Transform m_fish;
    private Slider m_slider;
    private bool m_barFilling;
    private FishingState m_state;

    // Start is called before the first frame update
    void Start()
    {
        m_player = transform.Find("Player");
        m_fish = transform.Find("Fish");
        m_slider = transform.Find("Slider").GetComponent<Slider>();
        m_barFilling = m_player.GetComponent<Collider2D>().bounds.Intersects(m_fish.GetComponent<Collider2D>().bounds);
        m_state = FishingState.PLAYING;
    }

    private void OnEnable()
    {
        StartCoroutine(FadePanel(true));
    }

    // Update is called once per frame
    void Update()
    {
        FishInput();
        FillBar();
        CheckWinLoseCondition();
    }

    private void FishInput()
    {
        Vector2 movement = new Vector2();
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        Vector2 newPosition = new Vector2(m_player.position.x + movement.x * m_playerSpeed, m_player.position.y + movement.y * m_playerSpeed);
        m_player.position = newPosition;
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
            Debug.Log("I LOST! FUCK!");
            m_state = FishingState.LOST;

            StopAllCoroutines();
            StartCoroutine(FadePanel(false));
        }
        else if (m_slider.value == 1)
        {
            Debug.Log("I WIN!");
            m_state = FishingState.WON;

            StopAllCoroutines();
            StartCoroutine(FadePanel(false));
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
            transform.localScale = currentScale;
            while (time < m_fadeTime)
            {
                currentScale.x = Mathf.Lerp(currentScale.x, targetScale.x, time / m_fadeTime);
                currentScale.y = Mathf.Lerp(currentScale.y, targetScale.y, time / m_fadeTime);
                currentScale.z = Mathf.Lerp(currentScale.z, targetScale.z, time / m_fadeTime);
                transform.localScale = currentScale;

                time += Time.deltaTime;
                yield return null;
            }
        }
        else
        {
            if (m_state == FishingState.WON)
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
            transform.localScale = currentScale;
            while (time < m_fadeTime)
            {
                currentScale.x = Mathf.Lerp(currentScale.x, targetScale.x, time / m_fadeTime);
                currentScale.y = Mathf.Lerp(currentScale.y, targetScale.y, time / m_fadeTime);
                currentScale.z = Mathf.Lerp(currentScale.z, targetScale.z, time / m_fadeTime);
                transform.localScale = currentScale;

                time += Time.deltaTime;
                yield return null;
            }
            gameObject.SetActive(false);
        }
    }

    public void PlayerColliding(bool l_colliding)
    {
        m_barFilling = l_colliding;
    }
}
