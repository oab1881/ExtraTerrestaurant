///         == created by jack carter 10/07 ==
/// attached to storage units, inherits Hover
/// stores original, changed colors; changed hardcoded
/// overrides HiSp(): changes opacity
/// overrides MouseEnter/Exit: do nothing
///  future:
///    method for storing food items
///    list of stored food items

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


    new void Start()
    {
        base.Start();
        ogColor = sprRend.color;
        hoverColor = ogColor;
        hoverColor.a = .1f;
        fullColor = ogColor;
        fullColor.a = 1f;

        storedItems = new GameObject[maxCapacity];
    }

    // change opacity
    public override void HighlightSprite(bool highlight)
    {
        // no highlight;;; max cap or not
        if (!highlight)
            sprRend.color = ogColor;
        else
            if (currentCapacity == maxCapacity)
                sprRend.color = fullColor;
            else
                sprRend.color = hoverColor;
    }

    // called by DragAndDrop() when item dropped into storage
    public void StoreItem(GameObject item)
    {
        if (currentCapacity >= maxCapacity)
        {
            Debug.Log("store attempt FAILED");
            return;
        }
        else
        {
            Debug.Log("STORAGE store attempt");
            storedItems[currentCapacity] = item;
            currentCapacity++;
            item.GetComponent<DragAndDrop>().Stored(gameObject);
        }
    }

    // do nothing on mouse hover
    protected override void OnMouseEnter() { }
    protected override void OnMouseExit() { }
}
