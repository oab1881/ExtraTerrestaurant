///         == created by jack carter 10/07 ==
/// attached to storage units, inherits Hover
/// stores original, changed colors; changed hardcoded
/// overrides HiSp(): changes opacity
/// overrides MouseEnter/Exit: do nothing
///  future:
///    method for storing food items
///    list of stored food items
///    
///    Changes:
///    10/27/24 : Jake : Added remove items function
///    10/30/24 : Jake : Made the remove items have destory
///    10/30/24 : Jake : Made the remove items have a resize
///    

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : Hover
{
    Color ogColor;
    Color hoverColor;
    Color fullColor;

    [SerializeField]
    private GameObject[] storedItems;
    public int maxCapacity = 5;
    public int currentCapacity = 0;

    [SerializeField]
    private GameObject snapZone;
    private SpriteRenderer snapSprite;
    [SerializeField]
    private Vector2[] positions;


    //A property for the stored item list
    public GameObject[] StoredItem
    {
        get { return storedItems; }
        set { storedItems = value; }
    }

    new void Start()
    {
        base.Start();
        ogColor = sprRend.color;
        hoverColor = ogColor;
        hoverColor.a = .1f;
        fullColor = ogColor;
        fullColor.a = 1f;

        storedItems = new GameObject[maxCapacity];

        snapSprite = snapZone.GetComponent<SpriteRenderer>();
    }

    /// storage positions
    ///     vector2 array, loaded at runtime
    ///     single snap zone child
    ///         place at first available position (current capacity)
    ///         visible when food item hovering storage
    ///         takes food item sprite
    ///         change color on item hover

    // change opacity
    public override void HighlightSprite(bool highlight)
    {
        // no highlight
        if (!highlight)
        {
            sprRend.color = ogColor;
            snapSprite.enabled = false;
        }
        else
        {
            if (currentCapacity == maxCapacity)
                sprRend.color = fullColor;
            else
                sprRend.color = hoverColor;
        }
    }

    // highlight overload for item hover --> sprite renderer
    public void HighlightSprite(SpriteRenderer itemSprite)
    {
        if (currentCapacity == maxCapacity)
            sprRend.color = fullColor;
        else
        {
            snapZone.transform.localPosition = positions[currentCapacity];
            // get held item sprite
            snapSprite.sprite = itemSprite.sprite;
            snapSprite.enabled = true;
            HighlightSprite(true);
        }
    }

    // called by DragAndDrop() when item dropped into storage
    public void StoreItem(GameObject item)
    {
        Debug.Log("Current Capacity On Store: " + currentCapacity);
        if (currentCapacity >= maxCapacity)
        {
            Debug.Log("store attempt FAILED");
            return;
        }
        else
        {
            Debug.Log("STORAGE store attempt");
            storedItems[currentCapacity] = item;
            item.GetComponent<DragAndDrop>().Stored(gameObject, positions[currentCapacity]);
            currentCapacity++;
            snapSprite.enabled = false;
        }
    }

    /// <summary>
    /// Removes item from list and destorys it(Useful for switching item from storage
    /// </summary>
    /// <param name="index">Index to remove from</param>
    public void RemoveItem(int index, bool delete, bool resize)
    { 
        //Creates a new temp list
        GameObject[] newList = new GameObject[maxCapacity];

        //For if it's the last item
        if(index == currentCapacity)
        {
            //Sets the 2 arrays to be the same
            newList = storedItems;
            newList[index] = null;
        }
        //For if it's the first item
        else if(index == 0)
        {
            //Goes through each one and sets the current newList value to the stored list next value
            //IE everything gets shifted down one.
            for(int i = 0; i < currentCapacity - 1; i++)
            {
                newList[i] = storedItems[i + 1];
            }
        }
        //For case of removing in the middle
        else
        {
            //Loop until index

            //Goes from 0 to index gets
            //Gets eveything before removal becasue they don't change
            for (int i = 0; i < index; i++)
            {
                    newList[i] = storedItems[i];
            }
            //Skips over index and moves everything back till the end
            for (int i = index; i < currentCapacity - 1; i++)
            {
                newList[i] = storedItems[i + 1];
            }
        }
        
        /*
        for(int i =0; i < currentCapacity; i++)
        {
            newList[i].transform.position = positions[i];
        }
        */
        //Destroys the old item if parameter says to
        //Resizes if the parameter says to
        //Sets the stored list to the new list
        //Decreases capacity
        if (delete)
        {
            Destroy(storedItems[index]);
        }
        if (resize)
        {
            storedItems[index].transform.localScale *= 2;
        }
        storedItems = newList;
        currentCapacity--;

    }
    // do nothing on mouse hover
    protected override void OnMouseEnter() { }
    protected override void OnMouseExit() { }
}
