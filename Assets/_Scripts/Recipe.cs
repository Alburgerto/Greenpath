using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Recipe : ScriptableObject
{
    public string m_name;
    [TextArea] public string m_description; // TextArea allows to add New Line in the Inspector
    public List<Ingredient> m_ingredients; // 1 or more ingredients, each with 1 or more quantity
    public List<int> m_ingredientCount; // Separate from m_ingredients for serializing (won't Show a List<Tuple<int, Ingredient>> in the inspector)
    [HideInInspector] public bool m_cooked; // Whether this recipe has been cooked or not

    private void OnEnable()
    {
        m_cooked = false;
    }
}
