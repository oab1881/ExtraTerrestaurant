using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script by Owen Beck//
public class OrderingAlien : MonoBehaviour
{
    //Testing Script that will spawn in an alien prefab and have it slowly move toward the camera

    public GameObject alienPrefab;  // The alien prefab to spawn
    public float startSize = 0.1f;  // Initial size of the alien when far away
    public float endSize = 1.0f;    // Final size of the alien when it reaches the stopping point
    public float approachSpeed = 1.0f;  // Speed at which the alien approaches
    public Vector3 startPoint = new Vector3(0, 0, 10);  // Starting point far away from the camera
    public Vector3 endPoint = new Vector3(0, 0, 0);     // Point where the alien will stop
    private GameObject alienInstance;  // Instance of the alien prefab
    private float journeyLength;
    private float distanceCovered = 0.0f;

    bool hasApproached = false; //Bool tracking if the alien has finished approaching

    void Start()
    {
        // Spawn the alien at the starting point and set its initial scale
        alienInstance = Instantiate(alienPrefab, startPoint, Quaternion.identity);
        alienInstance.transform.localScale = Vector3.one * startSize;

        // Calculate the total distance between the start and end points
        journeyLength = Vector3.Distance(startPoint, endPoint);
    }

    void Update()
    {
        if (!hasApproached)
            Lerp();
        else if (hasApproached)
        {
            //Ordering Code will go here****
            Debug.Log("HI!!!");
        }
    }

    //Helper Method that will Lerp the alien towards the player
    void Lerp()
    {
        float fractionOfJourney = 0.0f;

        // If the alien hasn't reached the stopping point, continue approaching
        if (distanceCovered < journeyLength)
        {
            // Move the alien closer to the end point
            distanceCovered += approachSpeed * Time.deltaTime;
            fractionOfJourney = distanceCovered / journeyLength;

            // Lerp the position between startPoint and endPoint
            alienInstance.transform.position = Vector3.Lerp(startPoint, endPoint, fractionOfJourney);

            // Lerp the size of the alien between startSize and endSize
            float currentSize = Mathf.Lerp(startSize, endSize, fractionOfJourney);
            alienInstance.transform.localScale = Vector3.one * currentSize;
        }

        // Check if the alien has reached the end point
        if (fractionOfJourney >= 1.0f)
        {
            hasApproached = true;  // The alien has finished approaching
        }
    }
}
