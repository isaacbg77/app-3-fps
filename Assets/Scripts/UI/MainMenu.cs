using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Level1");
    }

    public void ShowLeaderboard()
    {
        SceneManager.LoadScene("Leaderboard");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
