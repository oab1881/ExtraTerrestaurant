using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class LayerTreatment : MonoBehaviour
{
    //Stores the storage script
    [SerializeField]
    Storage storageScript;

    int currentCount;

    LayerState changeType;
    Color changeColor;

    List<float> timers = new List<float>();

    private void Start()
    {
        if(gameObject.name == "oven red")
        {
            changeType = LayerState.Cooked;
            changeColor = Color.red;
        }
        else if(gameObject.name == "freezer blue")
        {
            changeType = LayerState.Frozen;
            changeColor = Color.blue;
        }else if(gameObject.name == "goop green")
        {
            changeType = LayerState.Gooped;
            changeColor = Color.green;
        }
        currentCount = storageScript.currentCapacity;
    }

    private void Update()
    {
        /*
        if(currentCount < storageScript.currentCapacity)
        {
            currentCount++;
            CreateTimer();
        }
        DecreaseTimer();
        CheckTimer();
        */
    }

    private void CreateTimer()
    {
        for(int i = 0; i < storageScript.currentCapacity; i++)
        {
            if (storageScript.StoredItem[i].GetComponent<FoodData>().CurrentState == LayerState.Base)
            {
                timers.Add(5f);
            }
        }
    }


    private void CheckTimer()
    {
        for(int i = 0; i < timers.Count; i++)
        {
            if (timers[i] < 0.0f)
            {
                storageScript.StoredItem[i].GetComponent<FoodData>().ChangeState(changeType,changeColor);
                timers.RemoveAt(i);
            }
        }
    }

    private void DecreaseTimer()
    {
        for(int i = 0; i < timers.Count; i++)
        {
            timers[i] -= Time.deltaTime;
        }
    }
}
