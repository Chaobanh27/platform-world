using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelBtnController : MonoBehaviour
{
    //lấy đối tượng text của button
    [SerializeField] TextMeshProUGUI levelNumberText;
    private int levelIndex;
    private string sceneName;
    [SerializeField] private TextMeshProUGUI bestTimeText;
    [SerializeField] private TextMeshProUGUI scoreText;

    public void ButtonController(int newLevelIndex)
    {
        levelIndex = newLevelIndex;

        levelNumberText.text = "Level " + levelIndex;
        sceneName = "Level " + levelIndex;


        bestTimeText.text = TimeInfoText();
        scoreText.text = ScoreInfoText();
    }

    public void LoadLevelController()
    {
        SceneManager.LoadScene(sceneName);
    }

    //hàm chuyển score lấy trong playpref rồi chuyển từ int sang string
    private string ScoreInfoText()
    {
        int totalFruits = PlayerPrefs.GetInt("Level" + levelIndex + "TotalFruits");
        string totalFruitsText = totalFruits == 0 ? "?" : totalFruits.ToString();

        int collectedFruits = PlayerPrefs.GetInt("Level" + levelIndex + "CollectedFruits");

        return "Fruits: " + collectedFruits + " / " + totalFruitsText;

    }

    //hàm chuyển best time lấy trong playpref rồi chuyển từ float sang string
    private string TimeInfoText()
    {
        float timerValue = PlayerPrefs.GetFloat("Level" + levelIndex + "BestTime", 00);

        return "Best Time: " + timerValue.ToString("00");

    }
}
