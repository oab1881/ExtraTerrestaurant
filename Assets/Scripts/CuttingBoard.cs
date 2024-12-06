using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

/*
 * Adds the logic for snapping food to the cutting board and allows the player to cut food if they use the knife.
 * Functionally, almost identical to the mortar method, but works on the cutting board and the knife.
 * 
 * Change Log:
 *  - Inital push : Chris
 */
public class CuttingBoard : MonoBehaviour
{
    // Stores how many times the knife has come into contact with the ingredient on the table
    int cutCount = 0;

    //Stores the storage script
    [SerializeField]
    Storage storageScript;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        /*
         * When cutting, first checks to see if there is something inside of the
         * cutting board storage script, then checks if the ingredient's "chopped"
         * variable isn't null (rendering it uncuttable). If both are true, then
         * the game verifies that the player is holding a knife.
         */
        if (storageScript.StoredItem[0] != null &&
            //storageScript.StoredItem[0].GetComponent<FoodData>().Chopped != null &&
            collision.name == "knife")
        {

            Debug.Log("Cut!");
            //Increases the cut count on every enter
            cutCount++;

            // GameObject.Find("AudioManager").GetComponent<AudioPlayer>().PlaySoundEffect("mortar");

            // After the player cuts the object 3 times
            /*if (cutCount == 3)
            {
                //Temp gameobject that is new type (Which is the cut GameObject)
                GameObject newType = storageScript.StoredItem[0].GetComponent<FoodData>().Chopped;
                Debug.Log(newType);
                //Resets the count
                cutCount = 0;

                //Destroys current item in storage and instanitates new one in it's place
                // *** Prefab now gets created but does not get stored ***
                //storageScript.RemoveItem(0, true, false);
                GameObject temp;
                temp = Instantiate(newType, transform);
                temp.GetComponent<FoodData>().PrepareFood("chopped");
                //storageScript.StoreItem(temp);
            }*/
        }
    }
}
