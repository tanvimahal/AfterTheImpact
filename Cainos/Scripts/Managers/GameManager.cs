using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState currentState;

    public string username;
    public string password;
    public bool isLoggedIn = false;

    public int loadedCycle = 0;
    public bool isNewGame = true;
    public bool returnToPlayMenu = false;

    public int food_count;
    public int sapling_count;
    public int wood_count;

    public int total_score;
    public int total_trees_cut;
    public int total_trees_planted;
    public int total_animals_killed;
    public int total_buildings_built;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        SetState(GameState.MainMenu);
    }

    public void SetState(GameState newState)
    {
        currentState = newState;

        switch (currentState)
        {
            case GameState.MainMenu:
                Time.timeScale = 1;
                break;
            case GameState.Playing:
                Time.timeScale = 1;
                break;
            case GameState.Paused:
                Time.timeScale = 0;
                break;
            case GameState.GameOver:
                Time.timeScale = 0;
                break;
        }
    }

    public void StartNewGame()
    {
        Time.timeScale = 1;
        isNewGame = true;
        loadedCycle = 0;
        SetState(GameState.Playing);
        SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
    }

    public void LoadGameScene()
    {
        Time.timeScale = 1;
        SetState(GameState.Playing);
        SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1;
        isLoggedIn = true;
        SetState(GameState.MainMenu);
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        loadedCycle = 1;
        SetState(GameState.Playing);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}



