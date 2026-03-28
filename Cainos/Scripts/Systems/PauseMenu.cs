using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseUI;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }

    }

    void TogglePause()
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
            pauseUI.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            pauseUI.SetActive(false);
        }
    }

    public void Resume()
    {
        Time.timeScale = 1;
        pauseUI.SetActive(false);
    }

    public void Restart()
    {
        Time.timeScale = 1;
        GameManager.Instance.RestartGame();
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        GameManager.Instance.ReturnToMenu();
    }

}
