using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Advertisements;
using UnityEditor.Advertisements;

public class PlayerMovementAndCollision : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public Color redColor = Color.red;
    public Color blueColor = Color.blue;
    public Color purpleColor = Color.magenta;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI FinalScoreText;
    public GameObject gameOverPanel;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private float score = 0.0f;
    private bool isGameOver = false;

    private float leftClickTime = -1.0f;
    private float rightClickTime = -1.0f;
    private float simultaneousClickThreshold = 0.08f;

    void Start()
    {
        Debug.Log("Start method called in PlayerMovementAndCollision");

        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        if (spriteRenderer == null || rb == null)
        {
            Debug.LogError("Missing essential components! SpriteRenderer or Rigidbody2D is null.");
            return;
        }

        rb.linearVelocity = new Vector2(moveSpeed, 0);
        Debug.Log($"Initial velocity set to {rb.linearVelocity}");

        if (scoreText != null)
        {
            scoreText.text = "0";
            Debug.Log("Score text initialized to 0.");
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
            Debug.Log("GameOver panel set to inactive.");
        }
    }


    public void InitializePlayer()
    {
        Debug.Log("InitializePlayer method called.");

        // ?? ? ?? ???
        transform.position = Vector3.zero; // ?? ?? ??
        rb.linearVelocity = new Vector2(moveSpeed, 0); // ?? ?? ??
        rb.angularVelocity = 0f; // ?? ?? ???
        Debug.Log($"Player position and velocity initialized: Position={transform.position}, Velocity={rb.linearVelocity}");

        // ?? ???
        score = 0.0f;
        if (scoreText != null)
        {
            scoreText.text = "0";
            scoreText.gameObject.SetActive(true);
            Debug.Log("Score text reset to 0 and made visible.");
        }

        // ?? ?? ?? ???
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
            Debug.Log("GameOver panel deactivated.");
        }

        isGameOver = false; // ?? ?? ?? ???
        Debug.Log("Player state reset. isGameOver set to false.");
    }



    void Update()
    {
        if (isGameOver)
        {
            Debug.Log("Update skipped because game is over.");
            return;
        }

        HandleInput();
        UpdateScore();
    }

    private void HandleInput()
    {
        bool isLeftPressed = false;
        bool isRightPressed = false;

        // ??? ?? ??
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            isLeftPressed = true;
            leftClickTime = Time.time;
            Debug.Log($"Left arrow pressed. Time: {leftClickTime}");
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            isRightPressed = true;
            rightClickTime = Time.time;
            Debug.Log($"Right arrow pressed. Time: {rightClickTime}");
        }

        // ??? ?? ?? ??
        if (Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Stationary)
                {
                    if (touch.position.x < Screen.width / 2)
                    {
                        isLeftPressed = true;
                        leftClickTime = Time.time;
                        Debug.Log($"Left screen touched. Time: {leftClickTime}");
                    }
                    else if (touch.position.x >= Screen.width / 2)
                    {
                        isRightPressed = true;
                        rightClickTime = Time.time;
                        Debug.Log($"Right screen touched. Time: {rightClickTime}");
                    }
                }
            }
        }

        // ? ?? ??
        if (Mathf.Abs(leftClickTime - rightClickTime) <= simultaneousClickThreshold &&
            leftClickTime > 0 && rightClickTime > 0)
        {
            spriteRenderer.color = purpleColor;
            Debug.Log("Simultaneous input detected. Player color set to purple.");
        }
        else if (isLeftPressed)
        {
            spriteRenderer.color = blueColor;
            Debug.Log("Left input detected. Player color set to blue.");
        }
        else if (isRightPressed)
        {
            spriteRenderer.color = redColor;
            Debug.Log("Right input detected. Player color set to red.");
        }
    }


    private void UpdateScore()
    {
        score += Time.deltaTime;
        if (scoreText != null)
        {
            scoreText.text = Mathf.FloorToInt(score).ToString();
            Debug.Log($"Score updated: {scoreText.text}");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"OnCollisionEnter2D called with {collision.gameObject.name}");

        if (isGameOver)
        {
            Debug.Log("Collision ignored because game is over.");
            return;
        }

        SpriteRenderer otherRenderer = collision.gameObject.GetComponent<SpriteRenderer>();
        if (otherRenderer != null)
        {
            Debug.Log($"Collision detected. Player color: {spriteRenderer.color}, Other color: {otherRenderer.color}");

            if (otherRenderer.color != spriteRenderer.color)
            {
                Debug.Log("Game over condition met. Triggering GameOver.");
                GameOver();
            }
            else
            {
                Debug.Log("Same color collision. No GameOver.");
            }
        }
        else
        {
            Debug.LogWarning("Collision object does not have a SpriteRenderer.");
        }
    }

    private void GameOver()
    {
        Debug.Log("GameOver method called in PlayerMovementAndCollision.");
        Advertisement.Show("test_ad");
        isGameOver = true;

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            Debug.Log("Player velocity set to zero.");
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            Debug.Log("GameOver panel activated.");
            if (FinalScoreText != null)
            {
                FinalScoreText.text = Mathf.FloorToInt(score).ToString();
                Debug.Log($"Final score displayed: {FinalScoreText.text}");
            }
        }

        if (scoreText != null)
        {
            scoreText.gameObject.SetActive(false);
            Debug.Log("Score text hidden.");
        }

        // Global GameManager logic
        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            gameManager.GameOver();
            Debug.Log("GameManager.GameOver called.");
        }
    }


    public void RestartGame()
    {
        Debug.Log("RestartGame method called.");

        // ???? ?? ? ?? ???
        transform.position = Vector3.zero;
        rb.linearVelocity = new Vector2(moveSpeed, 0);
        Debug.Log($"Player reset to position {transform.position} with velocity {rb.linearVelocity}");

        // ?? ???
        score = 0.0f;
        if (scoreText != null)
        {
            scoreText.text = "0";
            scoreText.gameObject.SetActive(true);
            Debug.Log("Score text reset to 0 and made visible.");
        }

        // ?? ?? ?? ???
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
            Debug.Log("GameOver panel deactivated.");
        }

        // GameManager? StartGame ??
        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            gameManager.StartGame();
            Debug.Log("StartGame method called from RestartGame.");
        }

        // ?? ?? ?? ???
        isGameOver = false;
        Debug.Log("Game state reset. isGameOver set to false.");
    }



}
