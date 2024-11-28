using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DeadMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI enemiesKilled;
    [SerializeField] private TextMeshProUGUI elapsedTime;

    private void Awake()
    {
        enemiesKilled.text = "Enemies Killed: " + GameManager.Instance.EnemiesKilled.ToString();
        elapsedTime.text = "Elapsed Time: " + GameTimer.GetFormattedTime(GameManager.Instance.Timer.ElapsedSeconds);
    }
    
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void ExitGame()
    {
        SceneManager.LoadScene("Main Menu");
    }

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
