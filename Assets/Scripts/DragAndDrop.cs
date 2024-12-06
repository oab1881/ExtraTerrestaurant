<<<<<<< Updated upstream
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
    -Changed line in storeItem to getComponent instead of rigbod : 10/30/24 : Jake
    -Made items removable in onTriggerExit function : 10/31/24 : Jake
    -Attempted to optomize removale of item detection : Jake : 11/14/24
    -Added in sounds when click and release : Jake : 12/04/24

*/

using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
=======
/// === overhauled by jack 11/01 ===
/// does what it says on the tin  :p
>>>>>>> Stashed changes
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    // fields
    // public bc accessed by Ingredient, Tray
    public bool dragging = false;
    private Vector3 initialMouse = Vector3.zero;
    public Rigidbody2D rigidbod;
    public Collider2D coll;

<<<<<<< Updated upstream
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

    //Used to store isDragging state specifically for sound effect playing
    bool tempIsDragging;

    AudioPlayer quickAudioPlayer;

    new void Start()
    {
        //At starts sets isDragging to be opposite of dragging
        //This makes it so when the food item is created from bin the first click noise
        //Plays 
        tempIsDragging = !dragging;
        base.Start();
        rigidbod = gameObject.GetComponent<Rigidbody2D>();
        coll = gameObject.GetComponent<Collider2D>();
        gameObject.tag = "food item";

        quickAudioPlayer = GameObject.Find("AudioManager(Quick)").GetComponent<AudioPlayer>();
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
        quickAudioPlayer.PlaySoundEffect("Click", 0);
    }
=======
    // start
    void Start()
    {
        rigidbod = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
    }

    // update
