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

    new void Start()
    {
        base.Start();
        ogColor = sprRend.color;
        hoverColor = ogColor;
        hoverColor.a = .1f;
    }

    // change opacity
    public override void HighlightSprite(bool highlight)
    {
        if (highlight)
            sprRend.color = hoverColor;
        else
            sprRend.color = ogColor;
    }

    // do nothing on mouse hover
    protected override void OnMouseEnter() { }
    protected override void OnMouseExit() { }
}
