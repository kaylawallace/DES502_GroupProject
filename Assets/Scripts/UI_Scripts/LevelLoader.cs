using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * REFERENCE: Brackeys - How to make AWESOME Scene Transitions in Unity! - https://www.youtube.com/watch?v=CE9VOZivb3I
 */

/*
 * Script responsible for loading levels in the game
 */
public class LevelLoader : MonoBehaviour
{
    public Animator anim;
    public float transitionTime = 1f; 
    
    /*
     * Method to load the next level in the build
     */
    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    /*
     * Method to load the previous level in the build 
     */
    public void LoadPrevLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex - 1));
    }

    /*
     * Method to load a specific level in the build 
     */
    public void LoadScene(int sceneIndex)
    {
        StartCoroutine(LoadLevel(sceneIndex));
    }

    /*
     * Coroutine that handles the fade to black animation and loading the level required 
     */
    IEnumerator LoadLevel(int levelIndex)
    {    
        anim.SetTrigger("start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelIndex);
    }

    /*
     * Method to quit the game altogether
     */
    public void QuitGame()
    {
        Application.Quit();
    }
}
