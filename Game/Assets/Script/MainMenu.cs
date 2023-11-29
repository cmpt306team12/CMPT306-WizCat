using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Animator transition;
    public GameObject mainMenu;
    public GameObject highScoreMenu;
    public GameObject creditsMenu;
    public GameObject creditsButton;

    private void Start()
    {
        mainMenu.SetActive(true);
        highScoreMenu.SetActive(false);
        creditsMenu.SetActive(false);
    }

    public void PlayGame()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(levelIndex);
    }

    public void showHighScores()
    {
        mainMenu.SetActive(false);
        creditsButton.SetActive(false);
        highScoreMenu.SetActive(true);
    }

    public void closeHighScoreMenu()
    {
        highScoreMenu.SetActive(false);
        mainMenu.SetActive(true);
        creditsButton.SetActive(true);
    }
    
    public void showCreditsMenu()
    {
        mainMenu.SetActive(false);
        creditsButton.SetActive(false);
        creditsMenu.SetActive(true);
    }

    public void closeCreditsMenu()
    {
        creditsMenu.SetActive(false);
        mainMenu.SetActive(true);
        creditsButton.SetActive(true);
    }
}
