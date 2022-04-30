using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private GameObject mainMenuUI;
    private GameObject controlsUI;
    private GameObject creditsUI;

    // Start is called before the first frame update
    void Start()
    {
        mainMenuUI = GameObject.Find("MainMenu");
        controlsUI = GameObject.Find("ControlsUI");
        creditsUI = GameObject.Find("CreditsUI");

        mainMenuUI.SetActive(true);
        controlsUI.SetActive(false);
        creditsUI.SetActive(false);
    }

    public void ControlsMenu()
    {
        mainMenuUI.SetActive(false);
        controlsUI.SetActive(true);
    }

    public void CreditsMenu()
    {
        mainMenuUI.SetActive(false);
        creditsUI.SetActive(true);
    }

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
    
    public void QuitGame()
    {
        Application.Quit();
    }
}


