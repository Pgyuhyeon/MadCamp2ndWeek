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

    private const string VibrationPrefKey = "IsVibrationOn"; // PlayerPrefs 키

    void Start()
    {
        // 저장된 상태 불러오기 (기본값: Off -> 0)
        int savedState = PlayerPrefs.GetInt(VibrationPrefKey, 0);
        bool isVibrationOn = savedState == 1;

        // 초기 상태 설정
        UpdateButtonUI(isVibrationOn);

        // 버튼 클릭 이벤트 연결
        toggleButton.onClick.AddListener(() => ToggleButtonState());
    }

    void ToggleButtonState()
    {
        // 현재 상태 불러오기
        int currentState = PlayerPrefs.GetInt(VibrationPrefKey, 0);
        bool isVibrationOn = currentState == 1;

        // 상태 변경
        isVibrationOn = !isVibrationOn;

        // 변경된 상태 저장
        PlayerPrefs.SetInt(VibrationPrefKey, isVibrationOn ? 1 : 0);
        PlayerPrefs.Save();

        // UI 업데이트
        UpdateButtonUI(isVibrationOn);

        Debug.Log($"Vibration is now {(isVibrationOn ? "On" : "Off")}");
    }

    void UpdateButtonUI(bool isVibrationOn)
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

    public void TriggerVibration()
    {
        // 진동 상태 확인
        bool isVibrationOn = PlayerPrefs.GetInt(VibrationPrefKey, 0) == 1;

        if (isVibrationOn)
        {
#if UNITY_ANDROID
            // Android 진동
            Handheld.Vibrate();
            Debug.Log("Vibration Triggered");
#elif UNITY_IOS
            // iOS Haptic Feedback
            Debug.Log("Haptic Feedback Triggered (iOS)");
#else
            Debug.Log("Vibration not supported on this platform");
#endif
        }
        else
        {
            Debug.Log("Vibration is off, no vibration triggered.");
        }
    }
}
