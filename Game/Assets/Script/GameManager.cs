using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    
    public TMP_Text scoreText;
    public int scoreCount = 0;
    public TMP_Text coinText;
    public int coinCount = 0;
    public TMP_Text levelText;
    public int levelCount = 1;
    public string levelContext = "";

    [SerializeField] private GameObject player;

    private LevelGenerator _levelGenerator;
    // Start is called before the first frame update
    
    private void Awake() {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        coinCount = StaticData.coins;
        scoreCount = StaticData.score;
        levelCount = StaticData.level;
        levelContext = StaticData.levelContext;
        coinText.text = "Coins: " + coinCount.ToString();
        scoreText.text = "Score: " + scoreCount.ToString();
        levelText.text = "Level: " + levelCount.ToString() + levelContext;
        _levelGenerator = (LevelGenerator) FindObjectOfType(typeof(LevelGenerator));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject GetPlayer()
    {
        return player;
    }

    public void IncreaseScore(int amount){
        scoreCount += amount;
        scoreText.text = "Score: " + scoreCount.ToString();
    }

    public void IncreaseCoins(int amount)
    {
        coinCount += amount;
        coinText.text = "Coins: " + coinCount.ToString();
    }

    public void DecreaseCoins(int amount)
    {
        coinCount -= amount;
        coinText.text = "Coins: " + coinCount.ToString();
    }

    public void EnemyDefeated()
    {
        _levelGenerator.EnemyDefeated();
    }

}
