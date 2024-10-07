using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteChop : MonoBehaviour
{
    [SerializeField]
    Texture2D image;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {


        // get mouse pos
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Rect rect = new Rect(transform.position.x, transform.position.y, mousePos.x, 10);
        Vector2 center = new Vector2(0, 0);
        Debug.Log(mousePos);

        // create 1st cropped sprite
        //Sprite mySprite =
        //image = gameObject.GetComponent<SpriteRenderer>().sprite;
        gameObject.GetComponent<SpriteRenderer>().sprite = Sprite.Create(image, rect, center);
        // create 2nd half of cropped sprite
        //Sprite firstSprite =
        // Instantiate clone

        // set self 1st sprite
        rect.x = mousePos.x;
        rect.width = 10;
        // set clone 2nd sprite
        Instantiate(gameObject).GetComponent<SpriteRenderer>().sprite = Sprite.Create(image, rect, center);
    }
}
