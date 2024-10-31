/*
 ==== Created by Jake Wardell 09/30/24 ====

Makes it so items can be dragged and dropped around Screen
They need a boxCollider2d and this script for functionality to work

Changelog:
    -Created script : 09/30/24 : Jake
    -Made mouse follow a function for supplementing when food is created : 10/01/24 : Jake
    -newly spawned items are now dragged by default without needing an extra click : 10/02 : jack
    -when items are not being dragged, gravity is applied using a 2D rigidbody : 10/06 : jack
    -items that fall off screen are deleted : 10/06 : jack
    -now inherits hover; added one-at-a-time collisions, CollToAction methods : 10/07 : jack
    -removed old commented code: 10/07 : jack
    -Added a hard code for mortar pos and scale : 10/17/24 : Jake
*/

using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;
//          should DragAndDrop inherit Hover???????????????????????
public class DragAndDrop : Hover
{
    //Useing this to get the original point on start of click and drag
    [SerializeField]
    Vector3 initalMouse = Vector3.zero;
    Rigidbody2D rigidbod;
    Collider2D coll;
    //public GameObject originBucket; // for trashing items in original buckets (doesn't work)

    [SerializeField]
    bool canDragandDrop = true;

    [SerializeField]
    bool isPlate = false;
    // item being dragged?    -OnMouseDrag alternative, doesn't require object be clicked first
    public bool dragging = true;

    /// stores first applicable object collided with
    ///  applicable: storage, ?
    /// ensures item only interacts with one colliding object at a time
    ///  other than physically
    [SerializeField]
    private GameObject firstCollidingObject = null;
    private GameObject nextCollidingObject = null;

    new void Start()
    {
        base.Start();
        rigidbod = gameObject.GetComponent<Rigidbody2D>();
        coll = gameObject.GetComponent<Collider2D>();
        gameObject.tag = "food item";
    }

    public bool CanDragAndDrop
    {
        set { canDragandDrop = value; }
    }

    // do nothing on mouse hover
    //protected override void OnMouseEnter() {}
    protected override void OnMouseExit()
    {
        HighlightSprite(dragging);
    }

    private void OnMouseDown()
    {
        dragging = true;
    }
    private void Update()
    {
        if(isPlate && Input.GetMouseButtonDown(0))
        {
            canDragandDrop = true;
        }
        if (dragging)
        {
            FollowMouse();
            rigidbod.velocity = Vector2.zero;
            rigidbod.angularVelocity = 0f;
            rigidbod.gravityScale = 0;
            gameObject.layer = 7; // held layer
        }
        // OnMouseUp alternative for dragging solution
        if (Input.GetMouseButtonUp(0))
        {
            // drop
            dragging = false;
            initalMouse = Vector3.zero;
            rigidbod.simulated = true;
            rigidbod.gravityScale = 1;
            gameObject.layer = 0; // default layer
            // end drop
            // placed in storage?
            if (firstCollidingObject)
            {
                if (firstCollidingObject.tag.Equals("storage"))
                {
                    Debug.Log("ITEM store attempt");
                    firstCollidingObject.GetComponent<Storage>().StoreItem(gameObject);
                }

                EndCollToAction();
                firstCollidingObject = null;
                nextCollidingObject = null;
            }
        }
        if (transform.position.y < -8)
        {
            Destroy(gameObject);
        }
    }
    private void FollowMouse()
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

    // handles collision action
    private void CollToAction()
    {
        // highlights [storage] on collide
        if (firstCollidingObject.tag.Equals("storage"))
        //            || firstCollidingObject.Equals(originBucket)) // for trashing items in original buckets (doesn't work)
        {
            firstCollidingObject.GetComponent<Storage>().HighlightSprite(sprRend);
        }
    }
    // handles end of first collision action
    //  may be refactored into above method??
    private void EndCollToAction()
    {
        // catches missing objects      can be made redundant, see Update --> MouseUp
        // unhighlights [storage] after collide
        if (firstCollidingObject && firstCollidingObject.tag.Equals("storage"))
        {
            firstCollidingObject.GetComponent<Storage>().HighlightSprite(false);
        }
        /* // for trashing items in original buckets (doesn't work)
        else if (firstCollidingObject.Equals(originBucket))
        {
            firstCollidingObject.GetComponent<Hover>().HighlightSprite(false);
            Destroy(gameObject);
        }*/
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (dragging && other.gameObject.tag != "food bucket")
        {
            // no current collision: store collision object, call action method
            if (!firstCollidingObject)
            {
                firstCollidingObject = other.gameObject;
                CollToAction();
            }
            // queue next collision
            else if (!nextCollidingObject)
            {
                nextCollidingObject = other.gameObject;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        //  exit current collision
        if (other.gameObject.Equals(firstCollidingObject))
        {
            EndCollToAction();
            firstCollidingObject = null;
            // next collision moves up
            if (nextCollidingObject != null)
            {
                firstCollidingObject = nextCollidingObject;
                nextCollidingObject = null;
                CollToAction();
            }
        }
        // exit next collision
        else if (other.gameObject.Equals(nextCollidingObject))
        {
            nextCollidingObject = null;
        }
    }
    // enact property changes when stored
    //  pos, rot: freeze
    //  scale: decrease
    //  canDragAndDrop: off
    public void Stored(GameObject storedIn, Vector2 storedPos)
    {
        Debug.Log("stored");
        // oog jake code - migrated from PlateCollisions script, now altered to fit
        //==== We can change the data about the food gameobject to just put it on
        //the plate ====

        transform.SetParent(storedIn.transform, false);
        canDragandDrop = false;

        //Switching this from rigbod.constraints to gameObject.GetComponent<Rigidbody2D>() fixed issue with
        //mortar and pestle -Jake
        gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        transform.localScale /= 2;


        if (storedIn.GetComponent<BoxCollider2D>())
            //transform.localPosition = storedIn.GetComponent<BoxCollider2D>().offset;
            transform.localPosition = storedPos;
        else transform.localPosition = new Vector2(-2.0f, 2.5f);
        // above is a truly awful bandaid solution to include goop collider

        //Reall bad hard code for temp fix -Jake
        if (storedIn.name == "mortar")
        {
            transform.localScale *= 2;
            transform.localPosition = new Vector3(0, 0, 0);
        }


        /* og jack code
        rigidbod.constraints = RigidbodyConstraints2D.FreezeAll;
        gameObject.transform.localScale = Vector3.one / 4;
        canDragandDrop = false;*/
    }
}