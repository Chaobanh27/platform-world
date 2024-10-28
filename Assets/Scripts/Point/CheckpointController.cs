using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    private Animator checkpointAnimator;
    private bool isActive;
    private bool canBeReactivate;

    private void Awake()
    {
        checkpointAnimator = GetComponent<Animator>();
    }
    private void Start()
    {
        canBeReactivate = GameManagerController.instance.canBeReactive;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isActive && canBeReactivate == false)
            return;

        PlayerController playerController = collision.GetComponent<PlayerController>();

        if(playerController == null) return;
        if(playerController != null)
        {
            ActivateCheckpoint();
        }
    }

    private void ActivateCheckpoint()
    {
        isActive = true;
        checkpointAnimator.SetBool("isActive", isActive);
        PlayerManagerController.instance.UpdateRespawnPosition(transform);
    }
}
