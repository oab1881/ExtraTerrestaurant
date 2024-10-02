using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 == changelog ==
    -spawns summoned food item at mouse pos : 10/02 : jack
        -alternative commented out: spawn at box coll center; not preferred
*/

public class SummonFood : MonoBehaviour
{
    [SerializeField]
    GameObject foodItem;
    public Vector2 spawnPos;

    /*
    // spawns food item at box collider center
    public BoxCollider2D boxColl;
    private void Start()
    {   
        spawnPos = boxColl.offset;
    }*/

    private void OnMouseDown()
    {
        // spawns food item at mouse position2
        Vector3 spawnPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        spawnPos.z = 0;


        Instantiate(foodItem, spawnPos, gameObject.transform.rotation);
    }
}
