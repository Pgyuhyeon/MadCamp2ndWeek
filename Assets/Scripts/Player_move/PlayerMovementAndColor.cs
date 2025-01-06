using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerMovementAndCollision : MonoBehaviour
{
    public float moveSpeed = 5.0f; // 플레이어 이동 속도
    public Color redColor = Color.red; // 오른쪽 클릭 시 색상
    public Color blueColor = Color.blue; // 왼쪽 클릭 시 색상
    public Color purpleColor = Color.magenta; // 동시에 클릭 시 색상
    public TextMeshProUGUI scoreText; // 점수 표시용 UI 텍스트
    public GameObject gameOverPanel; // 게임 오버 창

    private SpriteRenderer spriteRenderer;
    private float score = 0.0f; // 현재 점수
    private bool isGameOver = false; // 게임 종료 상태

    void Start()
    {
        // SpriteRenderer 가져오기
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer component missing on the player!");
            return;
        }

        // 점수 초기화
        if (scoreText != null)
        {
            scoreText.text = "Score: 0";
        }

        // 게임 오버 창 숨기기
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    void Update()
    {
        if (isGameOver)
            return;

        // 플레이어 이동
        HandleMovement();

        // 색상 변경
        HandleColorChange();

        // 점수 증가
        UpdateScore();
    }

    private void HandleMovement()
    {
        // 현재 방향으로 계속 이동
        transform.position += new Vector3(moveSpeed * Time.deltaTime, 0, 0);
    }

    private void HandleColorChange()
    {
        // 터치 또는 마우스 클릭 확인
        bool isLeftClicked = false;
        bool isRightClicked = false;

        if (Input.touchCount > 0) // 터치 입력이 있는 경우
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Stationary)
                {
                    if (touch.position.x < Screen.width / 2)
                        isLeftClicked = true;
                    else
                        isRightClicked = true;
                }
            }
        }
        else if (Input.GetMouseButton(0)) // 마우스 클릭 확인
        {
            if (Input.mousePosition.x < Screen.width / 2)
                isLeftClicked = true;
            else
                isRightClicked = true;
        }

        // 색상 변경 로직
        if (isLeftClicked && isRightClicked)
        {
            spriteRenderer.color = purpleColor; // 동시에 클릭 시 보라색
        }
        else if (isLeftClicked)
        {
            spriteRenderer.color = blueColor; // 왼쪽 클릭 시 파란색
        }
        else if (isRightClicked)
        {
            spriteRenderer.color = redColor; // 오른쪽 클릭 시 빨간색
        }
    }

    private void UpdateScore()
    {
        score += Time.deltaTime; // 시간에 따라 점수 증가
        if (scoreText != null)
        {
            scoreText.text = "Score: " + Mathf.FloorToInt(score).ToString();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isGameOver)
            return;

        // 충돌한 객체의 SpriteRenderer 가져오기
        SpriteRenderer otherRenderer = collision.gameObject.GetComponent<SpriteRenderer>();
        if (otherRenderer != null)
        {
            if (otherRenderer.color == spriteRenderer.color)
            {
                // 색상이 같으면 방향 반전
                moveSpeed *= -1; // 이동 방향 반전
            }
            else
            {
                // 색상이 다르면 게임 종료
                GameOver();
            }
        }
    }



    private void GameOver()
    {
        isGameOver = true;

        // UI에 Game Over 창 활성화
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            Text finalScoreText = gameOverPanel.transform.Find("FinalScoreText").GetComponent<Text>();
            if (finalScoreText != null)
            {
                finalScoreText.text = "Score: " + Mathf.FloorToInt(score).ToString();
            }
        }

        // 플레이어 동작 멈추기
        moveSpeed = 0.0f;
    }

    // 게임 재시작 (버튼에서 호출)
    public void RestartGame()
    {
        // 현재 씬 다시 로드
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}
