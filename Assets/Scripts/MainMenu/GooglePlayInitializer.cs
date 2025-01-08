//using GooglePlayGames;
//using GooglePlayGames.BasicApi;
//using UnityEngine;

//public class GooglePlayInitializer : MonoBehaviour
//{
//    void Start()
//    {
//        // Google Play Games 초기화
//        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
//            .RequestEmail() // 이메일 요청 (선택 사항)
//            .RequestIdToken() // ID 토큰 요청 (선택 사항)
//            .Build();

//        PlayGamesPlatform.InitializeInstance(config);
//        PlayGamesPlatform.Activate();

//        // 로그인 시도
//        SignInToGooglePlay();
//    }

//    void SignInToGooglePlay()
//    {
//        Social.localUser.Authenticate((bool success) =>
//        {
//            if (success)
//            {
//                Debug.Log("Google Play 로그인 성공");
//            }
//            else
//            {
//                Debug.LogError("Google Play 로그인 실패");
//            }
//        });
//    }
//}
