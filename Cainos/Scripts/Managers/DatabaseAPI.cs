using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;

public class DatabaseAPI : MonoBehaviour
{
    // public IEnumerator Login(string username, string password, Action<bool, string> callback)
    // {
    //     string json = "{\"username\":\"" + username + "\",\"password\":\"" + password + "\"}";

    //     Debug.Log("Sending login JSON: " + json);

    //     UnityWebRequest request = new UnityWebRequest("http://localhost:3000/login", "POST");

    //     byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);

    //     request.uploadHandler = new UploadHandlerRaw(bodyRaw);
    //     request.downloadHandler = new DownloadHandlerBuffer();
    //     request.SetRequestHeader("Content-Type", "application/json");

    //     yield return request.SendWebRequest();

    //     Debug.Log("Login Result: " + request.result);
    //     Debug.Log("Login Response: " + request.downloadHandler.text);

    //     if (request.result == UnityWebRequest.Result.Success)
    //     {
    //         GameManager.Instance.username = username;
    //         GameManager.Instance.password = password;
    //         GameManager.Instance.isLoggedIn = true;

    //         callback?.Invoke(true, "");
    //     }
    //     else
    //     {
    //         callback?.Invoke(false, "Login failed");
    //     }
    // }
    public IEnumerator SaveGame(string username, string password, int cycle, int food, int saplings, int wood, int total_score, int total_trees_cut, int total_trees_planted, int total_animals_killed, int total_buildings_built)
    {

        string json = "{\"username\":\"" + username + "\"," +
        "\"password\":\"" + password + "\"," +
        "\"cycle\":" + cycle + "," +
        "\"food\":" + food + "," +
        "\"saplings\":" + saplings + "," +
        "\"wood\":" + wood + "," +
        "\"totalScore\":" + total_score + "," +
        "\"totalTreesCut\":" + total_trees_cut + "," +
        "\"totalTreesPlanted\":" + total_trees_planted + "," +
        "\"totalAnimalsKilled\":" + total_animals_killed + "," +
        "\"totalBuildingsBuilt\":" + total_buildings_built + "}";
        UnityWebRequest request = new UnityWebRequest("https://aftertheimpact.onrender.com/save", "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        Debug.Log("Save Result: " + request.result);
        Debug.Log("Response: " + request.downloadHandler.text);
    }

    public IEnumerator LoadGame(string username, string password, System.Action<SaveResponse> onLoaded)
    {
        string json = "{\"username\":\"" + username + "\",\"password\":\"" + password + "\"}";

        UnityWebRequest request = new UnityWebRequest("https://aftertheimpact.onrender.com/load", "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string text = request.downloadHandler.text;
            Debug.Log("Load Response: " + text);

            var obj = JsonUtility.FromJson<SaveResponse>(text);
            onLoaded?.Invoke(obj);
        }
        else
        {
            Debug.LogError("Load Failed: " + request.error);
            onLoaded?.Invoke(new SaveResponse()); // defaults to 0s
        }
    }

    public IEnumerator Login(string username, string password, System.Action<bool, string> onResult)
    {
        string json = "{\"username\":\"" + username + "\",\"password\":\"" + password + "\"}";

        UnityWebRequest request = new UnityWebRequest("https://aftertheimpact.onrender.com/login", "POST");

        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);

        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        Debug.Log("Login Result: " + request.result);
        Debug.Log("Response Code: " + request.responseCode);
        Debug.Log("Response Text: " + request.downloadHandler.text);

        if (request.result == UnityWebRequest.Result.Success)
        {
            // IMPORTANT: check response content, not just success
            if (request.downloadHandler.text.Contains("success"))
            {
                GameManager.Instance.username = username;
                GameManager.Instance.password = password;
                GameManager.Instance.isLoggedIn = true;

                onResult?.Invoke(true, "");
            }
            else
            {
                onResult?.Invoke(false, "Login failed");
            }
        }
        else
        {
            string error = request.downloadHandler.text;

            if (error.Contains("Incorrect password"))
                onResult?.Invoke(false, "Wrong password");
            else if (error.Contains("User not found"))
                onResult?.Invoke(false, "User not found");
            else
                onResult?.Invoke(false, "Login failed");
        }
    }

    public IEnumerator SignUp(string username, string email, string password, System.Action<bool, string> onResult)
    {
        string json = "{\"username\":\"" + username + "\",\"email\":\"" + email + "\",\"password\":\"" + password + "\"}";

        UnityWebRequest request = new UnityWebRequest("https://aftertheimpact.onrender.com/signup", "POST");

        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);

        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        Debug.Log("Signup Result: " + request.result);
        Debug.Log("Response: " + request.downloadHandler.text);

        if (request.result == UnityWebRequest.Result.Success)
        {
            onResult?.Invoke(true, "Account created!");
        }
        else
        {
            string error = request.downloadHandler.text;

            if (error.Contains("Username already exists"))
                onResult?.Invoke(false, "Username already exists");
            else
                onResult?.Invoke(false, "Signup failed");
        }
    }

    [System.Serializable]
    public class SaveResponse
    {
        public int disaster_cycle;
        public int food_count;
        public int sapling_count;
        public int wood_count;
        public int total_score;
        public int total_trees_cut;
        public int total_trees_planted;
        public int total_animals_killed;
        public int total_buildings_built;

    }
}