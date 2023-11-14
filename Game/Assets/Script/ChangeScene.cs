using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public int sceneBuildIndex;

    private GameManager gameManager;
    private GameObject player;
    private Health health;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.instance;
        player = gameManager.GetPlayer();
        health = player.GetComponent<Health>();
    }

    public void sceneChange()
    {
        // store scene game data in StaticData for next scene
        StaticData.coins = gameManager.coinCount;
        StaticData.score = gameManager.scoreCount;
        StaticData.currentHealth = health.currentHealth;
        StaticData.maxHealth = health.maxHealth;
        print("Switching scene to " + sceneBuildIndex);
        SceneManager.LoadScene(sceneBuildIndex, LoadSceneMode.Single);
    }
}
