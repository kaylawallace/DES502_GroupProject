using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;
    private GameObject pauseMenuUI;
    public LevelLoader levelLoader;
    private GameObject controlsUI;

    private void Start()
    {
        pauseMenuUI = GameObject.Find("PauseMenu");
        pauseMenuUI.SetActive(false);
        controlsUI = GameObject.Find("ControlsUI");
        controlsUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
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

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void LoadMenu()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        levelLoader.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ControlsMenu()
    {
        pauseMenuUI.SetActive(false);
        controlsUI.SetActive(true);
    }

    public void BackToPause()
    {
        controlsUI.SetActive(false);
        pauseMenuUI.SetActive(true);
    }
}
