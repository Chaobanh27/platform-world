using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class MushroomEnemyController : EnemyController
{

    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();
    }
    protected override void Update()
    {
        base.Update();

        if (isDead) return;

        MovementController();
        if (isGround)
        {
            TurnBackController();
        }
    }

    private void TurnBackController()
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
        //Gán vector2 mới vào vận tốc của nhân vật và lấy moveSpeed + facingDirection để tính toán hướng di chuyển của nhân vật
        enemyRigidbody.linearVelocity = new Vector2(moveSpeed * facingDirection, enemyRigidbody.linearVelocity.y);
    }
}
