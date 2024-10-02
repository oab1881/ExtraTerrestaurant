using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    // Name of the scene to load
    [SerializeField]
    private string sceneName;


    // Method to detect clicks
    void OnMouseDown()
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            // Load the scene with the provided name
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError("Scene name is not specified!");
        }
    }
}
