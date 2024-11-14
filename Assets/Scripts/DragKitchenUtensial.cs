/*
 ==== Created by Jake Wardell 10/17/24 ====

Drag and drop variation for knife and pestel

Changelog:
    -Created script : Jake : Unknown
    -Made it so if object collides with anything then it applies gravity : Jake : 11/14/24
    
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragKitchenUtensial : MonoBehaviour
{
    //Useing this to get the original point on start of click and drag
    Vector3 initalMouse = Vector3.zero;
    
    //Stores rigid body
    [SerializeField]
    Rigidbody2D rigidbod;
   
    //If it can drag and drop
    //Stores intial transform and rotation this is for when they fall off the map
    //Because of grabity
    bool canDragandDrop = true;
    Vector3 initalTransform;
    Quaternion intialRotation;

    private void Start()
    {
        //Sets the values to the inital
        initalTransform = transform.position;
        intialRotation = transform.rotation;
    }
    private void Update()
    {
        //If the knife or pestel go to far off the map teleports them back
        if (transform.position.y < -8)
        {
            //Resets eveything
            transform.rotation = intialRotation;
            transform.position = initalTransform;
            rigidbod.velocity = Vector2.zero;
            rigidbod.angularVelocity = 0f;
            rigidbod.gravityScale = 0;
            //gameObject.layer = 7; // held layer
        }
    }


    private void OnMouseDrag()
    {
        //On mouse drag calls the follow mouse and then changes some information
        followMouse();

        //Sets the velocity to zero velocity off and gravity off
        rigidbod.velocity = Vector2.zero;
        rigidbod.angularVelocity = 0f;
        rigidbod.gravityScale = 0;
        //gameObject.layer = 7; // held layer
    }

    private void OnMouseUp()
    {
        //Sets the intial mouse to zero and turns grabity and physics back on
        initalMouse = Vector3.zero;
        rigidbod.simulated = true;
        rigidbod.gravityScale = 1;
        //gameObject.layer = 0; // default layer
    }

   

    private void followMouse()
    {
        if (canDragandDrop)
        {
            //Checks if the inital is 0 this indicates start of mouse drag
            //Then the intial point gets set to current mouse pos
            if (initalMouse == Vector3.zero) initalMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            initalMouse.z = 0;

            //New pos is grabbed for the duration of mouse down
            Vector3 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            newPos.z = 0;

            //Takes the current pos of the object and adds newPos - inital this difference
            //Will move the object in relation to itself 
            transform.position += newPos - initalMouse;

            //Sets intial mouse to newPos at the end
            initalMouse = newPos;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        rigidbod.gravityScale = 1;
    }
}
