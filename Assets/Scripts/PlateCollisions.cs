/*
 ==== Created by Jake Wardell 10/01/24 ====

Does collision between the food objects and the plates in the game! Also
between mouse and plate!

Changelog:
    -Created script : 10/01/24
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCollisions : MonoBehaviour
{
    //Stores the object colliding with the plate
    GameObject collidingObject;

    [SerializeField]
    PlateData plateData;


    void Update()
    {
        /* -Handles changing data about the food item 
           -Also sends information on what food item is to the plate data
         */

        //Tests to see if object is null
        //This check is essential to find out if anything is actually colliding
        if (collidingObject != null)
        {
            //Something can be colliding but maybe it shouldn't go on the plate...
            //So we wait for mouse to let off of
            if (Input.GetMouseButtonUp(0))
            {
                //==== We can change the data about the food gameobject to just put it on
                //the plate ====
                collidingObject.transform.SetParent(transform, false);
                collidingObject.GetComponent<DragAndDrop>().CanDragAndDrop = false;
                collidingObject.transform.localScale = collidingObject.transform.localScale / 2;
                collidingObject.transform.localPosition = new Vector3(2, 2, 0);

                // ==== Then for each food item we can test it's name ====
                //And send it's data to the list of food items on the plate


                //For testing the tentacle food item
                if (collidingObject.name == "tentacle_food(Clone)")
                {
                    //Adds to list with corresponding food value obtained from Score
                    //Manager
                    plateData.AddIngriedint("tentacle");
                }
                

                else if(collidingObject.name == "leaf_food(Clone)")
                {
                    //Adds to list with corresponding food value obtained from Score
                    //Manager
                    plateData.AddIngriedint("leaf");
                }
                
                else if (collidingObject.name == "cyrstal_food(Clone)")
                {
                    //Adds to list with corresponding food value obtained from Score
                    //Manager
                    plateData.AddIngriedint("mineral");
                }
                
                else if (collidingObject.name == "egg_food(Clone)")
                {
                    //Adds to list with corresponding food value obtained from Score
                    //Manager
                    plateData.AddIngriedint("gooball");
                }
                
                else if (collidingObject.name == "sheet_food(Clone)")
                {
                    //Adds to list with corresponding food value obtained from Score
                    //Manager
                    plateData.AddIngriedint("sheet");
                }

                //At the end we set collidingObject to null to end the plate collision
                //Also deactivates collisions
                collidingObject.GetComponent<Collider2D>().enabled = false;
                collidingObject = null;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //On a collision enter set collidingObject to collision's(Thing colliding with
        //plate) gameObject
        collidingObject = collision.gameObject;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //If the collision leaves plate boxCollider set it to null
        collidingObject = null;
    }

    
    //This will be for moving the plate to the other screen when order is complete
    private void OnMouseDown()
    {
        
    }
}

