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
///    12/03/24 : Jake : Plays sound that are longer & set things up for oven goop and freezer sounds
///    
///     Issues:
///     -When I an item is removed it's timer will still be there removing a timer at 1 second left
///     then reinsetting it will keep oringinal time
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
            prepMethod = "frozen";          // Change food prepMethod to frozen
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
                AudioPlayer audioPlayer = GameObject.Find("AudioManager(Quick)").GetComponent<AudioPlayer>();

                //Depending on type of storage plays a sound

                //Plays the sound effect

                if (changeType == LayerState.Gooped)
                    audioPlayer.PlaySoundEffect("item_inserted_into_slime", 0);
                    
                        
                if (changeType == LayerState.Cooked)
                    audioPlayer.PlaySoundEffect("oven_Open", 0);
                if (changeType == LayerState.Frozen)
                    audioPlayer.PlaySoundEffect("Freezer_open", 0);

            }
        }
        DecreaseTimer();        //Call Decrease timer every frame
        CheckTimer();           //Call CheckTimer to see if timer ended
        CheckSounds();          //Checks if sounds should be playing 
        
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

                    //Gets reference to the LongAudioPlayer
                    AudioPlayer tempAudioPlayerQuick = GameObject.Find("AudioManager(Quick)").GetComponent<AudioPlayer>();
                    AudioPlayer tempAudioPlayerLong = GameObject.Find("AudioManager(Long)").GetComponent<AudioPlayer>();

                    //This makes it so different sounds play during treatments
                    //This works but not as fully intended stops the audio clip then doesn't allow any other to play
                    
                    if (changeType == LayerState.Gooped)
                        tempAudioPlayerLong.StopSound(0);

                    if (changeType == LayerState.Cooked)
                    {
                        tempAudioPlayerLong.StopSound(1);
                        tempAudioPlayerQuick.PlaySoundEffect("treatment_finished_Ding", 0);
                    }

                    if (changeType == LayerState.Frozen)
                        tempAudioPlayerLong.StopSound(2);

                    

                    

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

                    //Removes the timer at the end
                    timers.Remove(storageScript.StoredItem[i]);
                }
            }
        }
    }

    //Loop thru timer using delta time
    private void DecreaseTimer()
    {
        //Goes through every object in storage
        for(int i = 0; i < storageScript.currentCapacity; i++)
        {
            //Attempts to get each timer then checks if that timer is greater then 0 no point subtracting from
            //timers at 0
            if (timers.TryGetValue(storageScript.StoredItem[i].gameObject,out float fTimer) && fTimer > 0)
            {
                timers[storageScript.StoredItem[i].gameObject] -= Time.deltaTime;    //Subtract timer's length (5s) by Time.deltaTime
            }
        }
    }

    private void CheckSounds()
    {
        //Checking if the dictionary has any timers in it meaning a food is being transformed
        if(timers.Count > 0)
        {

            //Gets reference to the LongAudioPlayer
            AudioPlayer tempAudioPlayer = GameObject.Find("AudioManager(Long)").GetComponent<AudioPlayer>();

            //This makes it so different sounds play during treatments
            if (changeType == LayerState.Gooped)
                tempAudioPlayer.playLongSound("slime_treatment", 0);
            if (changeType == LayerState.Cooked)
                tempAudioPlayer.playLongSound("Oven_cooking_Timer", 1);
            if (changeType == LayerState.Frozen)
                tempAudioPlayer.playLongSound("Freezing_treatment", 2);

        }
    }
}
