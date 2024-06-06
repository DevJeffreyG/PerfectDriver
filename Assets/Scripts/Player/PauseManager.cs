using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public static bool isPaused = false;
    
    [SerializeField] private GameObject pauseMenu;
    private Settings settings;

    private void Start()
    {
        this.settings = ProfileController.getProfile().getSettings();
        pauseMenu.SetActive(false);
    }

    void Update()
    {
        if(this.settings.Down(Settings.SettingName.Back))
        {
            if (isPaused) this.ResumeGame();
            else this.PauseGame();
        }
    }

    public void GotoMainMenu()
    {
        this.ResumeGame();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("MainMenu");
    }

    public void ResetGame()
    {
        this.ResumeGame();        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void PauseGame()
    {
        Cursor.lockState = CursorLockMode.None;
        isPaused = true;
        Time.timeScale = 0f;

        pauseMenu.SetActive(isPaused);
        Cursor.visible = isPaused;
    }

    public void ResumeGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        isPaused = false;
        Time.timeScale = 1f;

        pauseMenu.SetActive(isPaused);
        Cursor.visible = isPaused;
    }
}
