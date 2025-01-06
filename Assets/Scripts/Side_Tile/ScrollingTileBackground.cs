using UnityEngine;
using System.Collections.Generic;

public class ScrollingLinesBackground : MonoBehaviour
{
    public GameObject linePrefab; // 선 프리팹
    public float lineWidth = 0.2f; // 선의 너비
    public float lineHeight = 1.0f; // 선의 높이
    public float moveSpeed = 2.0f; // 선의 이동 속도
    public Color[] lineColors; // 선 색상 배열

    private List<GameObject> activeLines; // 현재 활성화된 선 리스트
    private float screenHeight; // 화면 높이
    private float spawnOffset = 2.0f; // 화면 위로 미리 생성할 오프셋

    private void Start()
    {
        if (lineColors.Length == 0)
        {
            Debug.LogError("No colors assigned to the lineColors array!");
            return;
        }

        if (linePrefab == null)
        {
            Debug.LogError("linePrefab is not assigned! Please assign it in the Inspector.");
            return;
        }

        // 화면 높이 계산
        screenHeight = Camera.main.orthographicSize * 2;

        // 활성화된 선 리스트 초기화
        activeLines = new List<GameObject>();

        // 초기 선 생성
        float startYPosition = -Camera.main.orthographicSize - spawnOffset; // 화면 위로 미리 생성
        while (startYPosition < Camera.main.orthographicSize + lineHeight)
        {
            SpawnLine(-1, startYPosition); // 왼쪽 선 생성
            SpawnLine(1, startYPosition);  // 오른쪽 선 생성
            startYPosition += lineHeight;
        }
    }

    private void Update()
    {
        // 모든 선 이동
        foreach (var line in activeLines)
        {
            line.transform.position += Vector3.down * moveSpeed * Time.deltaTime;
        }

        // 가장 아래 선이 화면 아래로 벗어나면, 새 선을 위로 생성
        if (activeLines[0].transform.position.y < -Camera.main.orthographicSize - lineHeight)
        {
            // 아래로 벗어난 선 제거
            Destroy(activeLines[0]); // 왼쪽 선 제거
            Destroy(activeLines[1]); // 오른쪽 선 제거
            activeLines.RemoveAt(0); // 리스트에서 제거
            activeLines.RemoveAt(0);

            // 새 선 생성 (화면 위로 미리 생성)
            float newYPosition = activeLines[activeLines.Count - 1].transform.position.y + lineHeight;
            SpawnLine(-1, newYPosition); // 왼쪽 선 생성
            SpawnLine(1, newYPosition);  // 오른쪽 선 생성
        }
    }

    private void SpawnLine(int side, float yPosition)
    {
        float screenWidth = Camera.main.orthographicSize * Camera.main.aspect * 2;
        float xPosition = (side == -1) ? -screenWidth / 2 + lineWidth / 2 : screenWidth / 2 - lineWidth / 2;

        GameObject line = Instantiate(linePrefab, new Vector3(xPosition, yPosition, 0), Quaternion.identity);
        line.transform.localScale = new Vector3(lineWidth, lineHeight, 1);
        SetRandomColor(line);

        activeLines.Add(line); // 리스트에 추가
    }

    private void SetRandomColor(GameObject line)
    {
        SpriteRenderer renderer = line.GetComponent<SpriteRenderer>();
        if (renderer == null)
        {
            Debug.LogError("SpriteRenderer is missing on the linePrefab!");
            return;
        }

        Color randomColor = lineColors[Random.Range(0, lineColors.Length)];
        randomColor.a = 1.0f;
        renderer.color = randomColor;
    }
}
