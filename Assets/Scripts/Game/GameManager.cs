using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public bool IsGamePaused { get; private set; } = false;
    public bool IsGameActive { get; private set; } = true;

    [SerializeField] private GameTimer timer;
    public GameTimer Timer => timer;

    private int enemiesKilled = 0;
    public int EnemiesKilled => enemiesKilled;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Update()
    {
        if (IsGameActive)
        {
            if (Input.GetButtonUp("Cancel") && !IsGamePaused)
            {
                SceneManager.LoadScene("Pause Menu", LoadSceneMode.Additive);
                PauseGame(true);
            }
        }
    }

    public void IncrementKills()
    {
        enemiesKilled++;
    }

    public void PauseGame(bool isPaused)
    {
        IsGamePaused = isPaused;
    }

    public void EndGame(bool levelCompleted)
    {
        IsGameActive = false;

        if (levelCompleted)
        {

        }
    }
}
