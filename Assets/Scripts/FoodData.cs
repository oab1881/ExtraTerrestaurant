/*
 ==== Created by Jake Wardell 10/27/24 ====

Stores important information on food

Changelog:
    - Created script: 10/27/24 : Jake
    - Added values for a food item's name and booleans for if they are crushed, cooked, or cut
    - Added enum for the layerState; Function to change states : Jake/Owen : 11/8/24
    - Added functions to change the prep method of the food item : Chris
    - Condensed prep method functions into a single function that takes a string : Chris
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum LayerState
{
    Base,
    Gooped,
    Cooked,
    Frozen,
}
public class FoodData : MonoBehaviour
{
    [SerializeField]
    string foodName;
    string prepName = "";

    // If any of these are null that means this item
    // can't be transformed that way ie crushed food has no
    // crushed gameobject it is already crushed
    [SerializeField]
    GameObject crushed;
    [SerializeField]
    GameObject cut;

    LayerState currentState;

    [SerializeField]
    Color foodColor;

    /// <summary>
    /// Gets the crushed prefab that replaces it
    /// </summary>
    public GameObject Crushed
    {
        get { return crushed; }
    }
    public GameObject Chopped
    {
        get { return cut; }
    }

    public void PrepareFood(string prepMethod)
    {
        prepName = prepMethod;
    }

    //Gets food color
    public Color FoodColor
    {
        get { return foodColor; }
    }

    //gets the food name
    public string FoodName
    {
        get { return foodName; }
    }

    //Gets prep type
    public string PrepName
    {
        get { return prepName; }
    }

    //Stores one of four types goop, cooked, frozen, or base
    //Is related to the current item. Oven, freezer, and goop change
    //this state.
    public LayerState CurrentState
    {
        get { return currentState; }
        set { currentState = value; }
    }

    //On start all items are set to base.
    private void Start()
    {
        currentState = LayerState.Base;
    }

    /// <summary>
    /// Changes the state of an object sets name and new color
    /// </summary>
    /// <param name="newState">New state change</param>
    /// <param name="newColor">New color to change to</param>
    public void ChangeState(LayerState newState, Color newColor)
    {
        //Setting new state and color
        currentState = newState;
        gameObject.GetComponent<SpriteRenderer>().color = newColor;
    }
}