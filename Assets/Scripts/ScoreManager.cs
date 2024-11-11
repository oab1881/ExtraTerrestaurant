using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TMPro;
using UnityEngine;

/*
 * Handles the scoring of dishes, as well as displaying them on the monitor when the player
 * turns in a dish.
 */
public class ScoreManager : MonoBehaviour
{
    private int score;
    private Dictionary<string, string> ingredients = new Dictionary<string, string>();
    private Dictionary<string, string> preparationMethods = new Dictionary<string, string>();

    [SerializeField]
    GameObject kitchenMonitor;

    [SerializeField]
    PlateData plateData;

    [SerializeField]
    OrderingAlien alienOrder;

    private void Start()
    {
        // Create the Dictionary of ingredients
        ingredients.Add("tentacle", "A");
        ingredients.Add("leaf", "B");
        ingredients.Add("crystal", "C");
        ingredients.Add("egg", "D");
        ingredients.Add("sheet", "E");

        // Create the Dictionary of preparation_methods
        preparationMethods.Add("cooked", "H");
        preparationMethods.Add("frozen", "G");
        preparationMethods.Add("chopped", "M");
        preparationMethods.Add("grinded", "O");
        preparationMethods.Add("gooped", "J");

        // Set Score to 0
        score = 0;
    }

    /*
        Compares the food the player makes to the food the customer orders
        Takes customer's order and the player's food and compares them to see if the 
        ingredients used matched the ones that the customer asked for, and if they were prepared the correct way.
        
        For scoring prepared ingredients specifically: Since the customer could order the same ingredient but  
        prepared in different ways, when scoring a prepared ingredient, the method will save the score of the
        ingredient that scores the highest (i.e. the correct ingredient and correct preparation method) and that
        ingredient's location in the "ingredients_used" list. After all components of the player's dish are
        looped through, the method will then take the highest scoring result and add that to the total score
        before removing that specific component off of the list.

        Returns an int representing the score the player earned on the order
    */
    private List<int> ScoreDish(List<List<string>> customer_order, List<FoodData> player_food)
    {
        List<int> results = new List<int>();

        // Create modifiable list based off of the food on the plate
        //List<List<string>> ingredientsUsed = player_food;

        // Loop through each component of the customer's order
        foreach(List<string> component in customer_order)
        {
            int preppedHighestRating = 0;
            int preppedHighestIndex = 0;

            for (int i = 0; i < player_food.Count; i++)
            {
                // Evaluate a single raw ingredient
                if (component.Count == 1)
                {
                    int res = EvaluateRaw(component, player_food[i]);
                    if (res > preppedHighestRating)
                    {
                        preppedHighestRating = res;
                        continue;
                    }
                }
                // Evaluate a single prepared ingredient
                else if (component.Count == 2) 
                {
                    int res = EvaluatePrepped(component, player_food[i]);
                    if(res > preppedHighestRating)
                    {
                        preppedHighestRating = res;
                        preppedHighestIndex = i;
                    }
                }
            }

            results.Add(preppedHighestRating);
            //ingredientsUsed.RemoveAt(preppedHighestIndex);
        }
        return results;
    }

    // Used to evaluate an ingredient that has no preparation methods used on it
    private int EvaluateRaw(List<string> customer_order, FoodData used_ingredient)
    {
        int result = 0;
        string value = customer_order[0].Trim();

        // Use the ingredient the player used as the key to the ingredients dictionary and see if the item the customer ordered matches the value
        if (ingredients[used_ingredient.FoodName] == value)
        { 
            result += 2;
        }

        return result;
    }

    // Used to evaluate an ingredient that has a single preparation method used on it.
    private int EvaluatePrepped(List<string> customer_order, FoodData used_ingredient)
    {
        int result = 0;
        string ingredientValue = customer_order[0].Trim();
        string prepValue = customer_order[1].Trim();

        // Use the ingredient the player used as the key to the ingredients dictionary and see if the item the customer ordered matches the value
        if (ingredients[used_ingredient.FoodName] == ingredientValue)
        {
            result += 1;
            // If this test is passed, do the same with the preparation method.
            if (preparationMethods.ContainsKey(used_ingredient.PrepName) && preparationMethods[used_ingredient.PrepName] == prepValue)
            {
                result += 1;
            }
        }

        return result;
    }

    public void DisplayScore()
    {
        string displayText = "";
        
        TextMeshProUGUI scoreText = kitchenMonitor.GetComponentInChildren<TextMeshProUGUI>();
        List<int> results = ScoreDish(alienOrder.order, plateData.ingredients);
        foreach (int i in results) 
        {
            Debug.Log(i);
        }
        for(int i = 0; i < alienOrder.order.Count; i++)
        {
            switch (results[i]) 
            {
                case 0:
                    string text1 = "";
                    foreach(string letter in alienOrder.order[i])
                    {
                        text1 = string.Concat(text1, letter.Replace(" ", ""));
                    }
                    string display1 = $"<color=#FF0000>{text1}</color>";
                    displayText = string.Concat(displayText, display1);
                    break;
        
                case 1:
                    string text2 = "";
                    foreach (string letter in alienOrder.order[i])
                    {
                        text2 = string.Concat(text2, letter.Replace(" ", ""));
                    }
                    string display2 = $"<color=#FFEA00>{text2}</color>";
                    displayText = string.Concat(displayText, display2);
                    break;
        
                case 2:
                    string text3 = "";
                    foreach (string letter in alienOrder.order[i])
                    {
                        text3 = string.Concat(text3, letter.Replace(" ", ""));
                    }
                    string display3 = $"<color=#00FF00>{text3}</color>";
                    displayText = string.Concat(displayText, display3);
                    break;
        
                default:
                    Debug.Log("Something went wrong...");
                    break;
            }
        }
        scoreText.text = displayText;
    }
}
