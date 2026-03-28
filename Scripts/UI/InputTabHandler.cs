using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class InputTabHandler : MonoBehaviour
{
    public TMP_InputField usernameField;
    public TMP_InputField passwordField;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (usernameField.isFocused)
            {
                passwordField.Select();
                passwordField.ActivateInputField();
            }
            else if (passwordField.isFocused)
            {
                usernameField.Select();
                usernameField.ActivateInputField();
            }
        }
    }
}