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
    public bool stored = false;

    private FoodData foodData;

    private Hover hov;
    private SpriteRenderer sprRend;
    [SerializeField]
    private Sprite powderSprite;
    [SerializeField]
    private Sprite powderHoverSprite;

    void Start()
    {
        // ingredients drag on spawn
        dnd = GetComponent<DragAndDrop>();
        dnd.dragging = true;

        foodData = GetComponent<FoodData>();
        sprRend = GetComponent<SpriteRenderer>();
        hov = GetComponent<Hover>();
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
            // stored in tray
            /*else if (currentColl.isTray)        // ugly as sin, refactor
            {
                // tray moving, 
                if (/*currentColl.gameObject.GetComponent<DragAndDrop>().dragging   // stop simulation if tray is moving, prevents dragging
                    || /currentColl.gameObject.GetComponent<DragAndDrop>().rigidbod.velocity != Vector2.zero)
                {
                    //dnd.rigidbod.constraints = RigidbodyConstraints2D.None; // freeze transform ?
                    dnd.rigidbod.simulated = false;   // works, but can't click/drag
                }
                else
                {
                    //dnd.rigidbod.constraints = RigidbodyConstraints2D.FreezeAll; // freeze transform ?
                    dnd.rigidbod.simulated = true;   // works, but can't click/drag
                }
            }*/
        }
    }

    // place in storage
    private void Store()
    {
        stored = true;
        currentColl.StoredItem[currentColl.currentCap] = gameObject;
        currentColl.currentCap++;

        // pause physics
        //dnd.TogglePhysics(false);
        dnd.rigidbod.constraints = RigidbodyConstraints2D.FreezeAll; // freeze transform

        // set storage parent
        //transform.SetParent(currentColl.gameObject.transform, false);
        //for detach? transform.SetParent(null, false);

        // change layer, stored layer == 10
        gameObject.layer = 10;
        // draw behind storage tint
        sprRend.sortingLayerName = "storage";
        sprRend.sortingOrder = 8;


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
            transform.SetParent(currentColl.gameObject.transform, true);
            dnd.rigidbod.constraints = RigidbodyConstraints2D.FreezeAll; // freeze transform ?
            //dnd.rigidbod.simulated = false;   // works, but can't click/drag
            // draw on top of tray
            //sprRend.sortingOrder = 6;
            // add self to tray ingredient list -nonremovable?
            currentColl.gameObject.GetComponent<PlateData>().AddIngredient(gameObject);
        }
        else
        {
            // set position
            transform.position = currentColl.positions[currentColl.currentCap - 1];
            currentColl.HighlightSprite(false);
        }

        // steps queue
        //currentColl = null;   // current collider retained as reference to storage object
        nextColl = null;
    }

    // swaps normal assets for hovered
    public void Crush()
    {
        foodData.PrepareFood("grinded");
        hov.defaultSprite = powderSprite;
        hov.hoveredSprite = powderHoverSprite;
        hov.OnMouseExit();  // makes Hover change current sprite
        // reset collider with new sprite
        Destroy(GetComponent<Collider2D>());
        //if (foodData.FoodName.Equals("egg");
        GetComponent<DragAndDrop>().coll = gameObject.AddComponent<PolygonCollider2D>();
    }

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
            }
        currentColl.HighlightSprite(sprRend);
        }
    }

    private int RemoveFromStorage()
    {
        // finds own index in storage, removes self
        for (int s = 0; s < currentColl.maxCapacity; s++)
        {
            if (currentColl.StoredItem[s] && currentColl.StoredItem[s].Equals(gameObject))
                return s;
        }
        return -1;
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
                currentColl.RemoveItem(RemoveFromStorage());
                currentColl.currentCap--;
                if (currentColl.gameObject.name == "oven red")
                    dnd.quickAudioPlayer.PlaySoundEffect("oven_close", 0);
                if (currentColl.gameObject.name == "freezer blue")
                    dnd.quickAudioPlayer.PlaySoundEffect("Freezer close", 0);
                if (currentColl.gameObject.name == "goop green")
                    dnd.quickAudioPlayer.PlaySoundEffect("remove_from_slime", 0);
                //dnd.TogglePhysics(false);
                dnd.rigidbod.constraints = RigidbodyConstraints2D.FreezeAll; // freeze transform
                gameObject.layer = 9; //ingredient layer
                sprRend.sortingLayerName = "active";
                sprRend.sortingOrder = 2;
                transform.localScale /= 0.75f;
                currentColl = null;
                transform.SetParent(null, true);
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
                //else if (nextColl && collision.gameObject.GetComponent<Storage>().Equals(nextColl))
                else if (nextColl && collision.gameObject.GetComponent<Storage>() && collision.gameObject.GetComponent<Storage>().Equals(nextColl))
                //else if (nextColl)
                {
                    //nextColl.HighlightSprite(false);
                    //if (collision.gameObject.GetComponent<Storage>().Equals(nextColl))
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
