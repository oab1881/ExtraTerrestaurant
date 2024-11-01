/// === overhaul by jack 11/01 ===
///     script for ingredients
///         ie individual food items at various stages of prep
///     should this inherit from d&d?
///         -any kind of ingredient which can never be dragged??
///         -inherits dragging bool, rigidbod, coll, FollowMouse()
///             -necessary? unsure
///         -alternative: *assumes* d&d script using GetComp<>()
using UnityEngine;

public class Ingredient : MonoBehaviour
{
    private DragAndDrop dnd;
    //private GameObject currentColl;
    [SerializeField]
    private Storage currentColl;
    [SerializeField]
    private bool stored = false;

    void Start()
    {
        // ingredients drag on spawn
        dnd = GetComponent<DragAndDrop>();
        dnd.dragging = true;

    }

    void Update()
    {
        // on release, when colliding with storage, and not stored
        if (!dnd.dragging && currentColl && !stored)
        {
            // if room, store
            //if (currentColl.tag.Equals("storage") && currentColl.GetComponent<Storage>())
            if (currentColl.currentCap < currentColl.maxCapacity)
            {
                Store();
            }
        }
    }

    // place in storage
    private void Store()
    {
        stored = true;
        currentColl.currentCap++;

        // turn off physics
        dnd.TogglePhysics(false);
        dnd.rigidbod.simulated = false;

        // set storage parent
        transform.SetParent(currentColl.gameObject.transform, false);
        //for detach? transform.SetParent(null, false);



        //gameObject.GetComponent<SpriteRenderer>().sortinglayer;



        // set new position
        //transform.localPosition = currentColl.positions[currentColl.currentCap];
        // test
        transform.localScale *= 0.75f;

        //transform.localScale = new Vector3(1,1,1);
        //transform.localPosition = currentColl.position;
        //transform.localPosition = new Vector3(0,.5f,0);
        if (!currentColl.isTray)
            transform.position = currentColl.positions[currentColl.currentCap - 1];
    }

    // only continues when being dragged
    private void OnTriggerEnter2D(Collider2D other)
    {
        // if dragging, save reference to storage script
        //   ref can be null, should work anyway
        if (dnd.dragging)
        {
            //currentColl = other.gameObject;
            currentColl = other.gameObject.GetComponent<Storage>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // if dragging, clear collision reference
        if (dnd.dragging)
        {
            //if (other.gameObject.Equals(currentColl))
            if (currentColl && other.gameObject.Equals(currentColl.gameObject))
            {
                currentColl = null;
            }
        }
    }
}
