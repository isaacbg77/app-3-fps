using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI playerHealth;
    [SerializeField] private TextMeshProUGUI playerAmmo;

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

    public void UpdatePlayerHealth(int value)
    {
        playerHealth.text = value.ToString();
    }
    
    public void UpdatePlayerAmmo(int value)
    {
        playerAmmo.text = "Ammo: " + value.ToString();
    }
}
