using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// List of ingredients as well as recipes
public class Inventory : MonoBehaviour
{
    public List<Recipe> m_recipes = new List<Recipe>();
    public TextMeshProUGUI m_name;
    public TextMeshProUGUI m_description;
    public TextMeshProUGUI m_ingredients;
    public TextMeshProUGUI m_availableQuantities;
    public TextMeshProUGUI m_slashes;
    public TextMeshProUGUI m_requiredQuantities;

    private int m_currentRecipeIndex;
    private Recipe m_currentRecipe { get { return m_recipes[m_currentRecipeIndex]; } }
    private Dictionary<string, int> m_inventory = new Dictionary<string, int>(); // All items (ingredients for now) collected and available to use

    private void Start()
    {
        m_currentRecipeIndex = 0;
        SetupRecipeGUI();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (m_currentRecipeIndex == 0)
            {
                m_currentRecipeIndex = m_recipes.Count - 1;
            }
            else
            {
                m_currentRecipeIndex--;
            }
            SetupRecipeGUI();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (m_currentRecipeIndex == m_recipes.Count - 1)
            {
                m_currentRecipeIndex = 0;
            }
            else
            {
                m_currentRecipeIndex++;
            }
            SetupRecipeGUI();
        }
    }

    public void SetupRecipeGUI()
    {
        ResetGUI();

        m_name.text = m_currentRecipe.m_name;
        m_description.text = m_currentRecipe.m_description;
        foreach (var ingredient in m_currentRecipe.m_ingredients)
        {
            m_ingredients.text += ingredient.m_name + Environment.NewLine;
        }

        for (int i = 0; i < m_currentRecipe.m_ingredients.Count; i++)
        {
            m_inventory.TryGetValue(m_currentRecipe.m_ingredients[i].m_name, out int availableIngredients);
            m_availableQuantities.text += availableIngredients + Environment.NewLine;
            m_slashes.text += "/" + Environment.NewLine;
            m_requiredQuantities.text += m_currentRecipe.m_ingredientCount[i] + Environment.NewLine;
        }
    }

    public void UpdateAvailableIngredients()
    {
        m_availableQuantities.text = "";

        Recipe recipe = m_recipes[m_currentRecipeIndex];
        for (int i = 0; i < recipe.m_ingredients.Count; i++)
        {
            m_inventory.TryGetValue(recipe.m_ingredients[i].m_name, out int availableIngredients);
            m_availableQuantities.text += availableIngredients + Environment.NewLine;
        }
    }

    public void ResetGUI()
    {
        m_name.text = "";
        m_description.text = "";
        m_ingredients.text = "";
        m_availableQuantities.text = "";
        m_slashes.text = "";
        m_requiredQuantities.text = "";
    }

    public void AddIngredient(Ingredient l_ingredient, int l_count = 1)
    {
        if (m_inventory.ContainsKey(l_ingredient.m_name))
        {
            m_inventory[l_ingredient.m_name] += l_count;
        }
        else
        {
            m_inventory.Add(l_ingredient.m_name, l_count);
        }
        UpdateAvailableIngredients();

      //  Debug.Log($"{m_inventory[l_ingredient.m_name]} items of type {l_ingredient.m_name}");
    }

    public void Cook()
    {
        int ingredientMatch = 0;
        foreach (var recipe in m_recipes)
        {
            for (int i = 0; i < recipe.m_ingredients.Count; ++i)
            {
                string ingredientName = recipe.m_ingredients[i].m_name;
                if (m_inventory.ContainsKey(ingredientName) && m_inventory[ingredientName] == recipe.m_ingredientCount[i])
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
