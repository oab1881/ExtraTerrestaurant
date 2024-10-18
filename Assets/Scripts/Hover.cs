///         == created by jack carter 10/07 ==
/// abstract, inherited by: DragAndDrop, Bucket, Storage
/// contains default and highlighted sprite versions
/// HighlightSprite: swaps highlighted sprite
/// virtual MouseEnter/Exit call HiSp()
/// assumes collider

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Hover : MonoBehaviour
{
    //[SerializeField]
    protected Sprite defaultSprite;
    [SerializeField]
    protected Sprite hoveredSprite;
    protected SpriteRenderer sprRend;

    // Start is called before the first frame update
    protected void Start()
    {
        sprRend = gameObject.GetComponent<SpriteRenderer>();
        //defaultSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
        if (!defaultSprite)
            defaultSprite = sprRend.sprite;
        if (!hoveredSprite)
            hoveredSprite = defaultSprite;
    }

    // swaps sprite: default <-> highlighted
    //   bool param for highlight or not
    public virtual void HighlightSprite(bool highlight)
    {
        // sprite to highlighted sprite
        if (highlight)
            gameObject.GetComponent<SpriteRenderer>().sprite = hoveredSprite;
        else
            gameObject.GetComponent<SpriteRenderer>().sprite = defaultSprite;
    }

    protected virtual void OnMouseEnter()
    {
        HighlightSprite(true);
    }
    protected virtual void OnMouseExit()
    {
        // sprite to unhovered sprite
        HighlightSprite(false);
    }
}
