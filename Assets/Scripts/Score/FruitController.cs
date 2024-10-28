using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//Khái báo biến FruitType kiểu liệt kê để nhóm các loại fruit lại với nhau và mỗi fruit sẽ tương ứng với 1 số nguyên bắt đầu từ 0
public enum FruitType
{
    Apple,
    Banana,
    Cherry,
    Kiwi,
    Melon,
    Orange,
    Pineapple,
    Strawberry
}
public class FruitController : MonoBehaviour
{
    [SerializeField] private FruitType fruitType;
    [SerializeField] private GameObject pickUpVFX;

    private GameManagerController gameManagerController;
    private Animator fruitAnimator;
    private void Awake()
    {
        fruitAnimator = GetComponentInChildren<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        gameManagerController = GameManagerController.instance;
        SetRandomLookIfNeeded();
    }

    //hàm kích hoạt random hình ảnh của fruit 
    private void SetRandomLookIfNeeded()
    {
        if (gameManagerController.FruitHasRandomLook() == false)
        {
            UpdateFruitVisuals();
            return;
        }

        int randomIndex = Random.Range(0, 8);
        fruitAnimator.SetFloat("fruitIndex", randomIndex);
    }

    private void UpdateFruitVisuals()
    {
        //kích hoạt animation với fruitType bị ép kiểu sang số nguyên
        fruitAnimator.SetFloat("fruitIndex", (int)fruitType);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController playerController = collision.GetComponent<PlayerController>();

        if(playerController != null)
        {
            //thêm fruit mỗi khi nhân vật tương tác
            gameManagerController.AddFruit();

            //hủy fruit
            Destroy(gameObject);

            //tạo ra bản sao hiệu ứng mỗi khi nhân vật tương tác với fruit
            GameObject newVFX = Instantiate(pickUpVFX, transform.position, Quaternion.identity);

            //hủy hiệu ứng sau 0.5s
            Destroy(newVFX, .5f);
        }
    }
}
