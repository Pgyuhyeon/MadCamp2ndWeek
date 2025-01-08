using UnityEngine;
using System.IO; // 파일 저장 관련
using System.Collections;

public class ScreenshotAndShare : MonoBehaviour
{
    public void CaptureAndSendToSMS()
    {
        StartCoroutine(CaptureAndSendScreenshotToSMS());
    }

    private IEnumerator CaptureAndSendScreenshotToSMS()
    {
        // 화면 크기 설정
        int width = Screen.width;
        int height = Screen.height;

        // Texture2D에 현재 화면 캡처
        Texture2D screenshot = new Texture2D(width, height, TextureFormat.RGB24, false);
        yield return new WaitForEndOfFrame(); // 프레임 끝날 때까지 대기
        screenshot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        screenshot.Apply();

        // PNG로 변환 및 저장
        byte[] pngData = screenshot.EncodeToPNG();
        string filePath = "";

#if UNITY_ANDROID
        // Android의 캐시 디렉토리에 저장
        string cachePath = Application.persistentDataPath;
        filePath = Path.Combine(cachePath, "screenshot.png");
        File.WriteAllBytes(filePath, pngData);

        Debug.Log($"Screenshot saved to: {filePath}");

        // Android Intent 생성
        using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

            AndroidJavaObject intent = new AndroidJavaObject("android.content.Intent");
            intent.Call("setAction", "android.intent.action.SENDTO");

            // 파일 URI 생성 및 첨부
            string smsUri = "smsto:"; // 전화번호 없이 SMS 앱 열기
            intent.Call("setData", new AndroidJavaClass("android.net.Uri").CallStatic<AndroidJavaObject>("parse", smsUri));

            // 이미지 첨부 불가 (SMS는 일반적으로 텍스트만 허용)
            // intent.Call("putExtra", "android.intent.extra.STREAM", fileUri); // MMS 지원 시 사용 가능

            // 미리 작성된 텍스트 추가
            string message = "안녕하세요, 캡처된 이미지를 확인하세요!";
            intent.Call("putExtra", "sms_body", message);

            // Intent 실행
            currentActivity.Call("startActivity", intent);
        }
#else
        Debug.LogWarning("This functionality is for Android only.");
#endif
    }
}
