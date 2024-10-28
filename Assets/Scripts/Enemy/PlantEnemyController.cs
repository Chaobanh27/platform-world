using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantEnemyController : EnemyController
{
    [Header("Plant details")]
    [SerializeField] private BulletController bulletPrefab;
    [SerializeField] private Transform gunPoint;
    [SerializeField] private float bulletSpeed = 7;
    [SerializeField] private float attackCooldown = 1.5f;
    private float lastTimeAttacked;

    protected override void Update()
    {
        base.Update();

        bool canAttack = Time.time > lastTimeAttacked + attackCooldown;

        if (isPlayer && canAttack)
            AttackController();
    }

    private void AttackController()
    {
        lastTimeAttacked = Time.time;
        enemyAnimator.SetTrigger("isAttack");
    }

    private void CreateBulletsController()
    {
        BulletController newBullet = Instantiate(bulletPrefab, gunPoint.position, Quaternion.identity);

        Vector2 bulletVelocity = new Vector2(facingDirection * bulletSpeed, 0);
        newBullet.SetVelocityController(bulletVelocity);

        Destroy(newBullet.gameObject, 10);
    }

}
