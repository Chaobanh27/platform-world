using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingMenuUIController : MonoBehaviour
{
    private MainMenuUIController mainMenuUIController;
    [SerializeField] private GameObject firstSelected;
    [Space]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private float mixerMultiplier = 25;

    [Header("SFX settings")]
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private TextMeshProUGUI sfxSliderText;
    [SerializeField] private string sfxParameter;

    [Header("BGM settings")]
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private TextMeshProUGUI bgmSliderText;
    [SerializeField] private string bgmParameter;

    private void Awake()
    {
        mainMenuUIController = GetComponentInParent<MainMenuUIController>();
    }

    public void SfxSliderValue(float value)
    {
        sfxSliderText.text = Mathf.RoundToInt(value * 100) + "%";
        float newValue = Mathf.Log10(value) * mixerMultiplier;
        audioMixer.SetFloat(sfxParameter, newValue);
    }

    public void BgmSliderValue(float value)
    {
        bgmSliderText.text = Mathf.RoundToInt(value * 100) + "%";
        float newValue = Mathf.Log10(value) * mixerMultiplier;
        audioMixer.SetFloat(bgmParameter, newValue);
    }
    private void OnEnable()
    {
        mainMenuUIController.UpdateLastSelected(firstSelected);
        EventSystem.current.SetSelectedGameObject(firstSelected);

        sfxSlider.value = PlayerPrefs.GetFloat(sfxParameter, .7f);
        bgmSlider.value = PlayerPrefs.GetFloat(bgmParameter, .7f);
    }
    private void OnDisable()
    {
        PlayerPrefs.SetFloat(sfxParameter, sfxSlider.value);
        PlayerPrefs.SetFloat(bgmParameter, bgmSlider.value);
    }


}
