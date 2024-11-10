/*
 ==== Created by Jake Wardell 10/27/24 ====

Stores important information on food

Changelog:
    - Created script: 10/27/24 : Jake
    - Added values for a food item's name and booleans for if they are crushed, cooked, or cut
    - Added enum for the layerState; Function to change states : Jake/Owen : 11/8/24
    
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
    string prepName;

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

    public Color FoodColor 
    { 
        get { return foodColor; } 
    }

    public string FoodName
    {
        get { return foodName; }
    }
    public string PrepName
    {
        get { return PrepName; }
    }

    public LayerState CurrentState
    {
        get { return currentState; }
        set { currentState = value; }
    }

    private void Start()
    {
        currentState= LayerState.Base;
    }


    public void ChangeState(LayerState newState, Color newColor)
    {
        currentState= newState;
        gameObject.GetComponent<SpriteRenderer>().color= newColor;
    }
}
