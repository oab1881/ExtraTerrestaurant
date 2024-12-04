using System;
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
public class TutorialScore : MonoBehaviour
{
    private int score;
    private Dictionary<string, string> ingredients = new Dictionary<string, string>();
    private Dictionary<string, string> preparationMethods = new Dictionary<string, string>();

    //Reference to ordering script  (OWEN)
    NewOrdering orderRef = new NewOrdering();

    [SerializeField]
    GameObject kitchenMonitor;

    [SerializeField]
    PlateData plateData;

    [SerializeField]
    NewOrdering alienOrder;

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

    //*** THIS IS A VERY SIMPLIFIED VERSION FOR TUTORIAL PURPOSES ONLY******
    private List<int> ScoreDish(List<List<string>> customer_order, List<FoodData> player_food)
    {

        Debug.Log($"alienOrder is null: {alienOrder == null}");
        Debug.Log($"alienOrder.order is null: {alienOrder?.order == null}");
        Debug.Log($"plateData is null: {plateData == null}");
        Debug.Log($"plateData.ingredients is null: {plateData?.ingredients == null}");

        List<int> results = new List<int>();

        // Simplified check for tutorial mode with hardcoded "A"
        foreach (List<string> component in customer_order)
        {
            int scoreValue = 0;
            foreach (FoodData food in player_food)
            {
                if (component[0] == "A" && ingredients[food.FoodName] == "A")
                {
                    scoreValue = 2; // Correct ingredient
                }
                else
                {
                    scoreValue = 0; // Incorrect
                }
            }
            results.Add(scoreValue);
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
            if(customer_order.Count > 1)
            {
                result -= 1;
            }
        }

        score += result;
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

        score += result;
        return result;
    }

    /*
     * Uses the monitor in the kitchen to display feedback to the player, using colors
     * to indicate how the player did in comparison to the order received. This will also
     * return a boolean stating whether or not the player's dish was perfect.
     */
    public bool DisplayScore()
    {
        string displayText = "";
        
        TextMeshProUGUI scoreText = kitchenMonitor.GetComponentInChildren<TextMeshProUGUI>();
        //List<int> results = ScoreDish(alienOrder.order, plateData.ingredients);
        //foreach (int i in results) 
        //{
        //    Debug.Log(i);
        //}
        List<int> results = ScoreDish(alienOrder.order, plateData.ingredients);
        int testScore = 0;
        foreach (int i in results)
        {
            Debug.Log(i);
            testScore += i;
        }
        for (int i = 0; i < alienOrder.order.Count; i++)
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

                case 2: // Perfect score for matching "A"
                    string display3 = $"<color=#00FF00>{alienOrder.order[i][0]}</color>";
                    displayText = string.Concat(displayText, display3);
                    break;

                default:
                    Debug.Log("Something went wrong...");
                    break;
            }
        }
        scoreText.text = displayText;
        return isPerfect(alienOrder.order.Count, testScore);
    }

    // Sets the scoring method's plate data to the new plate instance
    public void CleanPlate(GameObject newPlate)
    {
        plateData = newPlate.GetComponent<PlateData>();
        
        //Testing to find a way to loop alien spawning
        orderRef.SpawnNewAlien();
        orderRef.Update();
        score = 0;
    }

    // Determines if a dish was perfect or not.
    public bool isPerfect(int orderCount, int score)
    {
        if((2 * orderCount) == score)
        {
            return true;
        } 
        else
        {
            return false;
        }
    }
}
