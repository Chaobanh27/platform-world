using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LevelSelectionUIController : MonoBehaviour
{
    private MainMenuUIController mainMenuUIController;
    [SerializeField] private GameObject firstSelected;
    [SerializeField] LevelBtnController buttonPrefab;
    [SerializeField] Transform buttonsParent;
    [SerializeField] private bool[] levelsUnlocked;
    private void Awake()
    {
        mainMenuUIController = GetComponentInParent<MainMenuUIController>();

        //phải để hàm render nút con ở đây bởi vì theo lifecycle trong unity thì hàm Awake sẽ chạy trước hàm OnEnable và hàm Start sẽ chạy sau hàm OnEnable 
        LoadLevelsInfo();
        CreateLevelButtons();
    }

    //không thể để render nút con trong hàm Start vì hàm Start sẽ chạy sau hàm OnEnable nên khi dùng buttonsParent.childCount trong OnEnable sẽ trả về 0
    //private void Start()
    //{
    //    LoadLevelsInfo();
    //    CreateLevelButtons();
    //}

    private void OnEnable()
    {

        mainMenuUIController.UpdateLastSelected(firstSelected);

        if (buttonsParent.childCount > 0)
        {
            GameObject firstLevelBTN = buttonsParent.GetChild(0).gameObject;
            if (firstLevelBTN != null)
            {
                EventSystem.current.SetSelectedGameObject(firstLevelBTN);
            }
            else
            {
                EventSystem.current.SetSelectedGameObject(firstSelected);
            }
        }
        else
        {
            return;
        }

    }
    //lặp ra các level scene trong build setting trừ đi 1 để loại credit scene và cho i bắt đầu là 1 để loại mainmenu scene
    private void CreateLevelButtons()
    {
        int totalLevels = SceneManager.sceneCountInBuildSettings - 1;
        for (int i = 1; i < totalLevels; i++)
        {
            if (IsLevelUnlocked(i) == false)
                return;

            //tạo ra các bản sao của button
            LevelBtnController newLevelButton = Instantiate(buttonPrefab, buttonsParent);

            //thêm số level ở đây là i đẵ lặp vào hàm ButtonController(i) thay đổi text và scene tương ứng với mỗi button
            newLevelButton.ButtonController(i);
        }
    }
    private bool IsLevelUnlocked(int levelIndex) => levelsUnlocked[levelIndex];

    //Hàm này sẽ load thông tin level đã unlock và chưa unlock
    private void LoadLevelsInfo()
    {
        int totalLevels = SceneManager.sceneCountInBuildSettings - 1;

        //mảng levelsUnlocked sẽ bằng mảng kiểu bool mới với số lượng là tổng các level 
        levelsUnlocked = new bool[totalLevels];

        for (int i = 1; i < totalLevels; i++)
        {
            //nếu như PlayerPrefs.GetInt("Level" + i + "Unlocked") trả về 1 tức là 
            bool levelUnlocked = PlayerPrefs.GetInt("Level" + i + "Unlocked") == 1;
            if (levelUnlocked)
                levelsUnlocked[i] = true;
        }

        levelsUnlocked[1] = true;
    }
}
