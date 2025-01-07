using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;

public class MainMenu : MonoBehaviour
{
    public GameObject startPanel;    // StartPanel
    public GameObject mainMenuPanel; // MainMenuPanel
    public GameManager gameManager; // GameManager 참조
    public GameObject LoginPanel;
    public GameObject LeaderBoard;
    [SerializeField] private string gameid = "5769898";
    private bool testMode = true;

    void Start()
    {
        //Advertisement.Show("test_ad");
        Debug.Log("Show Ads please...");
        Initialize();
    }

    void OnEnable()
    {
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

        if (LeaderBoard != null)
            LeaderBoard.SetActive(false);

        if (startPanel != null)
            startPanel.SetActive(false); // 스타트 패널 비활성화

        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(true); // 메인 메뉴 패널 활성화
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
        LoginPanel.SetActive(true);
    }

    public void QuitGame()
    {
        Debug.Log("Game Quit!");
        Application.Quit();
    }
}
