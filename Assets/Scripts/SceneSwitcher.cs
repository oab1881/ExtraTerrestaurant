using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    //Amount to shift the camera by
    [SerializeField]
    private float shiftAmount;

    //Reference to the camera
    private Camera mainCamera;

    void Start()
    {
        //Get reference to main camera
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main camera not found!");
        }
    }

    //Detect if player clicked
    void OnMouseDown()
    {
        if (mainCamera != null)
        {
            // Move the camera to the right by the shift amount
            mainCamera.transform.position += new Vector3(shiftAmount, 0, 0);
        }
    }
}