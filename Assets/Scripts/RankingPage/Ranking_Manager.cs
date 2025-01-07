using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro; // TextMeshPro 네임스페이스 추가

public class RankingManager : MonoBehaviour
{
    public string serverUrl = "http://43.202.48.66:3000/ranking"; // 서버 API URL
    public Transform content; // ScrollView의 Content
    public GameObject rankingItemPrefab; // 프리팹 참조

    private void OnEnable()
    {
        Debug.Log("Strat get ranking");
        GetRanking(); // 패널 활성화 시 랭킹 데이터 가져오기
    }

    // 랭킹 데이터 가져오기 함수
    public void GetRanking()
    {
        StartCoroutine(GetRankingCoroutine());
    }

    private IEnumerator GetRankingCoroutine()
    {
        UnityWebRequest request = UnityWebRequest.Get(serverUrl);

        // 요청 보내기
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            // JSON 응답 처리
            string jsonResult = request.downloadHandler.text;

            // 사용자 리스트로 변환
            List<User> users = JsonUtility.FromJson<UserList>($"{{\"users\":{jsonResult}}}").users;

            // 기존 리스트 초기화
            foreach (Transform child in content)
            {
                Destroy(child.gameObject);
            }

            // 데이터를 동적으로 추가
            foreach (var user in users)
            {
                GameObject newItem = Instantiate(rankingItemPrefab, content);
                TextMeshProUGUI itemText = newItem.GetComponent<TextMeshProUGUI>();
                if (itemText == null)
                {
                    Debug.LogError("TextMeshProUGUI component is missing on the prefab!");
                    yield break;
                }

                itemText.text = $"{user.username}: {user.highest_score}";
            }
        }
        else
        {
            Debug.LogError("Failed to fetch ranking data: " + request.error);
        }
    }

    // 사용자 데이터 구조체 정의
    [System.Serializable]
    public class User
    {
        public int id;
        public string username;
        public int highest_score;
    }

    [System.Serializable]
    public class UserList
    {
        public List<User> users;
    }
}
