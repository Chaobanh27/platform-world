using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZoneController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController playerController = collision.GetComponent<PlayerController>();

        if(playerController != null)
        {
            //kích hoạt hành động nhân vật chết
            playerController.Die();

            //kích hoạt sự kiện hồi sinh nhân vật
            PlayerManagerController.instance.RespawnPlayerController();
        }
    }
}
