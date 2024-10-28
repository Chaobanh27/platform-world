using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishPointController : MonoBehaviour
{
    private Animator finishPointAnimator;

    private void Awake()
    {
        finishPointAnimator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController playerController = collision.GetComponent<PlayerController>();

        if(playerController != null)
        {
            finishPointAnimator.SetTrigger("isActive");
            StartCoroutine(LoadCreditCoroutine());
        }
    }

    private IEnumerator LoadCreditCoroutine()
    {
        yield return new WaitForSeconds(3);
        GameManagerController.instance.LevelFinishedController();
    }
}
