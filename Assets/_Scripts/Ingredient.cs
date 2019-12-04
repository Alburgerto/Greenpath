using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : Interactable
{
    public string m_name;
    public string m_description;
    public Inventory m_inventory;

    private new void Start()
    {
        base.Start();
        m_inventory = m_inventory.GetComponent<Inventory>();
    }

    public override void Interact()
    {
        m_inventory.AddIngredient(this);
        GameObject.FindGameObjectWithTag("Player").GetComponent<Interaction>().OnInteractionZoneExit();
        transform.position = new Vector3(1000, 1000, 1000);
        //Destroy(gameObject);

    }

}