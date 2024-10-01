using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int score;
    private float timer;
    private Dictionary<string, List<string>> ingredients;

    private enum preparation_methods
    {
    }

    private void Start()
    {
        // Create the Dictionary of ingredients
        // Hardcoded ingredient values
        List<string> tentacle = new List<string>() { "A", };
        List<string> leaf = new List<string>() { "B", };
        List<string> mineral = new List<string>() { "C", };
        List<string> gooball = new List<string>() { "D", };
        List<string> sheet = new List<string>() { "E", };

        // Add ingredient values to ingredients dictionary
        ingredients.Add("tentacle", tentacle);
        ingredients.Add("leaf", leaf);
        ingredients.Add("mineral", mineral);
        ingredients.Add("gooball", gooball);
        ingredients.Add("sheet", sheet);

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

    }

    /*
        Compares the food the player makes to the food the customer orders
        Takes customer's order and the player's prepared food and compares them to see if the 
        ingredients used matched the ones that the customer asked for, and if they were prepared the correct way

        Returns an int representing the score the player earned on the order
    */
    private int CompareFood(List<string> customer_order, List<string> player_food)
    {
        int score = 0;

        List<string> food_components = player_food;

        foreach(string c_ingredient in customer_order)
        {
            for (int j = 0; j < food_components.Count; j++)
            {
                if (ingredients[c_ingredient].Contains(food_components[j]))
                {
                    score += 25;
                    food_components.RemoveAt(j);
                    break;
                }
            }
        }
        return score;
    }
}
