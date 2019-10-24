using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private Dictionary<Ingredient, int> m_ingredients;

    public void AddIngredient(Ingredient l_ingredient, int l_count = 1)
    {
        if (m_ingredients.ContainsKey(l_ingredient))
        {
            m_ingredients[l_ingredient] += l_count;
        } else
        {
            m_ingredients.Add(l_ingredient, l_count);
        }

        Debug.Log("{m_ingredients[l_ingredient]} items of type {l_ingredient.m_name}");
    }
}
