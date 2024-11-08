using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

//Script handles treatments for Oven, Freezer and Goop
//Will use delta time to change the color slowly of the ingredient being treated
//Written by Jake + Owen

public class LayerTreatment : MonoBehaviour
{
    //Stores the storage script
    [SerializeField]
    Storage storageScript;  //Reference to Storage

    int currentCount;   //Current count of ingredients being stored

    LayerState changeType;  //Reference to FoodData - LayerState enum
    Color changeColor;      //Color that the ingredient will change to (Red, Blue, Green)

    public PhysicsMaterial2D bouncyMaterial;   //Used to make gooped objects bouncy

    List<float> timers = new List<float>(); //List of timers used to keep track of timers on treated food

    private void Start()
    {
        if(gameObject.name == "oven red")   //If ingredient goes in oven
        {
            changeType = LayerState.Cooked; //Change its state to cooked
            changeColor = new Color32(110, 38, 14, 255);         //Change color to red
        }
        else if(gameObject.name == "freezer blue")  //If ingredient goes in freezer
        {
            changeType = LayerState.Frozen; //Change its state to frozen
            changeColor = Color.blue;       //Change color to blue
        }
        else if(gameObject.name == "goop green")   //If ingredient goes in goop
        {
            changeType = LayerState.Gooped;     //Change state to gooped
            changeColor = Color.green;          //Change color to Green
        }
        currentCount = storageScript.currentCapacity;   //Get current count of ingredients being stored
    }

    private void Update()
    {
        
        if(currentCount < storageScript.currentCapacity)    //If the current # of ingredients being stored < current capacity of objects that can be stored
        {
            currentCount++;     //Increase # of ingredients being stored
            CreateTimer();      //Create a timer for this ingredient
        }
        DecreaseTimer();        //Call Decrease timer every frame
        CheckTimer();           //Call CheckTimer to see if timer ended
        
    }

    //Create a timer for an instance of a treated food
    private void CreateTimer()
    {
        //For # of ingredients stored
        for(int i = 0; i < storageScript.currentCapacity; i++)
        {
            //If the stored ingredient is in it's base state (untreated)
            if (storageScript.StoredItem[i].GetComponent<FoodData>().CurrentState == LayerState.Base)
            {
                timers.Add(5f); //Create 5 second timer
            }
        }
    }

    //Check to see if timer has completed
    private void CheckTimer()
    {
        //For # of timers
        for(int i = 0; i < timers.Count; i++)
        {   
            //If timer ends
            if (timers[i] < 0.0f)
            {
                storageScript.StoredItem[i].GetComponent<FoodData>().ChangeState(changeType,changeColor);  //Change food's type and color

                //If the changeType is Gooped, add the bouncy material
                if (changeType == LayerState.Gooped && bouncyMaterial != null)
                {
                    Rigidbody2D ingredientRB = storageScript.StoredItem[i].GetComponent<Rigidbody2D>();
                    //Collider2D ingredientCollider = storageScript.StoredItem[i].GetComponent<Collider2D>();
                    if (ingredientRB != null)
                    {
                        ingredientRB.sharedMaterial = bouncyMaterial; //Apply the bouncy material
                    }
                }

                timers.RemoveAt(i); //Remove timer from list
                i--; // Adjust the index due to timer removal
            }
        }
    }

    //Loop thru timer using delta time
    private void DecreaseTimer()
    {
        for(int i = 0; i < timers.Count; i++)
        {
            timers[i] -= Time.deltaTime;    //Subtract timer's length (5s) by Time.deltaTime
        }
    }
}
