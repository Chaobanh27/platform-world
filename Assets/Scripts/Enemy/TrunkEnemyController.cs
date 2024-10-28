using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrunkEnemyController : EnemyController
{
    [SerializeField] private BulletController bulletPrefab;
    [SerializeField] private Transform gunPoint;
    [SerializeField] private float bulletSpeed = 7;
    [SerializeField] private float attackCooldown = 1.5f;
    private float lastTimeAttacked;

    protected override void Update()
    {
        base.Update();

        if (isDead)
            return;

        bool canAttack = Time.time > lastTimeAttacked + attackCooldown;

        if (isPlayer && canAttack)
            AttackController();


        MovementController();

        if (isGround)
            TurnAroundController();
    }

    private void AttackController()
    {
        idleTimer = idleDuration + attackCooldown;
        lastTimeAttacked = Time.time;
        enemyAnimator.SetTrigger("isAttack");
    }

    private void CreateBulletController()
    {
        BulletController newBullet = Instantiate(bulletPrefab, gunPoint.position, Quaternion.identity);

        Vector2 bulletVelocity = new Vector2(facingDirection * bulletSpeed, 0);
        newBullet.SetVelocityController(bulletVelocity);

        if (facingDirection == 1)
            newBullet.FlipSpriteController();

        Destroy(newBullet.gameObject, 10);
    }

    private void TurnAroundController()
    {
        if (!isFrontGround || isWall)
        {
            Flip();
            idleTimer = idleDuration;
            enemyRigidbody.linearVelocity = Vector2.zero;
        }
    }

    private void MovementController()
    {
        if (idleTimer > 0)
            return;

        enemyRigidbody.linearVelocity = new Vector2(moveSpeed * facingDirection, enemyRigidbody.linearVelocity.y);
    }
}
