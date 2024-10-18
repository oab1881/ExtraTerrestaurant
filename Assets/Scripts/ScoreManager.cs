using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int score;
    private Dictionary<string, string> ingredients;
    private Dictionary<string, string> preparation_methods;

    private void Start()
    {
        // Create the Dictionary of ingredients
        ingredients.Add("tentacle", "A");
        ingredients.Add("leaf", "B");
        ingredients.Add("crystal", "C");
        ingredients.Add("egg", "D");
        ingredients.Add("sheet", "E");

        // Create the Dictionary of preparation_methods
        preparation_methods.Add("cooked", "F");
        preparation_methods.Add("frozen", "G");
        preparation_methods.Add("chopped", "H");
        preparation_methods.Add("grinded", "I");
        preparation_methods.Add("gooped", "J");

        // Set Score to 0
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    /*
        Compares the food the player makes to the food the customer orders
        Takes customer's order and the player's food and compares them to see if the 
        ingredients used matched the ones that the customer asked for, and if they were prepared the correct way.

        Returns an int representing the score the player earned on the order
    */
    private int ScoreDish(List<List<string>> customer_order, List<List<string>> player_food)
    {
        int totalScore = 0;

        // Create modifiable list based off of the food on the plate
        List<List<string>> ingredients_used = player_food;

        // Loop through each component of the customer's order
        foreach (List<string> component in customer_order)
        {
            int preppedHighest = 0;
            int preppedHighestIndex = 0;

            for (int i = 0; i < ingredients_used.Count; i++)
            {
                // Evaluate a single raw ingredient
                if (component.Count == 1)
                {
                    int score = EvaluateRaw(component, ingredients_used[i]);
                    if (score > 0)
                    {
                        totalScore += score;
                        ingredients_used.RemoveAt(i);
                        break;
                    }
                }
                // Evaluate a single prepared ingredient
                else if (component.Count == 2) 
                {
                    int score = EvaluatePrepped(component, ingredients_used[i]);
                    if(score > preppedHighest)
                    {
                        preppedHighest = score;
                        preppedHighestIndex = i;
                    }
                }
            }

            if (component.Count > 1) 
            {
                totalScore += score;
                ingredients_used.RemoveAt(preppedHighestIndex);
            }
        }

        return totalScore;
    }

    // Used to evaluate an ingredient that has no preparation methods used on it
    private int EvaluateRaw(List<string> customer_order, List<string> used_ingredient)
    {
        int score = 0;

        // Use the ingredient the player used as the key to the ingredients dictionary and see if the item the customer ordered matches the value
        if(ingredients[used_ingredient[0]].Contains(customer_order[0]))
        { 
            score += 25; 
        }

        return score;
    }

    // Used to evaluate an ingredient that has a single preparation method used on it.
    private int EvaluatePrepped(List<string> customer_order, List<string> used_ingredient)
    {
        int score = 0;

        // Use the ingredient the player used as the key to the ingredients dictionary and see if the item the customer ordered matches the value
        if (ingredients[used_ingredient[0]].Contains(customer_order[0]))
        {
            score += 25;
            // If this test is passed, do the same with the preparation method.
            if (ingredients[used_ingredient[1]].Contains(customer_order[1]))
            {
                score += 25;
            }
        }

        return score;
    }
}
