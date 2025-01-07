using UnityEngine;

public class learder_board_panel : MonoBehaviour
{
    public RankingManager rankingManager;
    private void OnEnable()
    {
        if (rankingManager != null)
        {
            rankingManager.GetRanking(); // Ranking 데이터 로드
        }
        else
        {
            Debug.LogError("RankingManager is not assigned!");
        }
    }
}
