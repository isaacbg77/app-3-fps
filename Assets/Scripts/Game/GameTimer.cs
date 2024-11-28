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
            timerText.text = GetFormattedTime();
        }
    }

    public string GetFormattedTime()
    {
        int minutes = Mathf.RoundToInt(ElapsedSeconds) / 60;
        int seconds = Mathf.RoundToInt(ElapsedSeconds) % 60;
        return minutes.ToString() + ":" + (seconds < 10 ? "0" + seconds.ToString() : seconds.ToString());
    }
}
