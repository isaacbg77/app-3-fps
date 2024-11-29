using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : Health
{
    private void Start()
    {
        UpdateUI();
    }

    protected override void HandleDeath()
    {
        GameManager.Instance.EndGame(false);
        SceneManager.LoadScene("Dead", LoadSceneMode.Additive);
        gameObject.SetActive(false);
    }
    
    public void UpdateUI()
    {
        UIManager.Instance.UpdatePlayerHealth(CurrentHealth);
    }
}
