using UnityEngine;

public class FallingObstacles : MonoBehaviour
{
    public GameObject obstaclePrefab; // 장애물 프리팹
    public float obstacleSizeMin = 0.5f; // 장애물 최소 크기
    public float obstacleSizeMax = 0.8f; // 장애물 최대 크기
    public float spawnInterval = 1.0f; // 생성 간격
    public float fallSpeed = 2.0f; // 장애물 속도
    public Color[] obstacleColors; // 장애물 색상 배열 (랜덤 선택)

    private float screenWidthHalf; // 화면 너비의 절반
    private float timeSinceLastSpawn = 0.0f;

    void Start()
    {
        // 카메라 화면 너비의 절반 계산
        screenWidthHalf = Camera.main.orthographicSize * Camera.main.aspect - 0.5f;

        if (obstacleColors.Length == 0)
        {
            Debug.LogError("Obstacle colors array is empty!");
        }
    }

    void Update()
    {
        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= spawnInterval)
        {
            SpawnObstacle();
            timeSinceLastSpawn = 0.0f;
        }
    }

    private void SpawnObstacle()
    {
        if (obstaclePrefab == null)
        {
            Debug.LogError("Obstacle prefab is not assigned!");
            return;
        }

        // 장애물의 X 위치는 화면 너비 범위 내에서 랜덤
        float randomX = Random.Range(-screenWidthHalf, screenWidthHalf);

        // 장애물의 Y 위치는 화면 위
        float spawnY = Camera.main.orthographicSize + 1.0f;

        // 장애물 생성
        GameObject obstacle = Instantiate(obstaclePrefab, new Vector3(randomX, spawnY, 0), Quaternion.identity);

        // 랜덤 크기 설정
        float randomSize = Random.Range(obstacleSizeMin, obstacleSizeMax);
        obstacle.transform.localScale = new Vector3(randomSize, randomSize, 1);

        // 랜덤 색상 설정
        SpriteRenderer renderer = obstacle.GetComponent<SpriteRenderer>();
        if (renderer != null && obstacleColors.Length > 0)
        {
            Color randomColor = obstacleColors[Random.Range(0, obstacleColors.Length)];
            randomColor.a = 1.0f; // 알파 값(투명도)을 1로 설정
            renderer.color = randomColor;


        }

        // 장애물 이동 속도 설정
        Rigidbody2D rb = obstacle.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0; // 중력 효과 제거
        rb.linearVelocity = Vector2.down * fallSpeed; // 아래로 이동

        // 화면 아래로 벗어나면 해제
        obstacle.AddComponent<DestroyWhenOffScreen>();
    }
}