>>>>>>> Stashed changes
    private void Update()
    {
        //FollowMouse();
        //if (Input.GetMouseButtonDown(0))
        //{
        //    FollowMouse();
        //}
        if (dragging)
        {
            
            FollowMouse();
<<<<<<< Updated upstream
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

            //Plays sound on drop
            quickAudioPlayer.PlaySoundEffect("release_item", 0);

            // end drop
            // placed in storage?
            if (firstCollidingObject)
=======
            // on release
            if (Input.GetMouseButtonUp(0))
>>>>>>> Stashed changes
            {
                // stop dragging
                initialMouse = Vector3.zero;
                dragging = false;
                gameObject.layer = 8; // tool layer
                //TogglePhysics(true);
                // ingredient?
                if (gameObject.GetComponent<Ingredient>())
                {
                    // ingredient layer
                    gameObject.layer = 9; //ingredient layer
                    // dont resume physics if currently stored
                    if (gameObject.GetComponent<Ingredient>().stored)
                        return;
                }
                // resume physics
                rigidbod.angularVelocity = 0f;  // reset velocity
                rigidbod.constraints = RigidbodyConstraints2D.None; // unfreeze transform

            }
        }
<<<<<<< Updated upstream
        if (transform.position.y < -8)
        {
            Destroy(gameObject);
        }

        //Checks if is dragging and tempIsdragging are diff; this indicates a state change
        //in clicking.
        //Then check if is dragging is true we only want click to play when the play is clicking down
        //If check isn't in then when they let go sound plays
        if (tempIsDragging != dragging && dragging == true)
        {
            GameObject.Find("AudioManager(Quick)").GetComponent<AudioPlayer>().PlaySoundEffect("Click", 0);
        }

        //Sets tempIsDragging to match dragging at the end.
        tempIsDragging= dragging;
=======
>>>>>>> Stashed changes
    }

    // follow mouse
    private void FollowMouse()
    {
        // pause physics
        //TogglePhysics(false);
        rigidbod.constraints = RigidbodyConstraints2D.FreezeAll; // freeze transform

        // save mouse pos
        Vector3 beegMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // if start of drag, set initMouse to current mouse
        if (initialMouse == Vector3.zero)
            initialMouse = beegMouse;
        // z-zeroing
        initialMouse.z = 0;
        beegMouse.z = 0;

        // move in relation to self
        transform.position += beegMouse - initialMouse;

        // set initMouse for next drag frame
        initialMouse = beegMouse;
    }

    /*private void FollowMouse(Vector3 initialMouse)
    {
        Vector3 newPos = initialMouse;
        // set position to mouse pos
        if (initialMouse == Vector3.zero)
        {
            newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        newPos.z = 0;
        transform.position = newPos;
        // mouse still clicking, call again
        if (Input.GetMouseButtonDown(0))
        {
            FollowMouse(newPos);
        }
    }*/

    // check new mouse click
    private void OnMouseDown()
    {
        //FollowMouse(Vector3.zero);
        //FollowMouse();
        dragging = true;
    }

    // on release
    /*private void OnMouseUpAsButton()
    {
        // stop dragging, resume physics
        initialMouse = Vector3.zero;
        dragging = false;
        TogglePhysics(true);
    }*/

    // DO NOT USE
    /*public void TogglePhysics(bool togOn)
    {
        //coll.isTrigger = !togOn;
        //rigidbod.simulated = togOn;
        //rigidbod.velocity = Vector2.zero;
        rigidbod.angularVelocity = 0f;  // reset velocity
        if (togOn)  // freeze transform, set sorting/layers
        {
            rigidbod.constraints = RigidbodyConstraints2D.None;
            if (gameObject.GetComponent<Ingredient>())  // ingredient or tray
                gameObject.layer = 3;   // change layer to 3 (ingredient)
            else
            {
                gameObject.layer = 8;   // change layer to 8 (storage)
                gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "storage";
                gameObject.GetComponent<SpriteRenderer>().sortingOrder = 5;
            }
        }
        else
        {
            rigidbod.constraints = RigidbodyConstraints2D.FreezeAll;
            gameObject.layer = 7;   // change layer to 7 (held)
            gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "active";
            gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
        }
<<<<<<< Updated upstream
        // exit next collision
        else if (other.gameObject.Equals(nextCollidingObject))
        {
            nextCollidingObject = null;
        }


        //System for removing items from storage system
        if (stored && (other.gameObject.tag == "storage" || other.gameObject.name == "single plate"))
        {
            //Gets a reference to other which in this case is some sort of storage gameobject then
            //Gets the storage script
            Storage goStorage = other.gameObject.GetComponent<Storage>();

            //Sets it to -1 by default stops errors if we can not find the index to remove
            int indexRemove = -1;

            //Sets the object to dragging
            dragging = true;

            //Trys finding the index to remove
            for(int i = 0; i < goStorage.currentCapacity; i++)
            {
                //Checks with the list of stored items in that storage type
                //ie goop stored items, freezer stored items, mortar stored items, ect
                //Checks if that the current one in the list equals the gameobject
                //If so index to remove equals i and we break
                if (goStorage.StoredItem[i].Equals(gameObject))
                {
                    indexRemove = i;
                    break;
                }
            }

            //If it doesn't equal -1 then we can remove indexRemove
            if (indexRemove != -1)
            {
                //Calls the function from the storage item
                goStorage.RemoveItem(indexRemove, false, true);
                if (other.gameObject.name == "oven red")
                    quickAudioPlayer.PlaySoundEffect("oven_close", 0);
                if (other.gameObject.name == "freezer blue")
                    quickAudioPlayer.PlaySoundEffect("Freezer close", 0);
                if (other.gameObject.name == "goop green")
                    quickAudioPlayer.PlaySoundEffect("remove_from_slime", 0);

                //Sets the rigidbody constraints to none
                gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            }
        }
        //stored = false; Not sure what this line does commenting it out 11/14/2024
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
        
        //canDragandDrop = false; Commenting out for removal

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
            //transform.localScale *= 2;
            transform.localPosition = new Vector3(0, 0, 0);
        }

        stored = true;
        /* og jack code
        rigidbod.constraints = RigidbodyConstraints2D.FreezeAll;
        gameObject.transform.localScale = Vector3.one / 4;
        canDragandDrop = false;*/
    }

    private void OnMouseUp()
    {
        
    }
}
=======
    }*/
}
>>>>>>> Stashed changes
