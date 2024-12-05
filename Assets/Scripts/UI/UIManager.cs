using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField, Range(0, 1)] private float fadeSpeed;
    [SerializeField] private TextMeshProUGUI playerHealth;
    [SerializeField] private TextMeshProUGUI playerAmmo;
    [SerializeField] private TextMeshProUGUI introText;

    [SerializeField] private RectTransform hitMarker;
    [SerializeField, Range(0, 1)] private float hitMarkerDuration;

    [SerializeField] private RawImage hitImage;
    [SerializeField, Range(0, 1)] private float hitImageDuration;
    [SerializeField, Range(0, 1)] private float hitImageAlpha;
    private Coroutine hitCoroutine;

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
            yield return new WaitForSeconds(fadeSpeed);
        }

        yield return new WaitForSeconds(duration);
        
        // Fade out
        while (fadeColor.a > 0)
        {
            fadeColor.a -= 0.1f;
            message.color = fadeColor;
            yield return new WaitForSeconds(fadeSpeed);
        }
    }

    public void ShowHitMarker()
    {
        StartCoroutine(ShowHitMarkerAsync());
    }

    private IEnumerator ShowHitMarkerAsync()
    {
        hitMarker.gameObject.SetActive(true);
        yield return new WaitForSeconds(hitMarkerDuration);
        hitMarker.gameObject.SetActive(false);
    }

    public void ShowHitImage()
    {
        if (hitCoroutine != null) {
            StopCoroutine(hitCoroutine);
        }
        hitCoroutine = StartCoroutine(ShowHitImageAsync());
    }

    private IEnumerator ShowHitImageAsync()
    {
        Color fadeColor = hitImage.color;
        fadeColor.a = hitImageAlpha;
        hitImage.color = fadeColor;
        yield return new WaitForSeconds(hitImageDuration);

        while (fadeColor.a > 0)
        {
            fadeColor.a -= 0.01f;
            hitImage.color = fadeColor;
            yield return new WaitForSeconds(fadeSpeed);
        }
    }
}
