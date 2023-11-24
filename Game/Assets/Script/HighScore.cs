using UnityEngine;
using TMPro;
using System;
using System.Linq;
using System.Collections.Generic;

public class HighScore : MonoBehaviour
{
    public TMP_Text highScoreText;

    void Start()
    {
        LoadAndDisplayHighScores();
    }

    private void DisplayHighScores(List<int> highScores)
    {
        highScoreText.text = "High Scores:\n";

        // Display each high score on a new line and log to the console
        for (int i = 0; i < highScores.Count; i++)
        {
            highScoreText.text += $"{i + 1}. {highScores[i]}\n";
            Debug.Log($"High Score {i + 1}: {highScores[i]}");
        }

        // Log the entire list of high scores
        Debug.Log("All High Scores: " + string.Join(", ", highScores));
    }


    private void LoadAndDisplayHighScores()
    {
        if (PlayerPrefs.HasKey(GameManager.HighScoreKey))
        {
            string[] scoreStrings = PlayerPrefs.GetString(GameManager.HighScoreKey).Split(',');

            // Convert the string array to a list of integers
            var highScores = new List<int>();
            foreach (var scoreString in scoreStrings)
            {
                int score = int.Parse(scoreString);
                highScores.Add(score);
            }

            DisplayHighScores(highScores);
        }
        else
        {
            highScoreText.text = "No high scores yet!";
        }
    }
}
