/*
 ==== Created by Jake Wardell 10/01/24 ====

Holds information on the plate such as list of all ingriedients

Changelog:
    -Created script : 10/01/24 : Jake
    -Changed style of list to match score manager : 10/17/24 : Jake
    -Added a property for size of list : 10/17/24 : Jake
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateData : MonoBehaviour
{
    //List to store list of strings
    //The list of strings is for {"Ingredient", "Effect"} 
    //Ex - {"tentacle", "Cooked"}
    List<List<string>> ingredients = new List<List<string>>();

    /// <summary>
    /// Returns size of the list of ingredients
    /// </summary>
    public int IngriedentsListSize
    {
        get
        {
            return ingredients.Count;
        }
    }

    /// <summary>
    /// Public function to add ingredients/effects to the plate
    /// </summary>
    /// <param name="ingredient1">Ingriedent to be added</param>
    /// <param name="effect">The effect added to the ingreident</param>
    public void AddIngredient(string ingredient1, string effect)
    {
        ingredients.Add(new List<string> { ingredient1, effect});
    }
    
}
