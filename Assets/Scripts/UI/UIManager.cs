using System.Collections;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField, Range(0, 1)] private float messageFadeSpeed;

    [SerializeField] private TextMeshProUGUI playerHealth;
    [SerializeField] private TextMeshProUGUI playerAmmo;
    [SerializeField] private TextMeshProUGUI introText;

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

    private void Start()
    {
        ShowMessage(introText, 5f);
    }

    public void UpdatePlayerHealth(int value)
    {
        playerHealth.text = value.ToString();
    }
    
    public void UpdatePlayerAmmo(int clipValue, int totalValue)
    {
        playerAmmo.text = clipValue.ToString() + " | " + totalValue.ToString();
    }

    public void ShowMessage(TextMeshProUGUI message, float duration)
    {
        StartCoroutine(ShowMessageAsync(message, duration));
    }

    private IEnumerator ShowMessageAsync(TextMeshProUGUI message, float duration)
    {
        Color fadeColor = message.color;

        // Fade in
        while (fadeColor.a < 1)
        {
            fadeColor.a += 0.1f;
            message.color = fadeColor;
            yield return new WaitForSeconds(messageFadeSpeed);
        }

        yield return new WaitForSeconds(duration);
        
        // Fade out
        while (fadeColor.a > 0)
        {
            fadeColor.a -= 0.1f;
            message.color = fadeColor;
            yield return new WaitForSeconds(messageFadeSpeed);
        }
    }
}
