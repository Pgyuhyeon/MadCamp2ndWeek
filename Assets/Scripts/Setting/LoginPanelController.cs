using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoginPanelController : MonoBehaviour
{
    public Button loginButton;             // 로그인 버튼
    public TextMeshProUGUI buttonText;     // 버튼의 TextMeshProUGUI 컴포넌트

    public GameObject loginPanel;          // 로그인 패널
    public GameObject settingsPanel;       // 설정 패널

    public Sprite onSprite;                // On 상태 Sprite
    public Sprite offSprite;               // Off 상태 Sprite
    public string onText = "Login On";     // On 상태 텍스트
    public string offText = "Login Off";   // Off 상태 텍스트

    private bool isLoginOn = false;        // 로그인 버튼 상태 (On/Off)

    void Start()
    {
        // 초기 UI 설정
        UpdateButtonUI();

        // 버튼 클릭 이벤트 연결
        loginButton.onClick.AddListener(ToggleLoginPanel);
    }

    void ToggleLoginPanel()
    {
        // 상태 변경
        isLoginOn = !isLoginOn;

        // UI 업데이트
        UpdateButtonUI();

        // 로그인 패널 활성화/비활성화
        loginPanel.SetActive(isLoginOn);

        if(!isLoginOn)
        {
            PlayerPrefs.SetInt("IsLoggedIn", 0); // 로그인 상태 초기화
            PlayerPrefs.SetInt("HighestScore", 0); // 최고 점수 초기화
            PlayerPrefs.Save();
            Debug.Log("Logout it");
        }

        // 설정 패널 비활성화
        settingsPanel.SetActive(!isLoginOn);

        Debug.Log($"Login Panel is now {(isLoginOn ? "Open" : "Closed")}");
    }

    void UpdateButtonUI()
    {
        // 버튼 이미지 변경
        Image buttonImage = loginButton.GetComponent<Image>();
        if (buttonImage != null)
        {
            buttonImage.sprite = isLoginOn ? onSprite : offSprite;
        }

        // 버튼 텍스트 변경
        buttonText.text = isLoginOn ? onText : offText;
    }
}
