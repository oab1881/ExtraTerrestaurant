/*
 ==== Created by Jake Wardell 10/17/24 ====

Makes it so food can be crushed not fully implemented

Changelog:
    -Created script: 10/17/24 : Jake
    -Fixing bugs from playtes; Making it switch to smashed version(Broken); Added particle system : 10/27/24 : Jake
    
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mortar : MonoBehaviour
{
    //Stores how many times the pestel has been smashed
    int pestelCount;

    //Stores the storage script
    [SerializeField]
    Storage storageScript;

    //Particles for on smashes
    [SerializeField]
    ParticleSystem particles;



    private void Start()
    {
        //On the start sets the pestel count to 0
        pestelCount= 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //For using the pestel : When it enters the mortar collider
        //Do a quick check to see if there is in fact something at 0 index
        //Then another check on food data, which checks if 
        if(collision.name == "pestle" && storageScript.StoredItem[0] != null &&
            storageScript.StoredItem[0].GetComponent<FoodData>().Crushed != null)
        {
            //Debug.Log("Smash!!!");
            //Increases the pestel count on every enter
            pestelCount++;
            //Plays particles on smash
            particles.Play();

            //If the count is equal to 5
            if(pestelCount == 5)
            {
                //Temp gameobject that is new type(Which is the crushed GameObject)
                GameObject newType = storageScript.StoredItem[0].GetComponent<FoodData>().Crushed;
                Debug.Log(newType);
                //Resets the count
                pestelCount = 0;

                //Destorys current item in storage and instanitates new one in it's place
                // *** Note For some reason doesn't create new prefab ***
                storageScript.RemoveItem(0);
                Instantiate(newType, transform);
                //storageScript.StoreItem();
            }

        }
    }
}
