/// === created for overhaul by jack 11/01 ===
/// script for ingredients
///   i.e. individual food items at various stages of prep
///   assumes d&d script using GetComp<>()

/// COLLISION REFERENCE:
///   all storages and ingredients have colliders and rigidbodies
///   none are ever marked as triggers
///   there are 3 layers
///     held --------> anything currently being dragged, e.g. tray, egg
///     stored ------> ingredients currently in storage
///     ingredient --> loose ingredients, i.e. catch-all for non-held, non-stored
///   stored and held items don't receive or apply forces from anything*
///     where: Inspector > Collider > Layer Overrides > Force Send/Recieve
///     *utensil exception? maybe split held into held food and held tools
using UnityEngine;

public class Ingredient : MonoBehaviour
{
    private DragAndDrop dnd;
    //private GameObject currentColl;
    [SerializeField]
    private Storage currentColl = null;
    private Storage nextColl = null;
    [SerializeField]
    private bool stored = false;

    private SpriteRenderer sprRend;

    void Start()
    {
        // ingredients drag on spawn
        dnd = GetComponent<DragAndDrop>();
        dnd.dragging = true;

        sprRend = gameObject.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // on release, when colliding with storage, and not stored
        if (!dnd.dragging && currentColl)
        {
            if (!stored)
            {
                // if room, store
                //if (currentColl.tag.Equals("storage") && currentColl.GetComponent<Storage>())
                if (currentColl.currentCap < currentColl.maxCapacity)
                {
                    Store();
                    //CollToAction();
                }
            }
            else
            {
                dnd.FollowMouse();
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
        //dnd.rigidbod.simulated = false;

        // set storage parent
        //transform.SetParent(currentColl.gameObject.transform, false);
        //for detach? transform.SetParent(null, false);

        // change layer, stored layer == 9
        gameObject.layer = 9;
        // draw behind storage tint
        sprRend.sortingLayerName = "storage";
        sprRend.sortingOrder = 3;

        // set new position
        //transform.localPosition = currentColl.positions[currentColl.currentCap];
        // test
        transform.localScale *= 0.75f;

        //transform.localScale = new Vector3(1,1,1);
        //transform.localPosition = currentColl.position;
        //transform.localPosition = new Vector3(0,.5f,0);
        
        // tray bien        ...tray-specific stuff
        if (currentColl.isTray)
        {
            // transform with tray
            //transform.SetParent(currentColl.gameObject.transform, false);
            // draw on top of tray
            sprRend.sortingOrder = 6;
        }
        else
        {
            // set position
            transform.position = currentColl.positions[currentColl.currentCap - 1];
        }

        currentColl.HighlightSprite(false);
        // steps queue
        //currentColl = null;   // current collider retained as reference to storage object
        nextColl = null;
    }

    // handles collision action
    //private void CollToAction(Storage collStorage)    // alternate, uses param instead of current collision
    //{ currentColl = collStorage;
    /*private void CollToAction()
    {
        Store();
    }*/

    // INGREDIENT & STORAGE COLLISIONS  --  more deets at top
    // NOW MANAGED USING COLLISION ENTER/EXIT  --  NO* TRIGGER COLLIDERS ALLOWED
    //    *unless they turn out to be useful/necessary for something else, trash can maybe?
    // only continues when being dragged
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // if dragging, save reference to storage script
        //   ref can be null, should work anyway
        if (dnd.dragging && collision.gameObject.tag.Equals("storage"))
        {
            if (currentColl)    // if there is a current collision
            {
                if (!nextColl)  // if queue spot open
                {
                    nextColl = collision.gameObject.GetComponent<Storage>();
                }
            }
            else
            {
                currentColl = collision.gameObject.GetComponent<Storage>();
                currentColl.HighlightSprite(sprRend);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // if dragging, clear collision reference
        if (dnd.dragging)
        {
            // dragging out of storage
            if (stored)
            {
                stored = false;
                currentColl.currentCap--;
                dnd.TogglePhysics(false);
                sprRend.sortingLayerName = "active";
                sprRend.sortingOrder = 2;
                transform.localScale /= 0.75f;
                currentColl = null;
            }
            else
            {
                //if (other.gameObject.GetComponent<Storage>().Equals(currentColl))
                if (currentColl && collision.gameObject.Equals(currentColl.gameObject))
                {
                    currentColl.HighlightSprite(false);
                    currentColl = null;
                    if (nextColl)
                    {
                        currentColl = nextColl;
                        currentColl.HighlightSprite(sprRend);
                        nextColl = null;
                    }
                }
                else if (nextColl && collision.gameObject.GetComponent<Storage>().Equals(nextColl))
                {
                    //nextColl.HighlightSprite(false);
                    nextColl = null;
                }
            }
        }
    }
}
//    // only continues when being dragged
//    private void OnTriggerEnter2D(Collider2D other)
//    {
//        // if dragging, save reference to storage script
//        //   ref can be null, should work anyway
//        if (dnd.dragging)
//        {
//            if (currentColl)    // if there is a current collision
//            {
//                if (!nextColl)  // if queue spot open
//                {
//                    nextColl = other.gameObject.GetComponent<Storage>();
//                }
//            }
//            else
//            {
//                currentColl = other.gameObject.GetComponent<Storage>();
//                currentColl.HighlightSprite(sprRend);
//            }
//        }
//    }

//    private void OnTriggerExit2D(Collider2D other)
//    {
//        // if dragging, clear collision reference
//        if (dnd.dragging)
//        {
//            //if (other.gameObject.GetComponent<Storage>().Equals(currentColl))
//            if (currentColl && other.gameObject.Equals(currentColl.gameObject))
//            {
//                currentColl.HighlightSprite(false);
//                currentColl = null;
//                if (nextColl)
//                {
//                    currentColl = nextColl;
//                    currentColl.HighlightSprite(sprRend);
//                    nextColl = null;
//                }
//            }
//            else if (nextColl && other.gameObject.GetComponent<Storage>().Equals(nextColl))
//            {
//                nextColl.HighlightSprite(false);
//                nextColl = null;
//            }
//        }
//    }
//}
