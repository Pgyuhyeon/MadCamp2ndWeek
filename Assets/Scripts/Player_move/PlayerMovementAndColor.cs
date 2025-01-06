using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerMovementAndCollision : MonoBehaviour
{
    [Header("Player Settings")]
    public float moveSpeed = 5.0f;
    public Color redColor = Color.red;
    public Color blueColor = Color.blue;
    public Color purpleColor = Color.magenta;

    [Header("UI Elements")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI FinalScoreText;
    public GameObject gameOverPanel;
    public GameObject gmaeUIPanel;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private float score = 0.0f;
    private bool isGameOver = false;

    private float leftClickTime = -1.0f;
    private float rightClickTime = -1.0f;
    private float simultaneousClickThreshold = 0.08f;

    void Start()
    {
        InitializePlayer();
    }

    public void InitializePlayer()
    {
        Debug.Log("InitializePlayer method called.");

        // Initialize components
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        if (spriteRenderer == null || rb == null)
        {
            Debug.LogError("Missing essential components! SpriteRenderer or Rigidbody2D is null.");
            return;
        }

        // Reset player position and velocity
        transform.position = Vector3.zero;
        rb.linearVelocity = new Vector2(moveSpeed, 0);
        rb.angularVelocity = 0f;
        Debug.Log($"Player initialized: Position={transform.position}, Velocity={rb.linearVelocity}");

        // Reset score
        score = 0.0f;
        UpdateScoreText();

        // Hide GameOver panel
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
            Debug.Log("GameOver panel deactivated.");
        }

        isGameOver = false;
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

        // Handle keyboard input
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            isLeftPressed = true;
            leftClickTime = Time.time;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            isRightPressed = true;
            rightClickTime = Time.time;
        }

        // Handle touch input
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
                    }
                    else if (touch.position.x >= Screen.width / 2)
                    {
                        isRightPressed = true;
                        rightClickTime = Time.time;
                    }
                }
            }
        }

        // Handle simultaneous input
        if (Mathf.Abs(leftClickTime - rightClickTime) <= simultaneousClickThreshold &&
            leftClickTime > 0 && rightClickTime > 0)
        {
            ChangePlayerColor(purpleColor);
        }
        else if (isLeftPressed)
        {
            ChangePlayerColor(blueColor);
        }
        else if (isRightPressed)
        {
            ChangePlayerColor(redColor);
        }
    }

    private void ChangePlayerColor(Color newColor)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = newColor;
            Debug.Log($"Player color changed to {newColor}");
        }
    }

    private void UpdateScore()
    {
        score += Time.deltaTime;
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = Mathf.FloorToInt(score).ToString();
            Debug.Log($"Score updated: {scoreText.text}");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isGameOver)
        {
            Debug.Log("Collision ignored because game is over.");
            return;
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            SpriteRenderer wallRenderer = collision.gameObject.GetComponent<SpriteRenderer>();
            if (wallRenderer != null)
            {
                if (wallRenderer.color != spriteRenderer.color)
                {
                    Debug.Log("Wall color does not match. Game Over triggered.");
                    GameOver();
                }
                else
                {
                    // ??? ????? ?? ???
                    Vector2 pushDirection = new Vector2(-Mathf.Sign(rb.linearVelocity.x), 0); // ?? ??
                    transform.position += (Vector3)pushDirection * 0.1f; // ??? ?? ???

                    // ?? ??
                    float currentSpeed = Mathf.Abs(rb.linearVelocity.x);
                    if (currentSpeed < 0.1f)
                    {
                        currentSpeed = moveSpeed; // ?? ?? ??
                    }
                    rb.linearVelocity = new Vector2(-currentSpeed, rb.linearVelocity.y);

                    Debug.Log($"Wall color matches. Player direction reversed. New Velocity: {rb.linearVelocity}");
                }
            }
            else
            {
                Debug.LogWarning("Wall does not have a SpriteRenderer.");
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isGameOver)
        {
            Debug.Log("Trigger ignored because game is over.");
            return;
        }

        if (collision.CompareTag("Obstacle"))
        {
            SpriteRenderer obstacleRenderer = collision.GetComponent<SpriteRenderer>();
            if (obstacleRenderer != null)
            {
                if (obstacleRenderer.color == spriteRenderer.color)
                {
                    AddScore(1);
                    TriggerCollisionEvent(collision.gameObject);
                    Destroy(collision.gameObject); // ??? ??
                    Debug.Log("Obstacle destroyed. Score increased.");
                }
                else
                {
                    Destroy(collision.gameObject); // ??? ??
                    Debug.Log("Obstacle destroyed. Game Over triggered.");
                    GameOver();
                }
            }
        }
    }






    private void HandleObstacleCollision(GameObject obstacle)
    {
        SpriteRenderer obstacleRenderer = obstacle.GetComponent<SpriteRenderer>();
        if (obstacleRenderer != null)
        {
            if (obstacleRenderer.color == spriteRenderer.color)
            {
                AddScore(1);
                TriggerCollisionEvent(obstacle);
                Destroy(obstacle);
            }
            else
            {
                GameOver();
            }
        }
    }

    private void HandleOtherCollision(GameObject otherObject)
    {
        SpriteRenderer otherRenderer = otherObject.GetComponent<SpriteRenderer>();
        if (otherRenderer != null)
        {
            if (otherRenderer.color != spriteRenderer.color)
            {
                GameOver();
            }
        }
        else
        {
            Debug.LogWarning($"Collision with object '{otherObject.name}' without a SpriteRenderer.");
        }
    }

    private void TriggerCollisionEvent(GameObject obj)
    {
        // ??? ?? ??
        ParticleSystem particle = obj.GetComponent<ParticleSystem>();
        if (particle != null)
        {
            particle.transform.parent = null;
            particle.Play();
            Destroy(particle.gameObject, particle.main.duration);
        }

        
    }


    private void AddScore(int points)
    {
        score += points;
        UpdateScoreText();
    }

    private void GameOver()
    {
        isGameOver = true;

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            if (FinalScoreText != null)
            {
                FinalScoreText.text = Mathf.FloorToInt(score).ToString();
            }
        }

        if (gmaeUIPanel != null)
        {
            gmaeUIPanel.SetActive(false);
        }

        Debug.Log("Game Over triggered.");
    }

    public void RestartGame()
    {
        InitializePlayer();

        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            gameManager.StartGame();
        }

        Debug.Log("Game restarted.");
    }
}
