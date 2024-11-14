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

    [SerializeField]
    int currentCount;   //Current count of ingredients being stored

    LayerState changeType;  // Reference to FoodData - LayerState enum
    Color changeColor;      // Color that the ingredient will change to (Red, Blue, Green)
    string prepMethod;      // Name of the prepMethod being applied to the food

    public PhysicsMaterial2D bouncyMaterial;   //Used to make gooped objects bouncy
    public PhysicsMaterial2D noBounce;   //Used to remove bounciness for frozen objects

    [SerializeField]
    Dictionary<GameObject, float> timers; //List of timers used to keep track of timers on treated food

    private void Start()
    {
        timers = new Dictionary<GameObject, float>();


        if (gameObject.name == "oven red")   //If ingredient goes in oven
        {
            changeType = LayerState.Cooked; //Change its state to cooked
            changeColor = new Color32(110, 38, 14, 255);         //Change color to red
            prepMethod = "cooked";          // Change food prepMethod to cooked
        }
        else if(gameObject.name == "freezer blue")  //If ingredient goes in freezer
        {
            changeType = LayerState.Frozen; //Change its state to frozen
            changeColor = Color.blue;       //Change color to blue
            prepMethod = "frozed";          // Change food prepMethod to frozen
        }
        else if(gameObject.name == "goop green")   //If ingredient goes in goop
        {
            changeType = LayerState.Gooped;     //Change state to gooped
            changeColor = Color.green;          //Change color to Green
            prepMethod = "gooped";              // Change food prepMethod to gooped
        }
        currentCount = storageScript.currentCapacity;   //Get current count of ingredients being stored
    }

    private void Update()
    {
        for (int i = 0; i < storageScript.currentCapacity; i++)
        {
            if (storageScript.StoredItem[i].GetComponent<FoodData>().CurrentState == LayerState.Base &&
                !timers.TryGetValue(storageScript.StoredItem[i].gameObject, out float f))    //If the current # of ingredients being stored < current capacity of objects that can be stored
            {
                CreateTimer(storageScript.StoredItem[i].gameObject);      //Create a timer for this ingredient

                //Plays sound effect when something is added to the goop
                
                AudioPlayer audioPlayer = GameObject.Find("AudioManager").GetComponent<AudioPlayer>();
                if(gameObject.name == "goop green")
                {
                    audioPlayer.PlaySoundEffect("item_inserted_into_slime");
                }
            }
        }
        DecreaseTimer();        //Call Decrease timer every frame
        CheckTimer();           //Call CheckTimer to see if timer ended
        
    }

    //Create a timer for an instance of a treated food
    private void CreateTimer(GameObject timerObject)
    {
        timers.Add(timerObject,5f); //Create 5 second timer
    }

    //Check to see if timer has completed
    private void CheckTimer()
    {
        //For # of timers
        for(int i = 0; i < storageScript.currentCapacity; i++)
        {
            if (timers.TryGetValue(storageScript.StoredItem[i].gameObject, out float fTimerTime))
            {
                //If timer ends
                if (fTimerTime < 0.0f)
                {
                    storageScript.StoredItem[i].GetComponent<FoodData>().ChangeState(changeType, changeColor);  //Change food's type and color
                    storageScript.StoredItem[i].GetComponent<FoodData>().PrepareFood(prepMethod);               // Change food prep method
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

                    //If the changeType is Frozen, remove the bouncy material
                    if (changeType == LayerState.Frozen && bouncyMaterial != null)
                    {
                        Rigidbody2D ingredientRB = storageScript.StoredItem[i].GetComponent<Rigidbody2D>();
                        if (ingredientRB != null)
                        {
                            ingredientRB.sharedMaterial = noBounce; //Remove bouncy script
                        }
                    }
                    //currentCount--;
                }
            }
        }
    }

    //Loop thru timer using delta time
    private void DecreaseTimer()
    {
        for(int i = 0; i < storageScript.currentCapacity; i++)
        {
            if (timers.TryGetValue(storageScript.StoredItem[i].gameObject,out float f))
            {
                timers[storageScript.StoredItem[i].gameObject] -= Time.deltaTime;    //Subtract timer's length (5s) by Time.deltaTime
            }
        }
    }
}
