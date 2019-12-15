using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class Ingredient : ScriptableObject
{
    public string m_name;
    public string m_description;

    public static implicit operator Ingredient(string l_name)
    {
        Ingredient ingredient = new Ingredient() { m_name = l_name };
        return ingredient;
    }
}
