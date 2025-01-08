using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;

public class MainMenu : MonoBehaviour
{
    public GameObject SettingPanel;
    public GameObject startPanel;    // StartPanel
    public GameObject mainMenuPanel; // MainMenuPanel
    public GameManager gameManager; // GameManager 참조
    public GameObject LoginPanel;
    public GameObject LeaderBoard;
    public GameObject gameOverPanel;
    public GameObject player;
    public AudioSource backgroundMusic; // 메인 메뉴 배경음악

    [SerializeField] private string gameid = "5769898";
    private bool testMode = true;

    private const string SoundPrefKey = "IsSoundOn";
    private const string VibrationPrefKey = "IsVibrationOn";

    void Start()
    {
        Debug.Log("Show Ads please...");
        Initialize();

        // 소리와 진동 상태 초기화
        InitializeSoundAndVibration();
    }

    void OnEnable()
    {
        // 플레이어 비활성화 (MainMenu에서는 숨기기)
        if (player != null)
        {
            player.SetActive(false);
            Debug.Log("Player is now hidden in MainMenu.");
        }

        // 로그인 후 사용자 정보 표시
        if (PlayerPrefs.GetInt("IsLoggedIn", 0) == 1)
        {
            string username = PlayerPrefs.GetString("Username", "Guest");
            int highestScore = PlayerPrefs.GetInt("HighestScore", 0);
            Debug.Log($"Welcome back, {username}! Your highest score: {highestScore}");
        }
        else
        {
            Debug.Log("User is not logged in.");
        }
    }

    public void Initialize()
    {
        if (LoginPanel != null)
            LoginPanel.SetActive(false);

        if (player != null)
            player.SetActive(false);

        if (LeaderBoard != null)
            LeaderBoard.SetActive(false);

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (SettingPanel != null)
            SettingPanel.SetActive(false);

        if (startPanel != null)
            startPanel.SetActive(false); // 스타트 패널 비활성화

        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(true); // 메인 메뉴 패널 활성화
    }

    public void InitializeSoundAndVibration()
    {
        // 소리 상태 초기화
        bool isSoundOn = PlayerPrefs.GetInt(SoundPrefKey, 1) == 1; // 기본값: 켜짐
        if (backgroundMusic != null)
        {
            backgroundMusic.mute = !isSoundOn;
        }
        Debug.Log($"Sound initialized to: {(isSoundOn ? "On" : "Off")}");

        // 진동 상태 초기화
        bool isVibrationOn = PlayerPrefs.GetInt(VibrationPrefKey, 1) == 1; // 기본값: 켜짐
        Debug.Log($"Vibration initialized to: {(isVibrationOn ? "On" : "Off")}");
    }

    public void ToggleSound(bool isOn)
    {
        // 소리 상태 저장
        PlayerPrefs.SetInt(SoundPrefKey, isOn ? 1 : 0);
        PlayerPrefs.Save();

        // 배경음 상태 업데이트
        if (backgroundMusic != null)
        {
            backgroundMusic.mute = !isOn;
        }

        Debug.Log($"Sound toggled to: {(isOn ? "On" : "Off")}");
    }

    public void ToggleVibration(bool isOn)
    {
        // 진동 상태 저장
        PlayerPrefs.SetInt(VibrationPrefKey, isOn ? 1 : 0);
        PlayerPrefs.Save();

        Debug.Log($"Vibration toggled to: {(isOn ? "On" : "Off")}");
    }

    public void StartGame()
    {
        Debug.Log("StartGame called");

        // 메인 메뉴 패널 비활성화
        if (mainMenuPanel != null)
        {
            mainMenuPanel.SetActive(false);
            Debug.Log("MainMenuPanel deactivated");
        }
        else
        {
            Debug.LogError("MainMenuPanel is not assigned!");
        }

        // 스타트 패널 활성화
        if (startPanel != null)
        {
            startPanel.SetActive(true);
            Debug.Log("StartPanel activated");
        }
        else
        {
            Debug.LogError("StartPanel is not assigned!");
        }

        // GameManager에서 게임 시작 처리
        if (gameManager != null)
        {
            gameManager.StartGame(); // 게임 상태 전환
        }
    }

    public void OpenLeaderboard()
    {
        Debug.Log("Opening Leaderboard");
        mainMenuPanel.SetActive(false);
        LeaderBoard.SetActive(true);
    }

    public void OpenSettings()
    {
        Debug.Log("Opening Settings");
        mainMenuPanel.SetActive(false);
        SettingPanel.SetActive(true);
    }

    public void QuitGame()
    {
        Debug.Log("Game Quit!");
        Application.Quit();
    }
}
