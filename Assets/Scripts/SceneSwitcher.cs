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
    // Store the initial camera position
    private Vector3 initialPosition;

    // Track whether the camera is toggled
    private bool isToggled = false;

    void Start()
    {
        // Get reference to the main camera
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main camera not found!");
        }
        else
        {
            // Store the initial position of the camera
            initialPosition = mainCamera.transform.position;
        }
    }

    // Detect mouse click on the object
    void OnMouseDown()
    {
        if (mainCamera != null)
        {
            if (isToggled)
            {
                // Move the camera back to the initial position
                mainCamera.transform.position = initialPosition;
            }
            else
            {
                // Shift the camera to the new position
                mainCamera.transform.position += new Vector3(shiftAmount, 0, 0);
            }

            // Toggle the state
            isToggled = !isToggled;
        }
    }
}