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

    private bool isOn = true;              // 버튼 상태 (초기값: On 상태)

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
        isOn = !isOn;

        // 소리 상태 변경
        SetAudioState(isOn);

        // UI 업데이트
        UpdateButtonUI();

        Debug.Log($"Button is now {(isOn ? "On" : "Off")}");
    }

    void UpdateButtonUI()
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
        // AudioListener를 사용해 소리 상태 변경
        AudioListener.volume = isAudioOn ? 1f : 0f;

        if (isAudioOn)
        {
            Debug.Log("Audio is now ON");
        }
        else
        {
            Debug.Log("Audio is now OFF");
        }
    }
}
