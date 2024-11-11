/// === overhauled by jack 11/01 ===
/// does what it says on the tin  :p
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    // fields
    // public bc accessed by Ingredient, Tray
    public bool dragging = false;
    private Vector3 initialMouse = Vector3.zero;
    public Rigidbody2D rigidbod;
    private Collider2D coll;

    // start
    void Start()
    {
        rigidbod = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
    }

    // update
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
            // on release
            if (Input.GetMouseButtonUp(0))
            {
                // stop dragging, resume physics
                //Debug.Log("drag end");
                initialMouse = Vector3.zero;
                dragging = false;
                TogglePhysics(true);
            }
        }
    }

    // follow mouse
    private void FollowMouse()
    {
        // pause physics
        TogglePhysics(false);

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
        gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "active";
    }

    // on release
    /*private void OnMouseUpAsButton()
    {
        // stop dragging, resume physics
        initialMouse = Vector3.zero;
        dragging = false;
        TogglePhysics(true);
    }*/

    public void TogglePhysics(bool togOn)
    {
        //coll.isTrigger = !togOn;
        //rigidbod.simulated = togOn;
        //rigidbod.velocity = Vector2.zero;
        rigidbod.angularVelocity = 0f;
        if (togOn)
        {
            rigidbod.constraints = RigidbodyConstraints2D.None;
            if (gameObject.GetComponent<Ingredient>())
                gameObject.layer = 3;   // change layer to 3 (ingredient)
            else
                gameObject.layer = 0;   // change layer to 0 (default)
        }
        else
        {
            rigidbod.constraints = RigidbodyConstraints2D.FreezeAll;
            gameObject.layer = 7;   // change layer to 7 (held)
        }
    }
}