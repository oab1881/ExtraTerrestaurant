///         == created by Jake Wardell & Owen Beck  ==
/// Goal : Script handles treatments for Oven, Freezer and Goop
/// Will use delta time to change the color slowly of the ingredient being treated
/// 
/// 
/// Attached to:
/// oven Red
/// freezer blue
/// goop green
/// 
///    
///    Changes:
///    11/08/24 : Jake & Owen : Created script
///    11/13/24 : Jake : Switched from list to dictionary
///    11/14/24 : Jake : Removed count for items
///    
///     Issues:
///     -When I an item is removed it's timer will still be there removing a timer at 1 second left
///     then reinsetting it will keep oringinal time
///    
///    

using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class LayerTreatment : MonoBehaviour
{
    //Stores the storage script
    [SerializeField]
    Storage storageScript;  //Reference to Storage

    LayerState changeType;  // Reference to FoodData - LayerState enum
    Color changeColor;      // Color that the ingredient will change to (Red, Blue, Green)
    string prepMethod;      // Name of the prepMethod being applied to the food

    public PhysicsMaterial2D bouncyMaterial;   //Used to make gooped objects bouncy
    public PhysicsMaterial2D noBounce;   //Used to remove bounciness for frozen objects


    //Dictionary gameobject/float used to keep track of timers on treated food
    [SerializeField]
    Dictionary<GameObject, float> timers; 

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
    }

    private void Update()
    {
        for (int i = 0; i < storageScript.currentCapacity; i++)
        {
            //If the current # of ingredients being stored < current capacity of objects that can be stored
            //Checks to make sure there isn't already a timer attached to the object
                if (storageScript.StoredItem[i].GetComponent<FoodData>().CurrentState == LayerState.Base &&
                !timers.TryGetValue(storageScript.StoredItem[i].gameObject, out float f)) { 
                
                CreateTimer(storageScript.StoredItem[i].gameObject);      //Create a timer for this ingredient

                //Plays sound effect when something is added to the goop
                //Gets the audio manager
                AudioPlayer audioPlayer = GameObject.Find("AudioManager").GetComponent<AudioPlayer>();

                //Depending on type of storage plays a sound
                if(gameObject.name == "goop green")
                {
                    //Plays the sound effect
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
        //Creates a key value for the current object
        timers.Add(timerObject,5f); //Create 5 second timer
    }

    //Check to see if timer has completed
    private void CheckTimer()
    {
        //For # of timers
        for(int i = 0; i < storageScript.currentCapacity; i++)
        {
            //Trys to get a timer for the object
            if (timers.TryGetValue(storageScript.StoredItem[i].gameObject, out float fTimerTime))
            {
                //If timer ends
                if (fTimerTime < 0.0f)
                {
                    //Changes the state of the food and changes the prepname
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
                }
            }
        }
    }

    //Loop thru timer using delta time
    private void DecreaseTimer()
    {
        //Goes through every object in storage and tr
        for(int i = 0; i < storageScript.currentCapacity; i++)
        {
            if (timers.TryGetValue(storageScript.StoredItem[i].gameObject,out float fTimer))
            {
                timers[storageScript.StoredItem[i].gameObject] -= Time.deltaTime;    //Subtract timer's length (5s) by Time.deltaTime
            }
        }
    }
}
