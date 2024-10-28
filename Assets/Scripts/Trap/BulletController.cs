using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private Rigidbody2D bulletRB;
    private SpriteRenderer bulletSR;

    private void Awake()
    {
        bulletRB = GetComponent<Rigidbody2D>();
        bulletSR = GetComponent<SpriteRenderer>();
    }

    public void FlipSpriteController() => bulletSR.flipX = !bulletSR.flipX;
    public void SetVelocityController(Vector2 velocity) => bulletRB.linearVelocity = velocity;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerController playerController = collision.GetComponent<PlayerController>();
            playerController.Damage();
            playerController.KnockBackController(transform.position.x);
            Destroy(gameObject);
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Destroy(gameObject);
        }
    }

}
