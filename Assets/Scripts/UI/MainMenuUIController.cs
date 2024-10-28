using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUIController : MonoBehaviour
{

    private FadeEffectUIController fadeEffectUIController;
    private DefaultInputActions defaultInputActions;
    [SerializeField] private GameObject lastSelected;
    //tạo ra mảng chứa các phần tử UI
    [SerializeField] private GameObject[] uiElements;
    [SerializeField] private GameObject continueGameBTN;

    [Header("Volume")]
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private string sfxParameter;
    [SerializeField] private string bgmParameter;
    private void Awake()
    {
        fadeEffectUIController = GetComponentInChildren<FadeEffectUIController>();
        defaultInputActions = new DefaultInputActions();
    }
    private void Start()
    {
        if (HasLevelProgression())
            continueGameBTN.SetActive(true);

        if (fadeEffectUIController != null)
        {
            fadeEffectUIController.ScreenFade(0, 1.5f);
        }
        sfxSlider.value = PlayerPrefs.GetFloat(sfxParameter, .7f);
        bgmSlider.value = PlayerPrefs.GetFloat(bgmParameter, .7f);

    }
    private void OnEnable()
    {
        UpdateLastSelected(EventSystem.current.currentSelectedGameObject);
        defaultInputActions.Enable();
        defaultInputActions.UI.Navigate.performed += ctx => UpdateSelectedController();

    }

    public void UpdateLastSelected(GameObject newLastSelected)
    {
        lastSelected = newLastSelected;
    }
    private void UpdateSelectedController()
    {
        if(EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(lastSelected);
        }
    }

    //hàm này dùng để gắn vào button đê chuyển đổi UI
    public void SwitchUIController(GameObject uiToEnable)
    {
        //lặp ra các phần tử trong mảng và thiết lập active là false cho UI không cần hiển thị
        foreach(GameObject ui in uiElements)
        {
            ui.SetActive(false);
        }

        AudioManagerController.instance.PlaySfx(4);
        //thiết lập active là true cho UI cần hiển thị
        uiToEnable.SetActive(true);
    }

    public void ContinueGameController()
    {
        int levelToLoad = PlayerPrefs.GetInt("ContinueLevelNumber");

        SceneManager.LoadScene("Level " + levelToLoad);
    }

    //kiểm tra nếu như có level nào đang chơi dở
    private bool HasLevelProgression()
    {
        //nếu PlayerPrefs.GetInt("ContinueLevelNumber") trả về giá trị lớn hơn 0 thì chứng tỏ có level đang chơi dở
        bool hasLevelProgression = PlayerPrefs.GetInt("ContinueLevelNumber") > 0;

        return hasLevelProgression;
    }

    public void ExitGameController()
    {
        Application.Quit();
        Debug.Log("Game is Exiting");
    }

}
