using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverHandling : MonoBehaviour
{

    public CanvasGroup gameOverScreen;
    public DeathMessage deathMessage;
    public LevelMusic gameOverMusic;
    public GameObject ambience;

    private bool messageVisible = false;

    // Start is called before the first frame update
    void Start()
    {
        gameOverScreen = GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOverScreen.alpha >= 1 && !messageVisible)
        {
            ambience.SetActive(false);
            gameOverMusic.GetComponent<LevelMusic>().CallPlayerDeath();
            deathMessage.GetComponent<DeathMessage>().TypeMessage();
            gameOverScreen.interactable = true;
            messageVisible = true;
            
        }
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
