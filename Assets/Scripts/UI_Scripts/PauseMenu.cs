using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * REFERENCE: Brackeys - PAUSE MENU in Unity - https://www.youtube.com/watch?v=JivuXdrIHK0
 */

/*
 * Script to handle the pause menu functionality 
 */
public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;
    public LevelLoader levelLoader;

    private GameObject pauseMenuUI;   
    private GameObject controlsUI;

    private void Start()
    {
        pauseMenuUI = GameObject.Find("PauseMenu");
        pauseMenuUI.SetActive(false);

        controlsUI = GameObject.Find("ControlsUI");
        controlsUI.SetActive(false);
    }

    void Update()
    {
        // Pause/Resume game on Escape press 
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    /*
     * Method to handle resuming the game from a paused state 
     */
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    /*
     * Method to handle pausing the game 
     */
    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    /*
     * Method to load the main menu from the pause menu 
     */
    public void LoadMenu()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        levelLoader.LoadScene(0);
    }

    /*
     * Method to quit the game from the pause menu 
     */
    public void QuitGame()
    {
        Application.Quit();
    }

    /*
     * Method to set the controls menu to active 
     */
    public void ControlsMenu()
    {
        pauseMenuUI.SetActive(false);
        controlsUI.SetActive(true);
    }

    /*
     * Method to go back to the pause menu from the controls menu 
     */
    public void BackToPause()
    {
        controlsUI.SetActive(false);
        pauseMenuUI.SetActive(true);
    }
}
