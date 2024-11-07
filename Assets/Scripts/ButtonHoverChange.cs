using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//Written by Owen Beck
//Script handles hovering over UI elements to switch from alien language to English
public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image buttonImage;           //Reference to the button's Image component
    public Sprite defaultSprite;        //Default button sprite
    public Sprite hoverSprite;          //Hover button sprite

    private void Start()
    {
        if (buttonImage == null)
            buttonImage = GetComponent<Image>();  //Auto-assign if not set in Inspector

        //Set the button to the default sprite initially
        if (buttonImage != null)
            buttonImage.sprite = defaultSprite;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Change to the hover sprite when the pointer enters
        if (buttonImage != null)
            buttonImage.sprite = hoverSprite;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Revert to the default sprite when the pointer exits
        if (buttonImage != null)
            buttonImage.sprite = defaultSprite;
    }
}
