using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * REFERENCE: START MENU in Unity - https://www.youtube.com/watch?v=zc8ac_qUXQY
 */

/*
 * Script to handle main menu functionality 
 */
public class MainMenu : MonoBehaviour
{
    private GameObject mainMenuUI;
    private GameObject controlsUI;
    private GameObject creditsUI;

    void Start()
    {
        mainMenuUI = GameObject.Find("MainMenu");
        controlsUI = GameObject.Find("ControlsUI");
        creditsUI = GameObject.Find("CreditsUI");

        mainMenuUI.SetActive(true);
        controlsUI.SetActive(false);
        creditsUI.SetActive(false);
    }

    /*
     * Method to handle setting the controls menu to active  
     */
    public void ControlsMenu()
    {
        mainMenuUI.SetActive(false);
        controlsUI.SetActive(true);
    }

    /*
     * Method to handle setting the credits menu to active 
     */
    public void CreditsMenu()
    {
        mainMenuUI.SetActive(false);
        creditsUI.SetActive(true);
    }

    /*
     * Method to handle the functionality of the back button 
     * Taking the player back to the main menu from the controls or credits menu 
     */
    public void Back()
    {
        if (controlsUI.activeSelf)
        {
            controlsUI.SetActive(false);
        }
        else if (creditsUI.activeSelf)
        {
            creditsUI.SetActive(false);
        }

        mainMenuUI.SetActive(true);
    }
    
    /*
     * Method to quit the game from the main menu 
     */
    public void QuitGame()
    {
        Application.Quit();
    }
}


