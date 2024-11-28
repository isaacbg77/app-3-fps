using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LeaderboardMenu : MonoBehaviour
{
    [SerializeField] private RectTransform scoresContainer;
    [SerializeField] private RectTransform highScoresContainer;
    [SerializeField] private RectTransform scoreText;

    private int[] scores;
    private int[] highScores;

    private void Start()
    {
        if (!PlayerPrefs.HasKey("NumPlayers")) return;

        GetScores();
        AddScoresToLeaderboard();
    }

    private void GetScores()
    {
        int numPlayers = PlayerPrefs.GetInt("NumPlayers");
        scores = new int[numPlayers];

        // Get scores from last game
        for (int i = 0; i < numPlayers; i++)
        {
            scores[i] = PlayerPrefs.GetInt("PlayerScore" + i);
        }

        // Get high scores
        highScores = Array.ConvertAll(PlayerPrefs.GetString("HighScores").Split(','), s => int.Parse(s));
    }

    private void AddScoresToLeaderboard()
    {
        for (int i = 0; i < scores.Length; i++)
        {
            RectTransform gameScore = Instantiate(scoreText);
            gameScore.SetParent(scoresContainer, false);
            gameScore.GetComponent<TextMeshProUGUI>().text =
                $"Player {i+1} ({PlayerPrefs.GetString("PlayerType"+i)}): {scores[i]}";
        }
        
        for (int i = highScores.Length-1; i >= 0; i--)
        {
            RectTransform highScore = Instantiate(scoreText);
            highScore.SetParent(highScoresContainer, false);
            highScore.GetComponent<TextMeshProUGUI>().text = $"{highScores.Length-i}. {highScores[i]}";
        }
    }

    public void Exit()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
