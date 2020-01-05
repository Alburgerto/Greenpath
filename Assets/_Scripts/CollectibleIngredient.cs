using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CollectibleIngredient : Interactable
{
    public Ingredient m_ingredient;
    public Inventory m_inventory;
    public CollectedItemUI m_collectedItemUI;
    public AudioClip m_collectClip;

    private AudioSource m_audioSource;

    private new void Start()
    {
        base.Start();
        m_inventory = m_inventory.GetComponent<Inventory>();
        m_audioSource = GetComponent<AudioSource>();
    }

    public override void Interact()
    {
        m_inventory.AddIngredient(m_ingredient);
        GameObject.FindGameObjectWithTag("Player").GetComponent<Interaction>().OnInteractionZoneExit();
        transform.position = new Vector3(1000, 1000, 1000);
        //Destroy(gameObject);
        m_audioSource.PlayOneShot(m_collectClip);
        m_collectedItemUI.SetText(m_ingredient.m_name);
    }

}