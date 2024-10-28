using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerController : MonoBehaviour
{
    //tạo game object GameManagerController
    //trong script tạo biến instance kiểu GameManagerController
    //áp dụng singleton cho GameManager để có thể truy cập ở mọi nơi và chỉ khởi tạo 1 lần
    public static GameManagerController instance;
    private InGameUIController inGameUIController;

    [Header("Manager")]
    [SerializeField] private PlayerManagerController playerManagerController;
    [SerializeField] private AudioManagerController audioManagerController;
    [SerializeField] private SkinManagerController skinManagerController;
    [SerializeField] private DifficultyManagerController difficultyManagerController;
    [SerializeField] private CreateObjectCloneController createObjectCloneController;

    [Header("Level")]
    private float levelTime;
    [SerializeField] private int currentLevelIndex;
    private int nextLevelIndex;

    [Header("Fruit")]
    public bool fruitAreRandom;
    public int collectedFruits;
    public int totalFruits;
    public FruitController[] fruits;

    [Header("Checkpoint")]
    public bool canBeReactive;
    public GameObject arrowPrefab;

    public String creditScene = "CreditsScene";
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        inGameUIController = InGameUIController.instance;

        //trả về chỉ mục của scene đang hoạt động
        currentLevelIndex = SceneManager.GetActiveScene().buildIndex;

        //level tiếp theo sẽ là chỉ mục lv hiện tại cộng thêm 1
        nextLevelIndex = currentLevelIndex + 1;

        //lấy thông tin fruit để sau này đưa lên UI
        AllFruitsInfo();
        CreateManagersIfNeeded();
    }

    private void Update()
    {
        //dùng time.Deltatime thay vì Time.time để tính thời gian chạy của từng scene chứ không phải thời gian của hệ thống 
        levelTime += Time.deltaTime;
        inGameUIController.UpdateTimeUI(levelTime);
    }


    private void CreateManagersIfNeeded()
    {
        if (AudioManagerController.instance == null) 
        {
            Instantiate(audioManagerController);
        }

        if(PlayerManagerController.instance == null)
        {
            Instantiate(playerManagerController);
        }

        if (SkinManagerController.instance == null)
        {
            Instantiate(skinManagerController);
        }

        if (DifficultyManagerController.instance == null)
        {
            Instantiate(difficultyManagerController);
        }
        if (CreateObjectCloneController.instance == null)
        {
            Instantiate(createObjectCloneController);
        }
    }
    private void AllFruitsInfo()
    {
        //tạo 1 mảng fruits rồi tìm tất các game object có componet là FruitController và không sắp xếp mảng trả về
        fruits = FindObjectsByType<FruitController>(FindObjectsSortMode.None);
        totalFruits = fruits.Length;
        inGameUIController.UpdateScoreUI(collectedFruits, totalFruits);

        //lưu tổng fruit vào playerPref
        PlayerPrefs.SetInt("Level" + currentLevelIndex + "TotalFruits", totalFruits);
    }
    public void AddFruit()
    {
        collectedFruits++;
        inGameUIController.UpdateScoreUI(collectedFruits, totalFruits);
    }
    public void RemoveFruit()
    {
        collectedFruits--;
        inGameUIController.UpdateScoreUI(collectedFruits, totalFruits);
    }
    public int CollectedFruits() => collectedFruits;

    public bool FruitHasRandomLook() => fruitAreRandom;

    public void LevelFinishedController()
    {
        //lưu thông tin level
        SaveLevelProgression();

        //lưu thông tin best time
        SaveBestTime();

        //lưu thông tin score
        SaveScoreInfo();

        //chuyển sang level tiếp theo
        LoadNextScene();
    }
    private void SaveScoreInfo()
    {
        //lấy số fruit đã thu thập được đã lưu trong playerPref trước đó
        int collectedFruitBefore = PlayerPrefs.GetInt("Level" + currentLevelIndex + "CollectedFruits");

        //nếu như số fruit đã thu thập được đã lưu trong playerPref trước đó bé hơn số fruit đang thu nhập hiện tại
        if (collectedFruitBefore < collectedFruits)
            //lưu số fruit hiện tại vào playerPref
            PlayerPrefs.SetInt("Level" + currentLevelIndex + "CollectedFruits", collectedFruits);

        //lấy tổng số fruit đang lưu trữ hiện tại
        int totalFruitsInBank = PlayerPrefs.GetInt("TotalFruitsAmount");

        //thêm số fruit vừa mới nhặt được vào số fruit hiện tại 
        PlayerPrefs.SetInt("TotalFruitsAmount", 100 + collectedFruits);
    }
    private void SaveBestTime()
    {
        float lastBestTime = PlayerPrefs.GetFloat("Level" + currentLevelIndex + "BestTime", 99);

        if (levelTime < lastBestTime)
            PlayerPrefs.SetFloat("Level" + currentLevelIndex + "BestTime", levelTime);
    }
    private void SaveLevelProgression()
    {
        PlayerPrefs.SetInt("Level" + nextLevelIndex + "Unlocked", 1);

        //kiểm tra nếu vẫn còn level chưa hoàn thành thì sẽ lưu vào ContinueLevelNumber
        if (NoMoreLevels() == false)
            PlayerPrefs.SetInt("ContinueLevelNumber", nextLevelIndex);
    }
    private void LoadCreditScene() => SceneManager.LoadScene(creditScene);
    public void RestartLevel()
    {
        inGameUIController.fadeEffectUIController.ScreenFade(1, .75f, LoadCurrentLevel);
    }
    private void LoadCurrentLevel() => SceneManager.LoadScene("Level " + currentLevelIndex);
    private void LoadNextLevel()
    {
        SceneManager.LoadScene("Level " + nextLevelIndex);
    }
    private void LoadNextScene()
    {
        FadeEffectUIController fadeEffect = InGameUIController.instance.fadeEffectUIController;

        if (NoMoreLevels())
        {
            fadeEffect.ScreenFade(1, 1.5f, LoadCreditScene);
        }
        else
        {
            fadeEffect.ScreenFade(1, 1.5f, LoadNextLevel);
        }
    }
    private bool NoMoreLevels()
    {
        //lấy tổng số scene trong build setting và sau đó trừ đi 2 ( mainmenu scene và credit scene ) sẽ còn lại số lượng level scene và đó cũng là chỉ mục level cuối
        int lastLevelIndex = SceneManager.sceneCountInBuildSettings - 2; 

        //kiểm tra nếu như chỉ mục lv hiện tại bằng với chỉ mục lv cuối
        bool noMoreLevels = currentLevelIndex == lastLevelIndex;

        return noMoreLevels;
    }
}
