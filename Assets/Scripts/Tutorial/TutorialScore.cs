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
    TutorialOrdering alienOrder;

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
        List<int> results = new List<int>();

        // Simplified check for tutorial mode with hardcoded "A"
        foreach (List<string> component in customer_order)
        {
            int scoreValue = 0;
            foreach (FoodData food in player_food)
            {
                if (ingredients[food.FoodName] == component[0])
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

    /*
     * Uses the monitor in the kitchen to display feedback to the player, using colors
     * to indicate how the player did in comparison to the order received. This will also
     * return a boolean stating whether or not the player's dish was perfect.
     */
    public bool DisplayScore()
    {
        string displayText = "";
        
        TextMeshProUGUI scoreText = kitchenMonitor.GetComponentInChildren<TextMeshProUGUI>();
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
                    string display1 = $"<color=#FF0000>A</color>";
                    displayText = string.Concat(displayText, display1);
                    break;

                case 1:
                    Debug.Log("How?");
                    break;

                case 2: // Perfect score for matching "A"
                    string display3 = $"<color=#00FF00>A</color>";
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
