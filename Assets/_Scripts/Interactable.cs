using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Interactable objects need to add script (Interactable/Ingredient/etc) AND set layer to "Interactable"
public abstract class Interactable : MonoBehaviour
{
    public delegate void InteractingDelegate();

    private UnityEvent m_interaction = new UnityEvent();
    private UnityEvent m_stopInteraction = new UnityEvent();
    public string m_textUI;

    // Protected as children to this class need to execute this as well
    protected void Start()
    {
        Interaction interactionMethod = GameObject.FindGameObjectWithTag("Player").GetComponent<Interaction>();
        InteractingDelegate interactingDelegate = new InteractingDelegate(interactionMethod.OnInteractionZone);
        InteractingDelegate exitInteractingDelegate = new InteractingDelegate(interactionMethod.OnInteractionZoneExit);
        m_interaction.AddListener(delegate { interactingDelegate(); });
        m_stopInteraction.AddListener(delegate { exitInteractingDelegate(); });
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !other.gameObject.GetComponent<Interaction>().m_coroutineRunning)
        {
            m_interaction.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            m_stopInteraction.Invoke();
        }
    }

    // Every interactable object will implement different actions to take when being interacted with
    public abstract void Interact();
}
