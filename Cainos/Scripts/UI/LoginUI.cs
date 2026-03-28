using UnityEngine;
using UnityEngine.UI;

public class LoginUI : MonoBehaviour
{
    public InputField usernameInput;
    public InputField passwordInput;

    public GameObject loginPanel;
    public Text messageText;

    public DatabaseAPI databaseAPI;

    public Button startButton;
    public Button loadButton;

    void Start()
    {
        if (GameManager.Instance.isLoggedIn)
        {
            loginPanel.SetActive(false);

            startButton.gameObject.SetActive(true);
            loadButton.gameObject.SetActive(true);
        }
        else
        {
            startButton.gameObject.SetActive(false);
            loadButton.gameObject.SetActive(false);
        }
    }

    public void OnLoginClicked()
    {
        string username = usernameInput.text;
        string password = passwordInput.text;

        if (username == "" || password == "")
        {
            messageText.text = "Please enter username and password";
            return;
        }

        messageText.text = "Logging in...";

        StartCoroutine(databaseAPI.Login(username, password, (success, errorMsg) =>
        {
            if (success)
            {
                messageText.text = "Login successful!";
                loginPanel.SetActive(false);

                startButton.gameObject.SetActive(true);
                loadButton.gameObject.SetActive(true);
            }
            else
            {
                messageText.text = errorMsg;
            }
        }));
    }
}