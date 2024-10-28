using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InGameUIController : MonoBehaviour
{
    public static InGameUIController instance;
    private PlayerInputAction playerInputAction;
    private DefaultInputActions defaultInputActions;
    private PlayerController playerController;
    //get bất kì đối tượng nào có thể đọc giá trị của nó
    //private set chỉ có các phương thức trong cùng một lớp mới có thể thiết lập giá trị cho thuộc tính này
    public FadeEffectUIController fadeEffectUIController { get; private set; }

    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI scoreText;

    [SerializeField] private GameObject pauseUI;
    private bool isPaused;
    [SerializeField] private GameObject firstSelected;

    private void Awake()
    {
        instance = this;

        fadeEffectUIController = GetComponentInChildren<FadeEffectUIController>();
        playerInputAction = new PlayerInputAction();
        defaultInputActions = new DefaultInputActions();
    }

    private void Start()
    {
        fadeEffectUIController.ScreenFade(0, 1);
    }

    private void OnEnable()
    {
        playerInputAction.Enable();
        defaultInputActions.Enable();
        playerInputAction.UI.Pause.performed += ctx => PauseButton();
        defaultInputActions.UI.Navigate.performed += ctx => UpdateSelected();
    }

    private void OnDisable()
    {
        playerInputAction.Disable();
        defaultInputActions.Disable();
        playerInputAction.UI.Pause.performed -= ctx => PauseButton();
        defaultInputActions.UI.Navigate.performed -= ctx => UpdateSelected();


    }
    public void PauseButton()
    {
        playerController = PlayerManagerController.instance.playerController;
        if (isPaused)
        {
            UnPauseGame();
        }
        else
        {
            PauseGame();
        }
    }
    private void UpdateSelected()
    {
        if(EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(firstSelected);
        }
    }
    private void PauseGame()
    {
        EventSystem.current.SetSelectedGameObject(firstSelected);
        playerController.playerInputAction.Disable();
        isPaused = true;
        Time.timeScale = 0;
        pauseUI.SetActive(true);
    }

    private void UnPauseGame()
    {
        playerController.playerInputAction.Enable();
        isPaused = false;
        Time.timeScale = 1;
        pauseUI.SetActive(false);
    }

    public void GoToMainMenuButton()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

    public void UpdateScoreUI(int collectedFruits, int totalFruits)
    {
        scoreText.text = collectedFruits + "/" + totalFruits;
    }

    public void UpdateTimeUI(float timer)
    {
        timeText.text = timer.ToString("00") + " s";
    }
}
