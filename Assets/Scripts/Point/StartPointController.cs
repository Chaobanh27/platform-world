using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPointController : MonoBehaviour
{
    private Animator startPointAnimator;
    void Start()
    {
        startPointAnimator = GetComponent<Animator>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();

        if(playerController != null)
        {
            startPointAnimator.SetTrigger("isActive");
        }
    }
}
