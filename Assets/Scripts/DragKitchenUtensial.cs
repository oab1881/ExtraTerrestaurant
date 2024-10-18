/*
 ==== Created by Jake Wardell 10/17/24 ====

Drag and drop variation for knife and pestel

Changelog:
    -Created script
    
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragKitchenUtensial : MonoBehaviour
{
    //Useing this to get the original point on start of click and drag
    Vector3 initalMouse = Vector3.zero;
    
    [SerializeField]
    Rigidbody2D rigidbod;
   
    bool canDragandDrop = true;
    public bool dragging = true;
    Vector3 initalTransform;
    Quaternion intialRotation;

    private void Start()
    {
        initalTransform = transform.position;
        intialRotation = transform.rotation;
    }
    private void Update()
    {
        if (transform.position.y < -8)
        {
            transform.rotation = intialRotation;
            transform.position = initalTransform;
            rigidbod.velocity = Vector2.zero;
            rigidbod.angularVelocity = 0f;
            rigidbod.gravityScale = 0;
            gameObject.layer = 7; // held layer
        }
    }


    private void OnMouseDrag()
    {
        followMouse();
        rigidbod.velocity = Vector2.zero;
        rigidbod.angularVelocity = 0f;
        rigidbod.gravityScale = 0;
        gameObject.layer = 7; // held layer
    }

    private void OnMouseUp()
    {
        initalMouse = Vector3.zero;
        rigidbod.simulated = true;
        rigidbod.gravityScale = 1;
        gameObject.layer = 0; // default layer
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
}
