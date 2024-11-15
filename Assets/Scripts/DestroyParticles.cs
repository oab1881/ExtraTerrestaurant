using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class DestroyParticles : MonoBehaviour
{
    float time;

    private void Start()
    {
        //Time to subtract from
        time = 5f;
    }

    //Every frame detracts time then destroys particle system
    private void Update()
    {
        time-= Time.deltaTime;
        if(time <= 0f)
        {
            Destroy(this.gameObject);
        }
    }
}

