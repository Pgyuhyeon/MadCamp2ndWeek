using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.Networking;

public class PlayerMovementAndCollision : MonoBehaviour
{
    [Header("Player Settings")]
    public float moveSpeed = 1.0f; // ?? ??
    public float maxSpeed = 4.0f; // ?? ???
    public float speedIncreaseRate = 0.1f; // ?? ??? ??


    public Color redColor = Color.red;
    public Color blueColor = Color.blue;
    public Color purpleColor = Color.magenta;

    [Header("UI Elements")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI FinalScoreText;
    public TextMeshProUGUI BestScoreText;

    public GameObject gameOverPanel;
    public GameObject gmaeUIPanel;

    public GameObject destroyEffectPrefab;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private float score = 0.0f;
    private bool isGameOver = false;

    private float leftClickTime = -1.0f;
    private float rightClickTime = -1.0f;
    private float simultaneousClickThreshold = 0.08f;



    
    public void StopBall()
    {
        if (rb != null)
        {
            Vector3 startPosition = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.3f, Camera.main.nearClipPlane));
            startPosition.z = 0; // Z?? 0?? ?? (2D ???)
            transform.position = startPosition;

            rb.linearVelocity = Vector2.zero; // ??? ???
            rb.angularVelocity = 0f;   // ?? ?? ???
            Debug.Log("Ball has been stopped.");
        }
        else
        {
            Debug.LogWarning("Rigidbody2D component is null. Ball cannot be stopped.");
        }
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

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = Mathf.FloorToInt(score).ToString();
        }
    }

    void Update()
    {
        if (isGameOver)
        {
            Debug.Log("Update skipped because game is over.");
            return;
        }

        HandleInput();
        IncreaseSpeedOverTime();
        
    }

    private void IncreaseSpeedOverTime()
    {
        if (rb != null && rb.linearVelocity.magnitude < maxSpeed)
        {
            float speedIncrease = speedIncreaseRate * Time.deltaTime;
            rb.linearVelocity += rb.linearVelocity.normalized * speedIncrease;

            if (rb.linearVelocity.magnitude > maxSpeed)
            {
                rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
            }
        }
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



    private bool canCollide = true; // ?? ?? ??
    private float collisionCooldown = 0.5f; // ?? ??? ??

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isGameOver || !canCollide)
        {
            return;
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            SpriteRenderer wallRenderer = collision.gameObject.GetComponent<SpriteRenderer>();
            if (wallRenderer == null)
            {
                Debug.LogWarning("Wall does not have a SpriteRenderer. Ignoring collision.");
                return;
            }

            if (wallRenderer.color != spriteRenderer.color)
            {
                Debug.Log("Wall color mismatch. Game Over triggered.");
                GameOver();
            }
            else
            {
                AddScore(1);

                // ?? ??
                Vector2 collisionNormal = collision.contacts[0].normal;
                float currentSpeed = rb.linearVelocity.magnitude;
                if (currentSpeed < 0.1f)
                {
                    currentSpeed = moveSpeed;
                }
                rb.linearVelocity = -collisionNormal * currentSpeed;

                Debug.Log($"Wall collision resolved. New Velocity: {rb.linearVelocity}");
            }

            // ?? ??? ??
            canCollide = false;
            Invoke(nameof(EnableCollision), collisionCooldown);
        }
    }

    private void EnableCollision()
    {
        canCollide = true; // ?? ?? ??? ??
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
                    CreateDestroyEffect(collision.transform.position, obstacleRenderer.color);

                    Destroy(collision.gameObject); // ??? ??
                    Debug.Log("Obstacle destroyed. Score increased.");
                }
                else
                {
                    CreateDestroyEffect(collision.transform.position, obstacleRenderer.color);

                    Destroy(collision.gameObject); // ??? ??
                    Debug.Log("Obstacle destroyed. Game Over triggered.");
                    GameOver();
                }
            }
        }
    }

    private void CreateDestroyEffect(Vector3 position, Color obstacleColor)
    {
        if (destroyEffectPrefab != null)
        {
            // ??? ??
            GameObject effect = Instantiate(destroyEffectPrefab, position, Quaternion.identity);

            // Particle System? Start Color? ??? ???? ??
            ParticleSystem particleSystem = effect.GetComponent<ParticleSystem>();
            if (particleSystem != null)
            {
                var main = particleSystem.main;
                main.startColor = obstacleColor; // ???? ??? ??
            }

            // ?? ?? ? ??
            Destroy(effect, 0.5f);
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


        UpdateHighScore();//???? ??

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


    private void UpdateHighScore()
    {
        // ?? ??? ?? ?? ????
        int highScore = PlayerPrefs.GetInt("HighestScore", 0);

        // ?? ??? ?? ???? ?? ?? ??
        if (Mathf.FloorToInt(score) > highScore)
        {
            highScore = Mathf.FloorToInt(score);
            PlayerPrefs.SetInt("HighestScore", highScore);
            PlayerPrefs.Save();
            if (PlayerPrefs.GetInt("IsLoggedIn", 0) == 1)
            {
                Debug.Log($"New Record={highScore}");
                StartCoroutine(UpdateHighScoreOnServer(highScore));
            }
            Debug.Log($"New High Score! HighScore={highScore}");
        }
        else
        {
            Debug.Log($"Current Score={Mathf.FloorToInt(score)}. HighScore remains={highScore}");
        }

        BestScoreText.text = highScore.ToString();
    }

    private IEnumerator UpdateHighScoreOnServer(int newHighScore)
{
    string updateScoreUrl = "http://43.202.48.66:3000/update-score";
    string jsonData = $"{{\"username\":\"{PlayerPrefs.GetString("Username")}\",\"highest_score\":{newHighScore}}}";

    UnityWebRequest request = new UnityWebRequest(updateScoreUrl, "POST");
    byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
    request.uploadHandler = new UploadHandlerRaw(bodyRaw);
    request.downloadHandler = new DownloadHandlerBuffer();
    request.SetRequestHeader("Content-Type", "application/json");

    yield return request.SendWebRequest();

    if (request.result == UnityWebRequest.Result.Success)
    {
        Debug.Log("High score updated successfully on the server!");
    }
    else
    {
        Debug.LogError("Failed to update high score: " + request.error);
    }
}
}
