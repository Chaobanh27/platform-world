using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampolineController : MonoBehaviour
{
    protected Animator trampolineAnimator;
    [SerializeField] private float pushPower;
    [SerializeField] private float duration = .5f;
    private void Awake()
    {
        trampolineAnimator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();

        if(playerController != null)
        {
            playerController.PushController(transform.up * pushPower, duration);
            trampolineAnimator.SetTrigger("isActive");
        }
    }
}
