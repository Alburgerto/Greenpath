using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

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
    public GameObject m_successText;
    public float m_fadeTime;
    
    private bool m_textAnimating;
    private int m_currentRecipeIndex;
    private CanvasGroup m_canvasGroup;
    private Recipe m_currentRecipe { get { return m_recipes[m_currentRecipeIndex]; } }
    private Dictionary<string, int> m_inventory = new Dictionary<string, int>(); // All items (ingredients for now) collected and available to use

    private void Awake()
    {
        m_textAnimating = false;
        m_currentRecipeIndex = 0;
        m_canvasGroup = m_name.GetComponentInParent<CanvasGroup>();
    }

    private void Start()
    {
        SetupRecipeGUI();
    }

    private void Update()
    {
        if (m_textAnimating) { return; }

        int newRecipeIndex = m_currentRecipeIndex;
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.Mouse0) || Input.mouseScrollDelta.y < 0)
        {
            if (m_currentRecipeIndex == 0)
            {
                m_currentRecipeIndex = m_recipes.Count - 1;
            }
            else
            {
                m_currentRecipeIndex--;
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.Mouse1) || Input.mouseScrollDelta.y > 0)
        {
            if (m_currentRecipeIndex == m_recipes.Count - 1)
            {
                m_currentRecipeIndex = 0;
            }
            else
            {
                m_currentRecipeIndex++;
            }
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            Cook();
        }

        // If another recipe was selected
        if (newRecipeIndex != m_currentRecipeIndex)
        {
            StartCoroutine(FadeText(false));
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
        m_successText.SetActive(m_currentRecipe.m_cooked);
        StartCoroutine(FadeText(true));
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

        Debug.Log($"{m_inventory[l_ingredient.m_name]} items of type {l_ingredient.m_name}");
    }

    public IEnumerator FadeText(bool m_fadeIn)
    {
        m_textAnimating = true;
        float time = 0;
        if (m_fadeIn)
        {
            while (time < m_fadeTime)
            {
                m_canvasGroup.alpha = Mathf.Lerp(0f, 1f, time / m_fadeTime);
                time += Time.deltaTime;
                yield return null;
            }
            m_canvasGroup.alpha = 1;
        }
        else // fade out
        {
            while (time < m_fadeTime)
            {
                m_canvasGroup.alpha = Mathf.Lerp(1f, 0f, time / m_fadeTime);
                time += Time.deltaTime;
                yield return null;
            }
            m_canvasGroup.alpha = 0;
            SetupRecipeGUI();
        }
        m_textAnimating = false;
    }

    public void CookRecipe()
    {
        m_currentRecipe.m_cooked = true;
        m_successText.SetActive(true);
        Ingredient ingredient;
        int ingredientQuantity;
        for (int i = 0; i < m_currentRecipe.m_ingredients.Count; ++i)
        {
            ingredient = m_currentRecipe.m_ingredients[i];
            ingredientQuantity = m_currentRecipe.m_ingredientCount[i];
            m_inventory[ingredient.m_name] -= ingredientQuantity;
        }
        UpdateAvailableIngredients();
    }

    public void Cook()
    {
        bool success = true;
        Ingredient ingredient;
        int ingredientQuantity;
        for (int i = 0; i < m_currentRecipe.m_ingredients.Count; ++i)
        {
            ingredient = m_currentRecipe.m_ingredients[i];
            ingredientQuantity = m_currentRecipe.m_ingredientCount[i];
            if (!(m_inventory.TryGetValue(ingredient.m_name, out int requiredQuantity) && requiredQuantity >= ingredientQuantity))
            {
                success = false;
            }
        }

        if (success)
        {
            CookRecipe();
        }

    }
    
}
