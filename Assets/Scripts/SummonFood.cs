using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonFood : MonoBehaviour
{
    [SerializeField]
    GameObject foodItem;

    private void OnMouseDown()
    {
        
        Instantiate(foodItem);
    }
}
