using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvent : MonoBehaviour
{
    private PlayerController playerController;

    private void Awake()
    {
        //lấy game object cha là game object player
        playerController = GetComponentInParent<PlayerController>();
    }

    //kích hoạt hàm kiểm tra hoàn thành respawn với tham số là true
    public void FinishRespawnEvent()
    {
        playerController.RespawnFinished(true);
    }
}
