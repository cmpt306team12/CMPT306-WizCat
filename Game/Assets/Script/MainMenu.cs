using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Animator transition;
    public GameObject mainMenu;
    public GameObject highScoreMenu;
    public GameObject creditsMenu;
    public GameObject creditsButton;

    // How-To-Play pages
    public GameObject howToPlayText;
    public List<GameObject> pages;
    private int currentPageIndex;
    

    private void Start()
    {
        mainMenu.SetActive(true);
        highScoreMenu.SetActive(false);
        creditsMenu.SetActive(false);
        howToPlayText.SetActive(false);

        // Set all pages to be inactive
        for (int i = 0; i < 6; i++)
        {
            pages[i].SetActive(false);
        }
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

    public void ShowHighScores()
    {
        mainMenu.SetActive(false);
        creditsButton.SetActive(false);
        highScoreMenu.SetActive(true);
    }

    public void CloseHighScoreMenu()
    {
        highScoreMenu.SetActive(false);
        mainMenu.SetActive(true);
        creditsButton.SetActive(true);
    }
    
    public void ShowCreditsMenu()
    {
        mainMenu.SetActive(false);
        creditsButton.SetActive(false);
        creditsMenu.SetActive(true);
    }

    public void CloseCreditsMenu()
    {
        creditsMenu.SetActive(false);
        mainMenu.SetActive(true);
        creditsButton.SetActive(true);
    }

    public void ShowHowToPlayMenu()
    {
        mainMenu.SetActive(false);
        creditsButton.SetActive(false);
        howToPlayText.SetActive(true);
        pages[0].SetActive(true);
        currentPageIndex = 0;
    }

    public void NextPage()
    {
        if (currentPageIndex < pages.Count - 1)
        {
            pages[currentPageIndex].SetActive(false);
            currentPageIndex++;
            pages[currentPageIndex].SetActive(true);
        }
    }
    
    public void PrevPage()
    {
        if (currentPageIndex > 0)
        {
            pages[currentPageIndex].SetActive(false);
            currentPageIndex--;
            pages[currentPageIndex].SetActive(true);
        }
    }
    
    public void CloseHowToPlayMenu()
    {
        if (pages[0].activeInHierarchy)
        {
            howToPlayText.SetActive(false);
            pages[0].SetActive(false);
            mainMenu.SetActive(true);
            creditsButton.SetActive(true);
        }
        else
        {
            howToPlayText.SetActive(false);
            pages[5].SetActive(false);
            mainMenu.SetActive(true);
            creditsButton.SetActive(true);
        }
        currentPageIndex = 0;
    }
}
