using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DifficultyType { Easy = 1, Medium, Hard }

public class DifficultyManagerController : MonoBehaviour
{
    public static DifficultyManagerController instance;

    public DifficultyType difficulty;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void SetDifficulty(DifficultyType newDifficulty) => difficulty = newDifficulty;
    public void LoadDifficulty(int difficultyIndex)
    {
        difficulty = (DifficultyType)difficultyIndex;
    }
}
