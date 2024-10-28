using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhinoEnemyController : EnemyController
{

    [SerializeField] private float maxSpeed;
    [SerializeField] private float speedUpRate;
    [SerializeField] private Vector2 impactPower;
    private float defaultSpeed;


    protected override void Start()
    {
        base.Start();
        canMove = false;
        defaultSpeed = moveSpeed;
    }

    protected override void Update()
    {
        base.Update();
        MovementController();

        if (isPlayer)
        {
            canMove = true;
        }
    }

    private void MovementController()
    {
        if (canMove == false)
            return;

        SpeedUpController();

        enemyRigidbody.linearVelocity = new Vector2 (moveSpeed * facingDirection, enemyRigidbody.linearVelocity.y);

        if (isGround)
        {
            if (isWall)
            {
                HitWallController();
            }

            if (!isFrontGround)
            {
                TurnAroundController();
            }
        }

    }

    private void TurnAroundController()
    {
        ResetSpeedController();
        canMove = false;
        enemyRigidbody.linearVelocity = Vector2.zero;
        Flip();
    }

    private void SpeedUpController()
    {
        moveSpeed = moveSpeed + ( speedUpRate * Time.deltaTime );

    }

    private void ResetSpeedController()
    {
        moveSpeed = defaultSpeed;
    }

    private void HitWallController()
    {
        canMove = false;
        ResetSpeedController();
        enemyAnimator.SetBool("isHitWall", true);
        enemyRigidbody.linearVelocity = new Vector2(impactPower.x * -facingDirection, impactPower.y);
    }

    private void ChargeIsOver()
    {
        enemyAnimator.SetBool("isHitWall", false);
        Invoke("Flip", 1);
    }
}
