/*
 ==== Created by Jake Wardell 10/27/24 ====

Stores important information on food

Changelog:
    -Created script: 10/27/24 : Jake
    
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodData : MonoBehaviour
{
    //If any of these are null that means this item
    //can't be transformed that way ie crushed food has no
    //crushed gameobject it is already crushed
    [SerializeField]
    GameObject crushed;
    [SerializeField]
    GameObject cut;
    [SerializeField]
    GameObject cooked;

    [SerializeField]
    Color foodColor;
    
    /// <summary>
    /// Gets the crushed prefab that replaces it
    /// </summary>
    public GameObject Crushed
    {
        get { return crushed; }
    }

    public Color FoodColor { get { return foodColor; } }
    
}
