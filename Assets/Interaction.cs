using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    public GameObject m_interactionUI;
    public Inventory m_inventory; // Here we attach gameObject having an Inventory script as component
    public LayerMask m_interactableLayer;
    public float m_interactionDistance = 6;
    public bool m_interactionOngoing = false;

    private GameObject m_interactionObject;
    private Ray m_ray;
    private RaycastHit m_rayHit;

    private void Start()
    {
        m_inventory = m_inventory.GetComponent<Inventory>();
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && m_interactionObject != null && m_interactionObject.CompareTag("Ingredient"))
        {
            Ingredient ingredient = m_interactionObject.GetComponent<Interactable>().m_ingredient;
            m_inventory.AddIngredient(ingredient);

            Destroy(m_interactionObject.gameObject);
            m_interactionUI.SetActive(false);
        }
    }

    public void OnInteractionZone()
    {
        
        StartCoroutine("Interacting");
    }

    // Displays interaction related UI + manages flags about interactable object
    // While this coroutine is running, rays will be cast to find an interactable object
    private IEnumerator Interacting()
    {
        while (true)
        {
            m_ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); // Middle point in the game screen
            if (Physics.Raycast(m_ray, out m_rayHit, 5, m_interactableLayer, QueryTriggerInteraction.Collide))
            {
                m_interactionObject = m_rayHit.collider.gameObject;

                m_interactionUI.GetComponent<UITextPosition>().m_object = m_interactionObject;
                m_interactionUI.SetActive(true);
                m_interactionUI.GetComponentInChildren<TextMeshProUGUI>().text = m_interactionObject.GetComponent<Interactable>().m_text;
            }
            else
            {
                m_interactionUI.SetActive(false);
            }
            yield return new WaitForSeconds(0.05f);
        }
    }
 

    public void OnInteractionZoneExit()
    {
        m_interactionOngoing = false;

        m_interactionObject = null;
        m_interactionUI.SetActive(false);
        StopCoroutine("Interacting");
    }

}
