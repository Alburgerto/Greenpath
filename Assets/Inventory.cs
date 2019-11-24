using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// List of ingredients as well as recipes
public class Inventory : MonoBehaviour
{
    private Dictionary<string, int> m_ingredients = new Dictionary<string, int>();
    public List<Recipe> m_recipes = new List<Recipe>();

    public void AddIngredient(Ingredient l_ingredient, int l_count = 1)
    {
        if (m_ingredients.ContainsKey(l_ingredient.m_name))
        {
            m_ingredients[l_ingredient.m_name] += l_count;
        }
        else
        {
            m_ingredients.Add(l_ingredient.m_name, l_count);
        }

        Debug.Log($"{m_ingredients[l_ingredient.m_name]} items of type {l_ingredient.m_name}");
    }

    public void Cook()
    {
        int ingredientMatch = 0;
        foreach (var recipe in m_recipes)
        {
            for (int i = 0; i < recipe.m_ingredients.Count; ++i)
            {
                string ingredientName = recipe.m_ingredients[i].m_name;
                if (m_ingredients.ContainsKey(ingredientName) && m_ingredients[ingredientName] == recipe.m_ingredientCount[i])
                {
                    ingredientMatch++;
                }
            }
            if (ingredientMatch == recipe.m_ingredients.Count)
            {
                string log = $"Can cook a {recipe.m_name} with ";
                for (int i = 0; i < recipe.m_ingredients.Count; i++)
                {
                    log += $"{recipe.m_ingredientCount[i]} items of type {recipe.m_ingredients[i].m_name}";
                    if (i == recipe.m_ingredients.Count - 1)
                    {
                        log += ".";
                    } else
                    {
                        log += ", ";
                    }
                }
                Debug.Log(log);
                
            }
        }
    }


}
