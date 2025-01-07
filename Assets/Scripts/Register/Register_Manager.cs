using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using UnityEngine.Networking;
using Unity.VisualScripting;

public class Register_Manager : MonoBehaviour
{
    public TMP_InputField Username_input;
    public TMP_InputField Password_input;
    public Button App_Back_Button;
    public Button App_Register_Button;
    public GameObject RegisterPanel;
    public GameObject LoginPanel;

    private string Register_server_url = "http://43.202.48.66:3000/register";
    void Start()
    {
        RegisterPanel.SetActive(false);
    }

    public void App_Back_button_click()
    {
        Debug.Log("click back button");
        clear_input();
        LoginPanel.SetActive(true);
        RegisterPanel.SetActive(false);
    }

    public void clear_input()
    {
        Username_input.text="";
        Password_input.text="";
    }

    public void App_Register_button_click()
    {
        string username = Username_input.text;
        string password = Password_input.text;
        StartCoroutine(Send_Register_Request(username, password));
        clear_input();
    }

    private IEnumerator Send_Register_Request(string username, string password)
    {
        string Register_Json_data = $"{{\"username\":\"{username}\",\"password\":\"{password}\"}}";
        UnityWebRequest request = new UnityWebRequest(Register_server_url, "POST");
         byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(Register_Json_data);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // 요청 보내기
        yield return request.SendWebRequest();

        // 응답 처리
        if (request.result == UnityWebRequest.Result.Success)
        {
            if (request.responseCode == 201) // 201 Created
            {
                Debug.Log("Register Successful");

                


                // 회원가입 성공 시 로그인 화면으로 이동
                LoginPanel.SetActive(true);
                RegisterPanel.SetActive(false);
            }
            else if (request.responseCode == 400) // 400 Bad Request
            {
                Debug.Log("Registration failed: Invalid data");
            }
            else
            {
                Debug.Log("Registration failed: " + request.downloadHandler.text);
            }
        }
        else
        {
            Debug.LogError("Error: " + request.error);
        }



    }



   

}
