/*
 ==== Created by Jake Wardell 10/17/24 ====

Makes it so food can be crushed not fully implemented

Changelog:
    -Created script: 10/17/24 : Jake
    -Fixing bugs from playtes; Making it switch to smashed version(Broken); Added particle system : 10/27/24 : Jake
    -Added sound effects for mortar : 11/11/24 : Jake
    -Changed audio stuff based on notes I recieved : 12/03/24 : Jake
    
*/
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class OldMortar : MonoBehaviour
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
        pestelCount = 0;
        //storageScript.maxCapacity = 1;
        //storageScript.positions[0] = gameObject.transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //This collision detection only happens with smaller circle collider b/c the main
        //polygon collider excludes pestle layer which only contains pestle


        //For using the pestel : When it enters the mortar collider
        //Do a quick check to see if there is in fact something at 0 index
        //Then another check on food data, which checks if 
        if (storageScript.StoredItem[0] != null &&
            //storageScript.StoredItem[0].GetComponent<FoodData>().Crushed != null &&
            collision.name == "pestle")
        {

            //Debug.Log("Smash!!!");
            //Increases the pestel count on every enter
            pestelCount++;


            //Another version to change the color apparently it's better 
            //ParticleSystem.MainModule main = GetComponent<ParticleSystem>().main;
            //main.startColor = storageScript.StoredItem[0].GetComponent<FoodData>().FoodColor;

            //Plays particles on smash
            //Changes the color
            particles.startColor = storageScript.StoredItem[0].GetComponent<FoodData>().FoodColor;
            particles.Play();
            GameObject.Find("AudioManager(Quick)").GetComponent<AudioPlayer>().PlaySoundEffect("mortar", 0);

            //If the count is equal to 3
            /*if (pestelCount == 3)
            {
                //Temp gameobject that is new type(Which is the crushed GameObject)
                GameObject newType = storageScript.StoredItem[0].GetComponent<FoodData>().Crushed;
                Debug.Log(newType);
                //Resets the count
                pestelCount = 0;

                //Destorys current item in storage and instanitates new one in it's place
                // *** Prefab now gets created but does not get stored ***
                //storageScript.RemoveItem(0, true, false);
                GameObject temp;
                temp = Instantiate(newType, transform);
                temp.GetComponent<FoodData>().PrepareFood("grinded");
                //storageScript.StoreItem(temp);
            }*/
        }
    }
}
