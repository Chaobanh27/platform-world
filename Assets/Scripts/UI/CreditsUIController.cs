using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsUIController : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private float scrollSpeed = 200;
    [SerializeField] private float offScreenPosition = 1800;
    [SerializeField] private FadeEffectUIController fadeEffectUIController;

    [SerializeField] private string mainMenuSceneName = "MainMenuScene";
    private bool creditsSkipped;

    private void Awake()
    {
        if(fadeEffectUIController != null)
        {
            fadeEffectUIController.ScreenFade(0, 2);
        }
    }

    private void Update()
    {
        //dịch chuyển credit đi lên
        rectTransform.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;

        //nếu như vị trí trục y của credit lớn hơn vị trí chỉ định thì dịch chuyển về menu chính
        if (rectTransform.anchoredPosition.y > offScreenPosition)
            GoToMainMenu();
    }

    public void SkipCreditsController()
    {
        if (creditsSkipped == false)
        {
            //tăng tốc độ cuộn lên của credits
            scrollSpeed *= 10;
            creditsSkipped = true;
        }
        else
        {
            GoToMainMenu();
        }
    }

    private void GoToMainMenu() => fadeEffectUIController.ScreenFade(1, 1, SwitchToMenuScene);

    private void SwitchToMenuScene()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }


}
