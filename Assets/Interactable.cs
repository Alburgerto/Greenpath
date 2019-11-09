using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public UnityEvent m_interaction;
    public UnityEvent m_stopInteraction;
    public Ingredient m_ingredient;
    public string m_text;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            m_stopInteraction.Invoke();
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
}
