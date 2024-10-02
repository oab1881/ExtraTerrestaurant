/*
 ==== Created by Jake Wardell 10/01/24 ====

Holds information on the plate such as list of all ingriedients

Changelog:
    -Created script : 10/01/24
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateData : MonoBehaviour
{
    //List to store ingredients
    List<string> ingredients = new List<string>();

    /// <summary>
    /// Public function to add ingredients to the plate
    /// </summary>
    /// <param name="name">Name of food item to add</param>
    public void AddIngredient(string name)
    {
        ingredients.Add(name);
    }
    
}
