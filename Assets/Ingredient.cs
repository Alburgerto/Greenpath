using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : Interactable
{
    public string m_name;
    public string m_description;
    public Inventory m_inventory;

    private void Start()
    {
        m_inventory = m_inventory.GetComponent<Inventory>();
    }

    public override void Interact()
    {
        m_inventory.AddIngredient(this);
  //      m_interactionUI.SetActive(false);
    //    Destroy(this);
    }

}