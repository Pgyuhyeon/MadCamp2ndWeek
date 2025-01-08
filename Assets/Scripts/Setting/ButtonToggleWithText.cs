using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonToggleController : MonoBehaviour
{
    public Button toggleButton;            // 버튼 컴포넌트
    public TextMeshProUGUI buttonText;     // 버튼의 TextMeshProUGUI 컴포넌트

    public Sprite onSprite;                // On 상태 Sprite
    public Sprite offSprite;               // Off 상태 Sprite
    public string onText = "Sound On";     // On 상태 텍스트
    public string offText = "Sound Off";   // Off 상태 텍스트

    public AudioSource sounds;

    private const string PlayerPrefsKey = "IsSoundOn"; // PlayerPrefs 키 값

    void Start()
    {
        // 저장된 상태 불러오기 (기본값: 1)
        int savedState = PlayerPrefs.GetInt(PlayerPrefsKey, 1);
        bool isOn = savedState == 1;

        // 초기 상태 설정
        SetAudioState(isOn);
        UpdateButtonUI(isOn);

        // 버튼 클릭 이벤트 연결
        toggleButton.onClick.AddListener(() => ToggleButtonState());
    }

    void ToggleButtonState()
    {
        // 현재 상태 불러오기
        int currentState = PlayerPrefs.GetInt(PlayerPrefsKey, 1);
        bool isOn = currentState == 1;

        // 상태 변경
        isOn = !isOn;

        // 상태 저장 (0 또는 1)
        PlayerPrefs.SetInt(PlayerPrefsKey, isOn ? 1 : 0);
        PlayerPrefs.Save();

        // 소리 상태 변경 및 UI 업데이트
        SetAudioState(isOn);
        UpdateButtonUI(isOn);

        Debug.Log($"Button is now {(isOn ? "On" : "Off")}");
    }

    void UpdateButtonUI(bool isOn)
    {
        // 버튼의 기본 Image(Sprite) 변경
        Image buttonImage = toggleButton.GetComponent<Image>();
        if (buttonImage != null)
        {
            buttonImage.sprite = isOn ? onSprite : offSprite;
        }

        // TextMeshPro 텍스트 변경
        buttonText.text = isOn ? onText : offText;
    }

    void SetAudioState(bool isAudioOn)
    {
        // AudioSource를 사용해 소리 상태 변경
        sounds.mute = !isAudioOn;
        Debug.Log($"Audio is now {(isAudioOn ? "ON" : "OFF")}");
    }
}
