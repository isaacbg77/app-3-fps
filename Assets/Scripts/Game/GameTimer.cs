using TMPro;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    public float ElapsedSeconds { get; private set; } = 0f;

    private void Update()
    {
        if (GameManager.Instance.IsGameActive)
        {
            ElapsedSeconds += Time.deltaTime;
            timerText.text = GetFormattedTime(ElapsedSeconds);
        }
    }

    public static string GetFormattedTime(float timeInSeconds)
    {
        int minutes = Mathf.RoundToInt(timeInSeconds) / 60;
        int seconds = Mathf.RoundToInt(timeInSeconds) % 60;
        return minutes.ToString() + ":" + (seconds < 10 ? "0" + seconds.ToString() : seconds.ToString());
    }
}
