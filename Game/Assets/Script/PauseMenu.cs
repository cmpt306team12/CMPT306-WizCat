using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject warningMenu;
    public GameObject controlsMenu;
    public static bool isPaused;
    
    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
        warningMenu.SetActive(false);
        controlsMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
                warningMenu.SetActive(false);
                controlsMenu.SetActive(false);
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
        isPaused = false;
        StaticData.Reset();
    }

    public void ShowWarningMenu()
    {
        pauseMenu.SetActive(false);
        warningMenu.SetActive(true);
    }

    public void CloseWarningMenu()
    {
        warningMenu.SetActive(false);
        pauseMenu.SetActive(true);
    }

    public void ShowControlsMenu()
    {
        pauseMenu.SetActive(false);
        controlsMenu.SetActive(true);
    }

    public void CloseControlsMenu()
    {
        controlsMenu.SetActive(false);
        pauseMenu.SetActive(true);
    }
    
}
