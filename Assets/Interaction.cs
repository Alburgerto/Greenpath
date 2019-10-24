using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    public GameObject m_interactionUI;
    private Ray m_ray;
    private RaycastHit m_hit;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnInteractionZone()
    {
        m_ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(m_ray, out m_hit, 10f))
        {
            Debug.Log("RAY HIT");

            if (m_hit.transform.CompareTag("Ingredient"))
            {
                Debug.Log("INGREDIENT");
                m_interactionUI.SetActive(true);
                m_interactionUI.GetComponentInChildren<TextMeshProUGUI>().text += m_hit.transform.name;

                if (Input.GetKeyDown(KeyCode.E))
                {
                    Destroy(m_hit.transform.gameObject);
                    m_interactionUI.SetActive(false);
                }
            }
        }
    }

    public void OnInteractionZoneExit()
    {
        m_interactionUI.SetActive(false);
    }

}
