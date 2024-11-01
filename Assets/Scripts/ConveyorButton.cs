using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Handles how the button acts when it is clicked.
 * 
 * FOR NOW: This function is the scoring function. In future sprints, the scoring will be
 * handled by a class that is not this.
 */

public class ConveyorButton : MonoBehaviour
{
    public Sprite pressedButton;
    public Sprite unpressedButton;
    public SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
    }

    private void OnMouseDown()
    {
        ChangeSprite(pressedButton);
    }
    private void OnMouseUp()
    {
        ChangeSprite(unpressedButton);
    }

    private void ChangeSprite(Sprite newSprite)
    {
        spriteRenderer.sprite = newSprite;
    }
}
