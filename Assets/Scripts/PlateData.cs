/*
 ==== Created by Jake Wardell 10/01/24 ====

Holds information on the plate such as list of all ingriedients

Changelog:
    - Created script : 10/01/24 : Jake
    - Changed style of list to match score manager : 10/17/24 : Jake
    - Added a property for size of list : 10/17/24 : Jake
    - Updated to use GameObjects instead of strings : Chris
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateData : MonoBehaviour
{
    //List to store the different ingredients on the plate
    public List<FoodData> ingredients = new List<FoodData>();

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
    /// <param name="food">The food item to be added to the ingredients list</param>
    public void AddIngredient(GameObject food)
    {
        ingredients.Add(food.GetComponent<FoodData>());
    }
}
