using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonfire : Interactable
{
    public GameObject m_recipeBook;

    public override void Interact()
    {
        if (m_recipeBook != null)
        {
            m_recipeBook.SetActive(true);
        }
    }
}
