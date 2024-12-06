/// The New Hover
/// only for mouse hovering
/// two sprites: default and hovered
///     default is default
///     hovered is set in the inspector
///     hovered is typically a simple white outline
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : MonoBehaviour
{
    public Sprite defaultSprite;
    [SerializeField]
    public Sprite hoveredSprite;
    private SpriteRenderer sprRend;

    void Start()
    {
        sprRend = gameObject.GetComponent<SpriteRenderer>();
        defaultSprite = sprRend.sprite;
    }

    private void OnMouseEnter()
    {
        sprRend.sprite = hoveredSprite;
    }
    public void OnMouseExit()
    {
        sprRend.sprite = defaultSprite;
    }
}