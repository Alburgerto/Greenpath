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

    private bool m_textAnimating;
    private int m_currentRecipeIndex;
    private Recipe m_currentRecipe { get { return m_recipes[m_currentRecipeIndex]; } }
    private Dictionary<string, int> m_inventory = new Dictionary<string, int>(); // All items (ingredients for now) collected and available to use

    private void Awake()
    {
        m_textAnimating = false;
        m_currentRecipeIndex = 0;
    }

    private void Start()
    {
        SetupRecipeGUI();
    }

    private void Update()
    {
        if (m_textAnimating) { return; }

        int newRecipeIndex = m_currentRecipeIndex;
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
        }

        // If another recipe was selected
        if (newRecipeIndex != m_currentRecipeIndex)
        {
            StartCoroutine(FadeOutText());
        }
    }

    public void SetupRecipeGUI()
    {
        ResetGUI();
        Color32 color = m_name.color;
        color.a = 0;

        m_name.color = color;
        m_description.color = color;
        m_ingredients.color = color;
        m_availableQuantities.color = color;
        m_slashes.color = color;
        m_requiredQuantities.color = color;

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
        StartCoroutine(FadeInText());
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

    // Input para cambiar de página -> StartCoroutine(FadeInText()); -> Cambiar de texto
    public IEnumerator FadeInText()
    {
        m_textAnimating = true;
        Color32 color = m_name.color;
        color.a = 0;
        while (color.a < 255)
        {
            color.a += 17;

            m_name.color = color;
            m_description.color = color;
            m_ingredients.color = color;
            m_availableQuantities.color = color;
            m_slashes.color = color;
            m_requiredQuantities.color = color;

            yield return new WaitForSeconds(0.01f);
        }
        // actualizar texto
        m_textAnimating = false;
    }

    public IEnumerator FadeOutText()
    {
        m_textAnimating = true;
        Color32 color = m_name.color;
        color.a = 255;
        while (color.a > 0)
        {
            color.a -= 17;

            m_name.color = color;
            m_description.color = color;
            m_ingredients.color = color;
            m_availableQuantities.color = color;
            m_slashes.color = color;
            m_requiredQuantities.color = color;

            yield return new WaitForSeconds(0.01f);
        }
        m_textAnimating = false;
        SetupRecipeGUI();
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

    //public IEnumerator TextAnimation()
    //{
    //    m_textAnimating = true;

    //    TMP_TextInfo name = m_name.textInfo;
    //    TMP_TextInfo description = m_description.textInfo;
    //    TMP_TextInfo ingredients = m_ingredients.textInfo;
    //    TMP_TextInfo availableQuantities = m_availableQuantities.textInfo;
    //    TMP_TextInfo slashes = m_slashes.textInfo;
    //    TMP_TextInfo requiredQuantities = m_requiredQuantities.textInfo;

    //    List<TMP_TextInfo> textList = new List<TMP_TextInfo> { name, description, ingredients, availableQuantities, slashes, requiredQuantities };
    //    List<int> animatedChars = new List<int>();
    //    int totalCharCount = name.characterCount + description.characterCount + ingredients.characterCount + availableQuantities.characterCount + slashes.characterCount + requiredQuantities.characterCount;
    //    int i = 0, sizeAcum = 0, stringPos = 0;

    //    while (i < totalCharCount)
    //    {
    //        int charPosition = Random.Range(0, totalCharCount);
    //        if (animatedChars.Contains(charPosition)) { continue; }
    //        sizeAcum = 0;
    //        stringPos = 0;
    //        while (stringPos < textList.Count)
    //        {
    //            sizeAcum += textList[stringPos].characterCount;
    //            if (sizeAcum >= charPosition)
    //            {
    //                int localPosition = sizeAcum - charPosition;
    
    //                TMP_TextInfo text = textList[stringPos];
    //                TMP_CharacterInfo character = text.characterInfo[localPosition];

    //                // Character already animated
    //                if (character.color.a != 255)
    //                {
    //                    break;
    //                }

    //                while (character.color.a >= 0)
    //                {
    //                    character.color.a -= 75;
    //                    yield return new WaitForSeconds(0.00001f);
    //                }
    //                animatedChars.Add(charPosition);
    //                break;
    //            }
    //            else
    //            {
    //                ++stringPos;
    //            }
    //        }
    //        i++;
    //    }

    //    SetupRecipeGUI();
    //    m_textAnimating = false;
    //}


}
