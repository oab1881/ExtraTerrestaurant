using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Written by Owen
//This script allows player to open up the pause screen which will display some buttons and darken the backgroun
public class PauseMenu : MonoBehaviour
{
    //variable tracks if game is paused
    public static bool GameIsPaused = false;

    //gameObject containing entire pause screen
    public GameObject pauseMenuUI;

    // Update is called once per frame
    void Update()
    {
        //Track if player presses escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //If game is unpaused then resume
            if(GameIsPaused)
            {
                Resume();   //Call resume function
            }
            //If game is unpaused and esc is pressed then pause
            else
            {
                Pause();
            }
        }
    }

    //Resume function resumes the game and can be called from the OnClick function of resume button
    public void Resume()
    {
        pauseMenuUI.SetActive(false);    //Pause screen is now invisible
        Time.timeScale = 1f; //resume time
        GameIsPaused = false;    //game is now unpaused
    }

    //Pause function freezes time and brings up all the pause UI
    void Pause()
    {
        pauseMenuUI.SetActive(true);    //Make pause screen visible
        Time.timeScale = 0f; //freeze time
        GameIsPaused = true;    //game is now paused
    }

    //Function to exit to menu when clicking exit button
    public void ExitToMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Resume();
    }
}
