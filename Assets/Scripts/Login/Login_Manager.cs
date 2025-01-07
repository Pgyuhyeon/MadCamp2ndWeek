using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using UnityEngine.Networking;
using Unity.VisualScripting;

public class Login_Manager : MonoBehaviour
{
    public TMP_InputField Username_input;
    public TMP_InputField Password_input;
    public Button App_Login_Button;
    public Button App_Register_Button;
    public GameObject LoginPanel;
    public GameObject Player;
    public GameObject RegisterPanel;
    public GameObject mainMenuPanel;
    private string Login_server_url = "http://43.202.48.66:3000/login";

    void Awake()
    {
        PlayerPrefs.SetInt("IsLoggedIn", 0); // 로그인 상태 초기화
        PlayerPrefs.Save();
    }

    void Start()
    {
        if (PlayerPrefs.GetInt("IsLoggedIn", 0) == 1)
        {
            LoginPanel.SetActive(false);
            Player.SetActive(true);
            mainMenuPanel.SetActive(true);
        }
        else
        {
            App_Login_Button.onClick.AddListener(App_Login_button_click);
            Player.SetActive(false);
        }
    }

    public void clear_input()
    {
        Username_input.text = "";
        Password_input.text = "";
    }

    public void App_Login_button_click()
    {
        string username = Username_input.text;
        string password = Password_input.text;
        Debug.Log("I clicked Login Button");
        StartCoroutine(Send_Login_Request(username, password));
    }

    private IEnumerator Send_Login_Request(string username, string password)
    {
        string Login_json_data = $"{{\"username\":\"{username}\",\"password\":\"{password}\"}}";
        UnityWebRequest request = new UnityWebRequest(Login_server_url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(Login_json_data);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        // 응답 처리
        if (request.result == UnityWebRequest.Result.Success)
        {
            if (request.responseCode == 200)
            {
                Debug.Log("Login Successful");

                // 서버 응답 데이터 파싱 및 로컬 저장
                UserData userData = JsonUtility.FromJson<UserData>(request.downloadHandler.text);
                SaveUserDataLocally(userData);
                Debug.Log("Server Response: " + request.downloadHandler.text);

                PlayerPrefs.SetInt("IsLoggedIn", 1);
                PlayerPrefs.Save();
                clear_input();
                LoginPanel.SetActive(false);
                mainMenuPanel.SetActive(true);
            }
            else if (request.responseCode == 401)
            {
                Debug.Log("Invalid username or password.");
            }
        }
        else
        {
            Debug.Log("Error: " + request.error);
        }
    }

    public void App_Register_button_click()
    {
        Debug.Log("I clicked Register Button");
        clear_input();
        LoginPanel.SetActive(false);
        RegisterPanel.SetActive(true);
    }

    private void SaveUserDataLocally(UserData userData)
    {
        PlayerPrefs.SetString("Username", userData.username);
        PlayerPrefs.SetInt("HighestScore", userData.highest_score);
        PlayerPrefs.Save();

        Debug.Log($"User data saved locally: Username={userData.username}, HighestScore={userData.highest_score}");
    }

    [System.Serializable]
    public class UserData
    {
        public int id;
        public string username;
        public int highest_score;
    }
}
