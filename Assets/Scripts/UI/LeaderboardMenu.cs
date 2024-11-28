using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LeaderboardMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI highScoreKills;
    [SerializeField] private TextMeshProUGUI highScoreTime;

    private void Start()
    {
        if (!PlayerPrefs.HasKey("HighScoreKills") || !PlayerPrefs.HasKey("HighScoreTime"))
            return;

        GetScores();
    }

    private void GetScores()
    {
        highScoreKills.text = "Most Enemies Killed: " + PlayerPrefs.GetInt("HighScoreKills").ToString();
        highScoreTime.text = "Best Time: " + GameTimer.GetFormattedTime(PlayerPrefs.GetFloat("HighScoreTime"));
    }

    public void Exit()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
