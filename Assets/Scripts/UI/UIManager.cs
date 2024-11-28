using System.Collections;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField, Range(0, 1)] private float fadeSpeed;

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
        StartCoroutine(DoIntro());
    }

    public void UpdatePlayerHealth(int value)
    {
        playerHealth.text = value.ToString();
    }
    
    public void UpdatePlayerAmmo(int clipValue, int totalValue)
    {
        playerAmmo.text = clipValue.ToString() + " | " + totalValue.ToString();
    }

    private IEnumerator DoIntro()
    {
        Color fadeColor = introText.color;

        // Fade in
        while (fadeColor.a < 1)
        {
            fadeColor.a += 0.1f;
            introText.color = fadeColor;
            yield return new WaitForSeconds(fadeSpeed);
        }

        yield return new WaitForSeconds(5f);
        
        // Fade out
        while (fadeColor.a > 0)
        {
            fadeColor.a -= 0.1f;
            introText.color = fadeColor;
            yield return new WaitForSeconds(fadeSpeed);
        }
    }
}
