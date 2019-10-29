using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    public GameObject m_interactionUI;
    public float m_interactionDistance = 6;
    private Ray m_ray;
    private RaycastHit m_hit;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && m_hit.transform != null && m_hit.transform.CompareTag("Ingredient"))
        {
            Debug.Log("boom");
            Destroy(m_hit.transform.gameObject);
            m_interactionUI.SetActive(false);
        }
    }

    public void OnInteractionZone()
    {
        StartCoroutine("Interacting");
    }

    private IEnumerator Interacting()
    {
        bool showingUI = false;
        while (true)
        {
            m_ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            if (Physics.Raycast(m_ray, out m_hit, m_interactionDistance))
            {
                if (m_hit.transform.CompareTag("Ingredient"))
                {
                    if (!showingUI)
                    {
                        showingUI = true;
                        m_interactionUI.SetActive(true);
                        m_interactionUI.GetComponentInChildren<TextMeshProUGUI>().text = m_hit.transform.GetComponent<Interactable>().m_text;
                    }
                }
                else
                {
                    showingUI = false;
                    m_interactionUI.SetActive(false);
                }
            } 
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void OnInteractionZoneExit()
    {
        m_interactionUI.SetActive(false);
    }

}
