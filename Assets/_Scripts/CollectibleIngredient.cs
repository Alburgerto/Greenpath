using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleIngredient : Interactable
{
    public Ingredient m_ingredient;
    public Inventory m_inventory;

    private new void Start()
    {
        base.Start();
        m_inventory = m_inventory.GetComponent<Inventory>();
    }

    public override void Interact()
    {
        m_inventory.AddIngredient(m_ingredient);
        GameObject.FindGameObjectWithTag("Player").GetComponent<Interaction>().OnInteractionZoneExit();
        transform.position = new Vector3(1000, 1000, 1000);
        //Destroy(gameObject);

    }

}