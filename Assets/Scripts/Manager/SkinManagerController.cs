using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinManagerController : MonoBehaviour
{
    public int chosenSkinId;
    public static SkinManagerController instance;

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

    public void SetSkinIdController(int id) => chosenSkinId = id;
    public int GetSkinIdController() => chosenSkinId;
}
