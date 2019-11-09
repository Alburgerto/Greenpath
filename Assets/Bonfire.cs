using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonfire : Interactable
{
    public GameObject m_recipeBook;

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Interact()
    {
        m_recipeBook.SetActive(true);
    }
}
