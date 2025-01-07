using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class RankingManager : MonoBehaviour
{
    [Header("Server Settings")]
    public string serverUrl = "http://43.202.48.66:3000/ranking"; // 서버 API URL

    [Header("UI References")]
    public Transform content; // ScrollView의 Content
    public GameObject rankingItemPrefab; // 프리팹 참조

    private void OnEnable()
    {
        Debug.Log("Start getting ranking data...");
        GetRanking(); // 패널 활성화 시 랭킹 데이터 가져오기
    }

    /// <summary>
    /// 서버에서 랭킹 데이터를 가져오는 함수.
    /// </summary>
    public void GetRanking()
    {
        StartCoroutine(GetRankingCoroutine());
    }

    /// <summary>
    /// 서버 API 호출 및 데이터 처리 코루틴.
    /// </summary>
    private IEnumerator GetRankingCoroutine()
    {
        UnityWebRequest request = UnityWebRequest.Get(serverUrl);

        // 요청 보내기
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Failed to fetch ranking data: " + request.error);
            yield break;
        }

        // JSON 응답 처리
        string jsonResult = request.downloadHandler.text;
        Debug.Log("Ranking data received: " + jsonResult);

        List<User> users;
        try
        {
            users = JsonUtility.FromJson<UserList>($"{{\"users\":{jsonResult}}}").users;
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to parse ranking data: " + e.Message);
            yield break;
        }

        // 기존 리스트 초기화
        ClearExistingItems();

        // 데이터를 동적으로 추가
        PopulateRankingList(users);
    }

    /// <summary>
    /// 기존 ScrollView 아이템 초기화.
    /// </summary>
    private void ClearExistingItems()
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
    }

    /// <summary>
    /// 사용자 데이터를 기반으로 ScrollView에 아이템 추가.
    /// </summary>
    /// <param name="users">사용자 데이터 리스트</param>
    private void PopulateRankingList(List<User> users)
    {
        foreach (var user in users)
        {
            // 프리팹 인스턴스 생성
            GameObject newItem = Instantiate(rankingItemPrefab, content);

            // TextMeshProUGUI 배열로 username과 highest_score 설정
            TextMeshProUGUI[] textComponents = newItem.GetComponentsInChildren<TextMeshProUGUI>();

            if (textComponents.Length >= 2)
            {
                // username은 왼쪽, highest_score는 오른쪽에 표시
                textComponents[0].text = user.username; // username 표시
                textComponents[1].text = user.highest_score.ToString(); // highest_score 표시
            }
            else
            {
                Debug.LogError("Prefab must have at least two TextMeshProUGUI components.");
                Destroy(newItem);
            }
        }
    }

    /// <summary>
    /// 사용자 데이터 구조체 정의.
    /// </summary>
    [System.Serializable]
    public class User
    {
        public int id;
        public string username;
        public int highest_score;
    }

    /// <summary>
    /// JSON 데이터 파싱용 리스트 래퍼 클래스.
    /// </summary>
    [System.Serializable]
    public class UserList
    {
        public List<User> users;
    }
}
