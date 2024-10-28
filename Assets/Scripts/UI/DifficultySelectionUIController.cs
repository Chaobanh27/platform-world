using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DifficultySelectionUIController : MonoBehaviour
{
    private MainMenuUIController mainMenuUIController;
    [SerializeField] private GameObject firstSelected;
    private DifficultyManagerController difficultyManager;

    private void Awake()
    {
        mainMenuUIController = GetComponentInParent<MainMenuUIController>();
    }
    private void OnEnable()
    {
        mainMenuUIController.UpdateLastSelected(firstSelected);
        EventSystem.current.SetSelectedGameObject(firstSelected);
    }
    private void Start()
    {
        difficultyManager = DifficultyManagerController.instance;
    }

    public void SetEasyMode() => difficultyManager.SetDifficulty(DifficultyType.Easy);
    public void SetMediumMode() => difficultyManager.SetDifficulty(DifficultyType.Medium);
    public void SetHardMode() => difficultyManager.SetDifficulty(DifficultyType.Hard);
}
