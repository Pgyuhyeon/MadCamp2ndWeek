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

    private const string LoginPrefKey = "IsLoggedIn"; // PlayerPrefs 키
    private bool isLoginOn = false;        // 로그인 버튼 상태 (On/Off)

    void Start()
    {
        // 저장된 로그인 상태 불러오기
        isLoginOn = PlayerPrefs.GetInt(LoginPrefKey, 0) == 1;

        // 초기 UI 설정
        UpdateButtonUI();

        // 로그인 패널 초기 상태 설정
        //loginPanel.SetActive(isLoginOn);
        //settingsPanel.SetActive(!isLoginOn);

        // 버튼 클릭 이벤트 연결
        loginButton.onClick.AddListener(ToggleLoginPanel);
    }

    void ToggleLoginPanel()
    {
        // 상태 변경
        isLoginOn = !isLoginOn;

        // 상태 저장
        PlayerPrefs.SetInt(LoginPrefKey, isLoginOn ? 1 : 0);
        PlayerPrefs.Save();

        // UI 업데이트
        UpdateButtonUI();

        // 로그인 패널 활성화/비활성화
        loginPanel.SetActive(isLoginOn);

        if (!isLoginOn)
        {
            PlayerPrefs.SetInt("HighestScore", 0); // 최고 점수 초기화
            PlayerPrefs.Save();
            Debug.Log("Logged out and scores reset.");
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
