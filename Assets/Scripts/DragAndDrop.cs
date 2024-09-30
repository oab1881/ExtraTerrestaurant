/*
 ==== Created by Jake Wardell 09/30/24 ====

Makes it so items can be dragged and dropped around Screen
They need a boxCollider2d and this script for functionality to work

Changelog:
    -Created script : 09/30/24
*/

using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class DragAndDrop : MonoBehaviour
{
    //Useing this to get the original point on start of click and drag
    Vector3 initalMouse = Vector3.zero;
    private void OnMouseDrag()
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
        initalMouse= newPos;
    }

    private void OnMouseUp()
    {
        //After mouse is up resets this to 0
        initalMouse = Vector3.zero;
    }
}

