/*
 ==== Created by Jake Wardell 10/17/24 ====

Makes it so food can be crushed not fully implemented

Changelog:
    -Created script: 10/17/24 : Jake
    
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mortar : MonoBehaviour
{
    int pestelCount;
    [SerializeField]
    Storage storageScript;

    private void Start()
    {
        pestelCount= 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "pestle" && storageScript.StoredItem[0] != null)
        {
            Debug.Log("SMASH!");
            pestelCount++;
            if(pestelCount == 5)
            {
                pestelCount = 0;
                Destroy(storageScript.StoredItem[0]);
            }

        }
    }
}
