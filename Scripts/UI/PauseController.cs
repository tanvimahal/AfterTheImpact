using UnityEngine;

public class PauseController : MonoBehaviour
{
    public GameObject pauseUI;
    public ShopPanelController shopPanelController;

    void Awake()
    {
        if (pauseUI != null)
        {
            pauseUI.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        if (Time.timeScale == 1f)
        {
            Pause();
        }
        else
        {
            Resume();
        }
    }

    public void Pause()
    {
        Time.timeScale = 0f;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetState(GameState.Paused);
        }

        if (shopPanelController != null)
        {
            shopPanelController.gameObject.SetActive(false);
        }

        if (pauseUI != null)
        {
            pauseUI.SetActive(true);
        }
    }

    public void Resume()
    {
        Time.timeScale = 1f;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetState(GameState.Playing);
        }

        if (shopPanelController != null)
        {
            shopPanelController.gameObject.SetActive(true);
            shopPanelController.HideShop(); 
        }

        if (pauseUI != null)
        {
            pauseUI.SetActive(false);
        }
    }

    public void Restart()
    {
        Time.timeScale = 1f;

        if (pauseUI != null)
        {
            pauseUI.SetActive(false);
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.RestartGame();
        }
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;

        if (pauseUI != null)
        {
            pauseUI.SetActive(false);
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.returnToPlayMenu = true;
            GameManager.Instance.ReturnToMenu();
        }
    }
}