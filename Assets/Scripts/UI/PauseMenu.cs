using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public void ResumeGame()
    {
        SceneManager.UnloadSceneAsync("Pause Menu");
        GameManager.Instance.PauseGame(false);
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

        Time.timeScale = 0f;
    }
    
    private void OnDisable()
    {
        Time.timeScale = 1f;
    }
}
