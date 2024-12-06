using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pestle : MonoBehaviour
{
    [SerializeField]
    private Storage mortarStorage;
    [SerializeField]
    private ParticleSystem particles;

    private DragAndDrop dnd;

    private GameObject itemToCrush;
    [SerializeField]
    private int smashCount = 0;

    void Start()
    {
        dnd = gameObject.GetComponent<DragAndDrop>();
    }
    void Update()
    {
        //dnd.rigidbod.freezeRotation = dnd.dragging; //rotation lock
        //dnd.rigidbod.freezeRotation = true; //rotation lock
        
        // maybe instead of every update frame, do this in Mortar script when object is stored
        itemToCrush = mortarStorage.StoredItem[0];
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //GameObject itemToCrush = mortarStorage.StoredItem[0];
        // when pestle collides with item in mortar
        if (itemToCrush && itemToCrush.Equals(collision.gameObject) && 
            //Added an extra check to make sure you can't grinding something that is already grinded
            itemToCrush.GetComponent<FoodData>().PrepName != "grinded")
        {
            smashCount++; //increment smashCount
            // particle effects
            particles.startColor = itemToCrush.GetComponent<FoodData>().FoodColor;
            particles.Play();

            // play sound
            GameObject.Find("AudioManager(Quick)").GetComponent<AudioPlayer>().PlaySoundEffect("mortar", 0);

            // when fully ground
            if (smashCount > 3)
            {
                itemToCrush.GetComponent<Ingredient>().Crush();
                smashCount = 0; //reset smashCount
                //itemToCrush = null; //prevents further smashing (doesn't work)
            }
        }
    }
}
