using UnityEngine;

public class KakaoLoginManager : MonoBehaviour
{
    private string clientId = "YOUR_REST_API_KEY"; // 카카오 REST API 키
    private string redirectUri = "http://yourserver.com/oauth/callback"; // Redirect URI

    public void OpenKakaoLogin()
    {
        string kakaoLoginUrl = $"https://kauth.kakao.com/oauth/authorize"
            + $"?client_id={clientId}"
            + $"&redirect_uri={redirectUri}"
            + $"&response_type=code";

        Application.OpenURL(kakaoLoginUrl);
    }
}
