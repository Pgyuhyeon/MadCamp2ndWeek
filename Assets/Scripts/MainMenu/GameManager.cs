using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject mainMenuPanel;  // 메인 메뉴 패널
    public GameObject startPanel;     // 스타트 패널
    public GameObject gameUIPanel;    // 게임 UI 패널
    public GameObject gameOverPanel;  // 게임 오버 패널
    public GameObject player;         // 플레이어 오브젝트

    private bool gameStarted = false; // 게임 시작 상태
    private bool waitingForStart = false; // 스타트 패널 터치 대기 상태

    public void StartGame()
    {
        Debug.Log("Game Start button clicked");

        // 메인 메뉴 패널 비활성화, 스타트 패널 활성화
        SetPanelState(false, true, false, false);


        waitingForStart = true; // 스타트 패널 터치 대기
        Debug.Log($"waitingForStart set to {waitingForStart}");
    }

    private void StartGameplay()
    {
        Debug.Log("Gameplay started");

        // 게임 UI 활성화, 플레이어 활성화
        SetPanelState(false, false, true, true);

        // 플레이어 속도 초기화
        if (player != null)
        {
            PlayerMovementAndCollision playerScript = player.GetComponent<PlayerMovementAndCollision>();
            if (playerScript != null)
            {
                playerScript.InitializePlayer(); // 플레이어 초기화 메서드 호출
                Debug.Log("Player initialized from GameManager.");
            }
            else
            {
                Debug.LogError("Player script (PlayerMovementAndCollision) not found on player object.");
            }
        }


        gameStarted = true; // 게임 시작 상태 전환
        waitingForStart = false; // 대기 상태 해제
    }


    private void ResetGame()
    {
        Debug.Log("Game Reset");

        if (player != null)
        {
            player.SetActive(false);
            player.transform.position = Vector3.zero;
            player.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
            player.SetActive(true);
        }

        gameStarted = false;
        waitingForStart = false;
    }

    private void SetPanelState(bool showMainMenu, bool showStartPanel, bool showGameUI, bool activatePlayer)
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(showMainMenu);
        if (startPanel != null) startPanel.SetActive(showStartPanel);
        if (gameUIPanel != null) gameUIPanel.SetActive(showGameUI);
        if (gameOverPanel != null) gameOverPanel.SetActive(false); // GameOverPanel은 항상 비활성화
        if (player != null) player.SetActive(activatePlayer);

        Debug.Log($"SetPanelState called: MainMenu={showMainMenu}, StartPanel={showStartPanel}, GameUI={showGameUI}, PlayerActive={activatePlayer}");
    }

    void Update()
    {
        Debug.Log($"Update called. waitingForStart={waitingForStart}, startPanel.activeSelf={startPanel.activeSelf}");

        if (waitingForStart && startPanel.activeSelf)
        {
            if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
            {
                Debug.Log("Touch detected, calling StartGameplay()");
                StartGameplay();
            }
        }
    }

    public void GameOver()
    {
        Debug.Log("Game Over triggered in GameManager");

        

        // Game UI 패널 유지 (필요 시 비활성화 가능)
        if (gameUIPanel != null)
        {
            gameUIPanel.SetActive(false);
            Debug.Log("GameUIPanel deactivated.");
        }

        gameStarted = false;
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
        Debug.Log("Game Quit");
        Application.Quit();
    }
}
