using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTriggerController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();


        if (playerController != null)
        {
            playerController.Damage();
            playerController.KnockBackController(transform.position.x);
        }
    }
}
