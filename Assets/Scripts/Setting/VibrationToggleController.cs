using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VibrationToggleController : MonoBehaviour
{
    public Button toggleButton;            // 버튼 컴포넌트
    public TextMeshProUGUI buttonText;     // 버튼의 TextMeshProUGUI 컴포넌트

    public Sprite onSprite;                // On 상태 Sprite
    public Sprite offSprite;               // Off 상태 Sprite
    public string onText = "Vibration On"; // On 상태 텍스트
    public string offText = "Vibration Off"; // Off 상태 텍스트

    private bool isVibrationOn = false;    // 진동 상태 (On/Off)

    void Start()
    {
        // 초기 상태 설정
        UpdateButtonUI();

        // 버튼 클릭 이벤트 연결
        toggleButton.onClick.AddListener(ToggleButtonState);
    }

    void ToggleButtonState()
    {
        // 상태 변경
        isVibrationOn = !isVibrationOn;

        // UI 업데이트
        UpdateButtonUI();

        // 진동 상태 변경
        if (isVibrationOn)
        {
            TriggerVibration(); // 진동 활성화
        }
        else
        {
            Debug.Log("Vibration Off");
        }

        Debug.Log($"Vibration is now {(isVibrationOn ? "On" : "Off")}");
    }

    void UpdateButtonUI()
    {
        // 버튼의 기본 Image(Sprite) 변경
        Image buttonImage = toggleButton.GetComponent<Image>();
        if (buttonImage != null)
        {
            buttonImage.sprite = isVibrationOn ? onSprite : offSprite;
        }

        // TextMeshPro 텍스트 변경
        buttonText.text = isVibrationOn ? onText : offText;
    }

    void TriggerVibration()
    {
#if UNITY_ANDROID
        // Android 진동 예제
        Handheld.Vibrate();
        Debug.Log("Vibration Triggered");
#elif UNITY_IOS
        // iOS Haptic Feedback 예제
        Debug.Log("Haptic Feedback Triggered (iOS)");
#else
        Debug.Log("Vibration not supported on this platform");
#endif
    }
}
