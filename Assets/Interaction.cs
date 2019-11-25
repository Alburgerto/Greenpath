using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    public GameObject m_interactionUI;
    public GameObject m_clipboard;
    public Inventory m_inventory; // Here we attach gameObject having an Inventory script as component
    public LayerMask m_interactableLayer;
    public float m_interactionDistance;
    [HideInInspector] public bool m_coroutineRunning;

    private Animator m_clipboardAnimator;
    private GameObject m_interactionObject;
    private UITextPosition m_textScript;
    private TextMeshProUGUI m_panelText;

    private void Start()
    {
        m_coroutineRunning  = false;
        m_clipboardAnimator = m_clipboard.GetComponent<Animator>();
        m_inventory         = m_inventory.GetComponent<Inventory>();
        m_textScript        = m_interactionUI.GetComponent<UITextPosition>();
        m_panelText         = m_interactionUI.GetComponentInChildren<TextMeshProUGUI>();
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && m_interactionObject != null)
        {
            m_interactionObject.GetComponent<Interactable>().Interact();
            m_interactionObject = null;
        } 
        else if (Input.GetKeyDown(KeyCode.Tab) && m_clipboardAnimator != null)
        {
            if (!m_clipboard.activeSelf)
            {
                m_clipboard.SetActive(true);
            }
            m_clipboardAnimator.SetBool("Showing", !m_clipboardAnimator.GetBool("Showing"));
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
        m_coroutineRunning = true;
        Ray ray;
        RaycastHit rayHit;
        while (true)
        {
            ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); // Middle point in the game screen
            if (Physics.Raycast(ray, out rayHit, m_interactionDistance, m_interactableLayer, QueryTriggerInteraction.Collide))
            {
                if (m_interactionObject == rayHit.collider.gameObject) { yield return new WaitForSeconds(0.05f); }

                m_interactionObject = rayHit.collider.gameObject;
                m_textScript.m_object = m_interactionObject;
                m_textScript.ActivateUI(); // So that it's a seamless transition to a new object's position
                Interactable interactable = m_interactionObject?.GetComponent<Interactable>();
                m_panelText.text = interactable?.m_textUI ?? "";
                m_interactionUI.SetActive(true);
            }
            else
            {
                m_interactionObject = null;
                m_interactionUI.SetActive(false);
            }
            yield return new WaitForSeconds(0.05f);
        }
    }
 
    public void OnInteractionZoneExit()
    {
        Collider[] intersectingColliders = Physics.OverlapSphere(transform.position, GetComponent<SphereCollider>().radius, m_interactableLayer, QueryTriggerInteraction.Collide);
        if (intersectingColliders.Length == 1)
        {
            m_interactionObject = null;
            m_interactionUI.SetActive(false);
            m_coroutineRunning = false;
            StopCoroutine("Interacting");
        }
    }

}
