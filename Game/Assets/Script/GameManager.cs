using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

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

    public const string HighScoreKey = "HighScore";
    private List<int> highScores = new List<int>();
    private const int MaxHighScores = 10;

    [SerializeField] private GameObject player;

    private LevelGenerator _levelGenerator;

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        // ClearPlayerPrefs();
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

    public void PlayerDiedHighScore()
    {
        highScores.Add(scoreCount);
        highScores.Sort((a, b) => b.CompareTo(a));
        highScores = highScores.Take(MaxHighScores).ToList();

        SaveHighScores();
    }


    public void SaveHighScores()
    {
        // Load existing high scores from PlayerPrefs
        string existingScoresString = PlayerPrefs.GetString(HighScoreKey, "");
        List<int> existingScores = new List<int>();

        if (!string.IsNullOrEmpty(existingScoresString))
        {
            existingScores = existingScoresString.Split(',').Select(int.Parse).ToList();
        }
        // take top 10
        existingScores.Add(scoreCount);
        existingScores.Sort((a, b) => b.CompareTo(a));
        existingScores = existingScores.Take(MaxHighScores).ToList();

        string scoresString = string.Join(",", existingScores.Select(score => score.ToString()).ToArray());
        PlayerPrefs.SetString(HighScoreKey, scoresString);
        PlayerPrefs.Save();
    }



}

