/*
 ==== Created by Jake Wardell 10/01/24 ====

Does collision between the food objects and the plates in the game! Also
between mouse and plate!

Changelog:
    -Created script : 10/01/24
    -changed item placement to new plate center : 10/02 : jack
    -Made new changes for new scoring system and cap to amount of food 10/17/24 : Jake
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

    Vector2 difference = Vector2.zero;

    void Update()
    {
        /* -Handles changing data about the food item 
           -Also sends information on what food item is to the plate data
         */

        //Tests to see if object is null
        //This check is essential to find out if anything is actually colliding
        //Also checks to see if max plate size is capped
        if (collidingObject && collidingObject.tag.Equals("food item") && 
            plateData.IngriedentsListSize < 3)
        {
            //Something can be colliding but maybe it shouldn't go on the plate...
            //So we wait for mouse to let off of
            if (Input.GetMouseButtonUp(0))
            {

                //moving this into a the Storage() method in d&d for food items,
                //applying it to other storage -Jack

                /*==== We can change the data about the food gameobject to just put it on
                //the plate ====
                collidingObject.transform.SetParent(transform, false);
                collidingObject.GetComponent<DragAndDrop>().CanDragAndDrop = false;
                collidingObject.transform.localScale = collidingObject.transform.localScale / 2;
                //collidingObject.transform.localPosition = new Vector3(2, 2, 0);
                collidingObject.transform.localPosition = Vector3.zero;
                */
                collidingObject.GetComponent<DragAndDrop>().Stored(gameObject, Vector2.zero);

                // ==== Then for each food item we can test it's name ====
                //And send it's data to the list of food items on the plate


                //For testing the tentacle food item
                if (collidingObject.name == "tentacle_food(Clone)")
                {
                    //Adds to list with corresponding food value obtained from Score
                    //Manager
                    plateData.AddIngredient("tentacle", "");
                }


                else if (collidingObject.name == "leaf_food(Clone)")
                {
                    //Adds to list with corresponding food value obtained from Score
                    //Manager
                    plateData.AddIngredient("leaf", "");
                }

                else if (collidingObject.name == "cyrstal_food(Clone)")
                {
                    //Adds to list with corresponding food value obtained from Score
                    //Manager
                    plateData.AddIngredient("crystal", "");
                }

                else if (collidingObject.name == "egg_food(Clone)")
                {
                    //Adds to list with corresponding food value obtained from Score
                    //Manager
                    plateData.AddIngredient("egg", "");
                }

                else if (collidingObject.name == "sheet_food(Clone)")
                {
                    //Adds to list with corresponding food value obtained from Score
                    //Manager
                    plateData.AddIngredient("sheet", "");
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
        difference = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position;
    }
    private void OnMouseDrag()
    {
        transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - difference;
    }
}

