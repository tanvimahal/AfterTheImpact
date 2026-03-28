using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class MenuController : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainMenuPanel;
    public GameObject loginPanel;
    public GameObject signUpPanel;
    public GameObject playMenuPanel;

    [Header("Extra Panels")]
    public GameObject howToPlayPanel;

    public DatabaseAPI databaseAPI;

    [Header("Login Inputs")]
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;

    [Header("Sign Up Inputs")]
    public TMP_InputField signupUsernameInput;
    public TMP_InputField signupPasswordInput;
    public TMP_InputField confirmPasswordInput;
    public TMP_InputField signupEmailInput;

    private void Start()
    {
        if (GameManager.Instance != null && GameManager.Instance.returnToPlayMenu)
        {
            ShowPlayMenu();
            GameManager.Instance.returnToPlayMenu = false;
        }
        else
        {
            ShowMainMenu();
        }

        if (howToPlayPanel != null)
            howToPlayPanel.SetActive(false);
    }

    private void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);
        loginPanel.SetActive(false);
        signUpPanel.SetActive(false);
        playMenuPanel.SetActive(false);
    }

    private void ShowLogin()
    {
        mainMenuPanel.SetActive(false);
        loginPanel.SetActive(true);
        signUpPanel.SetActive(false);
        playMenuPanel.SetActive(false);
    }

    private void ShowSignUp()
    {
        mainMenuPanel.SetActive(false);
        loginPanel.SetActive(false);
        signUpPanel.SetActive(true);
        playMenuPanel.SetActive(false);
    }

    private void ShowPlayMenu()
    {
        mainMenuPanel.SetActive(false);
        loginPanel.SetActive(false);
        signUpPanel.SetActive(false);
        playMenuPanel.SetActive(true);
    }

    public void OpenLogin()
    {
        ShowLogin();
    }

    public void OpenSignUp()
    {
        ShowSignUp();
    }

    public void BackToMainMenu()
    {
        ShowMainMenu();
    }

    public void BackToLogin()
    {
        ShowLogin();
    }

    public void BackToPlayMenu()
    {
        ShowPlayMenu();
    }

    public void StartGame()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager.Instance is NULL");
            return;
        }

        if (!GameManager.Instance.isLoggedIn)
        {
            Debug.Log("Login first!");
            return;
        }

        GameManager.Instance.StartNewGame();
    }

    public void Login()
    {
        string username = usernameInput.text;
        string password = passwordInput.text;

        if (username == "" || password == "")
        {
            Debug.Log("Please enter username and password");
            return;
        }

        Debug.Log("Logging in...");

        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager.Instance is NULL");
            return;
        }

        if (!GameManager.Instance.isLoggedIn)
        {
            StartCoroutine(databaseAPI.Login(username, password, (success, errorMsg) =>
            {
                if (success)
                {
                    Debug.Log("Login successful!");
                    GameManager.Instance.username = username;
                    GameManager.Instance.password = password;
                    GameManager.Instance.isLoggedIn = true;

                    ShowPlayMenu();
                }
                else
                {
                    Debug.LogError(errorMsg);
                }
            }));
        }
        else
        {
            Debug.Log("Already logged in!");
            ShowPlayMenu();
        }
    }

    public void SignUp()
    {
        string username = signupUsernameInput.text;
        string email = signupEmailInput.text;
        string password = signupPasswordInput.text;
        string confirm = confirmPasswordInput.text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) ||
            string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirm))
        {
            Debug.Log("Please fill all fields");
            return;
        }

        if (password != confirm)
        {
            Debug.Log("Passwords do not match");
            return;
        }

        StartCoroutine(databaseAPI.SignUp(username, email, password, (success, msg) =>
        {
            Debug.Log(msg);

            if (success)
            {
                usernameInput.text = username;
                passwordInput.text = "";

                ShowLogin();
            }
        }));
    }

    public void StartNewGame()
    {
        StartGame();
    }

    public void LoadGame()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager.Instance is NULL");
            return;
        }

        if (!GameManager.Instance.isLoggedIn)
        {
            Debug.Log("Login first!");
            return;
        }

        StartCoroutine(databaseAPI.LoadGame(
            GameManager.Instance.username,
            GameManager.Instance.password,
            (save) =>
            {
                GameManager.Instance.loadedCycle = save.disaster_cycle;
                GameManager.Instance.food_count = save.food_count;
                GameManager.Instance.sapling_count = save.sapling_count;
                GameManager.Instance.wood_count = save.wood_count;
                GameManager.Instance.total_score = save.total_score;
                GameManager.Instance.total_trees_cut = save.total_trees_cut;
                GameManager.Instance.total_trees_planted = save.total_trees_planted;
                GameManager.Instance.total_animals_killed = save.total_animals_killed;
                GameManager.Instance.total_buildings_built = save.total_buildings_built;
                GameManager.Instance.isNewGame = false;
                GameManager.Instance.LoadGameScene();
            }
        ));
    }

    public void LogoutToLogin()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.password = "";
            GameManager.Instance.isLoggedIn = false;
            GameManager.Instance.loadedCycle = 0;
            GameManager.Instance.isNewGame = true;
        }

        if (passwordInput != null) passwordInput.text = "";

        ShowLogin();
    }

    public void OnPasswordSubmit(string _)
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            Login();
        }
    }

    public void OpenHowToPlay()
    {
        if (howToPlayPanel != null)
        {
            mainMenuPanel.SetActive(false);
            howToPlayPanel.SetActive(true);
        }
    }

    public void CloseHowToPlay()
    {
        if (howToPlayPanel != null)
        {
            howToPlayPanel.SetActive(false);
            mainMenuPanel.SetActive(true);
        }
    }
}