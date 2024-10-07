using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : MonoBehaviour
{
    Sprite defaultSprite;
    [SerializeField]
    Sprite hoveredSprite;


    // Start is called before the first frame update
    void Start()
    {
        defaultSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
    }

    private void OnMouseEnter()
    {
        // sprite to hovered sprite
        gameObject.GetComponent<SpriteRenderer>().sprite = hoveredSprite;
    }
    private void OnMouseExit()
    {
        // sprite to unhovered sprite
        gameObject.GetComponent<SpriteRenderer>().sprite = defaultSprite;
    }
}
