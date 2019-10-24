using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ingredient")]
public class Ingredient : ScriptableObject
{
    public string m_name;
    public string m_description;
    public bool m_edible;
}
