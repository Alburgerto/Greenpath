using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingSpot : Interactable
{
    public string[] m_fish;
    public Inventory m_inventory;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Interact()
    {
        StartCoroutine(Fishing());
    }

    public IEnumerator Fishing()
    {
        int fishIndex = UnityEngine.Random.Range(0, m_fish.Length);
        string fish = m_fish[fishIndex];
        Ingredient ingredient = fish;
        m_inventory.AddIngredient(ingredient);

        yield return new WaitForSeconds(0.5f);

    }
}
