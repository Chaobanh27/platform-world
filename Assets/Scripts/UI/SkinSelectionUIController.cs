using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[System.Serializable]
public struct Skin
{
    public string skinName;
    public int skinPrice;
    public bool unlocked;
}

public class SkinSelectionUIController : MonoBehaviour
{
    private LevelSelectionUIController levelSelectionUIController;
    private MainMenuUIController mainMenuUIController;
    private DefaultInputActions defaultInputActions;
    [SerializeField] private GameObject firstSelected;

    [Header("Skin Info")]
    [SerializeField] private int skinIndex;
    [SerializeField] private int maxIndex;
    [SerializeField] private Animator listCharactersAnimator;
    [SerializeField] private Skin[] listSkins;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI buySelectText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private TextMeshProUGUI bankText;

    private void Awake()
    {

        mainMenuUIController = GetComponentInParent<MainMenuUIController>();
        levelSelectionUIController = mainMenuUIController.GetComponentInChildren<LevelSelectionUIController>(true);
        defaultInputActions = new DefaultInputActions();
    }
    private void Start()
    {
        LoadUnlockedSkins();
        UpdateSkinController();
    }

    private void OnEnable()
    {
        defaultInputActions.Enable();
        mainMenuUIController.UpdateLastSelected(firstSelected);
        EventSystem.current.SetSelectedGameObject(firstSelected);

        defaultInputActions.UI.Navigate.performed += ctx =>
        {
            if (ctx.ReadValue<Vector2>().x <= -1)
            {
                PreviousSkinController();
            }

            else if (ctx.ReadValue<Vector2>().x >= 1)
            {
                NextSkinController();
            }
            else
            {
                return;
            }
        };
    }

    private void OnDisable()
    {
        defaultInputActions.Disable();
    }
    private void LoadUnlockedSkins()
    {
        for (int i = 0; i < listSkins.Length; i++)
        {
            string skinName = listSkins[i].skinName;
            bool skinUnlocked = PlayerPrefs.GetInt(skinName + "Unlocked") == 1;

            if (skinUnlocked || i == 0)
                listSkins[i].unlocked = true;
        }
    }
    public void SkinSelectionController()
    {
        //set skin id trong SkinManagerController với layerIndex(currentIndex)
        SkinManagerController.instance.SetSkinIdController(skinIndex);
    }

    public void NextSkinController()
    {
        skinIndex++;
        if(skinIndex >= listCharactersAnimator.layerCount)
        {
            skinIndex = 0;
        }

        UpdateSkinController();

    }

    public void PreviousSkinController()
    {
        skinIndex--;

        if (skinIndex < 0)
        {
            skinIndex = listCharactersAnimator.layerCount - 1;
        }

        UpdateSkinController();
    }

    public void SelectSkin()
    {
        if (listSkins[skinIndex].unlocked == false)
        {
            BuySkin(skinIndex);
        }
        else
        {
            SkinManagerController.instance.SetSkinIdController(skinIndex);
            //levelSelectionUIController.gameObject dùng để tham chiếu đên gameObject mà script levelSelectionUIController đang gắn vào
            mainMenuUIController.SwitchUIController(levelSelectionUIController.gameObject);
        }


        UpdateSkinController();
    }

    private void UpdateSkinController()
    {
        bankText.text = FruitsInBank().ToString();

        for (int i = 0; i < listCharactersAnimator.layerCount; i++)
        {
            listCharactersAnimator.SetLayerWeight(i, 0);
        }

        listCharactersAnimator.SetLayerWeight(skinIndex, 1);


        if (listSkins[skinIndex].unlocked)
        {
            priceText.transform.parent.gameObject.SetActive(false);
            buySelectText.text = "Select".ToUpper();
        }
        else
        {
            priceText.transform.parent.gameObject.SetActive(true);
            priceText.text = listSkins[skinIndex].skinPrice.ToString();
            buySelectText.text = "Buy".ToUpper();

        }

    }

    private void BuySkin(int index)
    {
        if (HaveEnoughFruits(listSkins[index].skinPrice) == false)
        {
            Debug.Log("Not enough fruits");
            return;
        }


        string skinName = listSkins[skinIndex].skinName;
        listSkins[skinIndex].unlocked = true;

        PlayerPrefs.SetInt(skinName + "Unlocked", 1);
    }

    private int FruitsInBank() => PlayerPrefs.GetInt("TotalFruitsAmount");

    private bool HaveEnoughFruits(int price)
    {
        if (FruitsInBank() > price)
        {
            PlayerPrefs.SetInt("TotalFruitsAmount", FruitsInBank() - price);
            return true;
        }

        return false;
    }
}
