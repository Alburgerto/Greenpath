using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Recipe : ScriptableObject
{
    public string m_name;
    public string m_description;
    public List<Ingredient> m_ingredients; // 1 or more ingredients, each with 1 or more quantity
    public List<int> m_ingredientCount; // Separate from m_ingredients for serializing (won't Show a List<Tuple<int, Ingredient>> in the inspector)
    public List<int> m_availableIngredients; // Number of ingredients in the inventory

    // {  m_ingredients[i].m_name    m_availableIngredients[i] / m_ingredientCount[i]  }
}
