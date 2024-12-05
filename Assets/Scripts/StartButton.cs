using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    //Public method to load the Tutorial scene when button is clicked
    public void OnButtonClickTutorial()
    {
        //Load the Gameplay scene
        SceneManager.LoadScene("TutorialScene");
    }

    //Quit Game
    public void QuitGame()
    {
        Debug.Log("QUIT GAME PRESSED");
        Application.Quit();
    }

    //Public method to load the Gameplay scene when button is clicked
    public void OnButtonClickGameplay()
    {
        Debug.Log("Skip tutorial pressed");
        //Load the Gameplay scene
        SceneManager.LoadScene("Gameplay");
    }
}
