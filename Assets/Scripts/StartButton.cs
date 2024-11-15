using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    //Public method to load the Gameplay scene when button is clicked
    public void OnButtonClick()
    {
        //Load the Gameplay scene
        SceneManager.LoadScene("Gameplay");
    }

    //Quit Game
    public void QuitGame()
    {
        Debug.Log("QUIT GAME PRESSED");
        Application.Quit();
    }
}
