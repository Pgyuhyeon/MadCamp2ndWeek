using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject startPanel;    // StartPanel
    public GameObject mainMenuPanel; // MainMenuPanel
    public GameManager gameManager; // GameManager 참조

    void Start()
    {
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
        SceneManager.LoadScene("LeaderboardScene");
    }

    public void OpenSettings()
    {
        Debug.Log("Opening Settings");
        SceneManager.LoadScene("SettingsScene");
    }

    public void QuitGame()
    {
        Debug.Log("Game Quit!");
        Application.Quit();
    }
}
