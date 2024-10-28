using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenEnemyController : EnemyController
{
    [SerializeField] private float aggregationDuration;

    private float aggregationTimer;
    private bool canFlip = true;

    protected override void Update()
    {
        base.Update();

        //sử dụng aggregationTimer ở đây đê thiết lập khi nào chicken enemy mới được di chuyển 
        aggregationTimer -= Time.deltaTime;

        if (isDead)
            return;

        //nếu như phát hiện player thì mới cho phép di chuyển và aggregationTimer = aggregationDuration và bắt đầu đếm ngược về 0
        if (isPlayer)
        {
            canMove = true;
            aggregationTimer = aggregationDuration;
        }

        //nếu như âm thì không di chuyển
        if (aggregationTimer < 0)
            canMove = false;

        MovementController();

        if (isGround)
            TurnAroundController();
    }

    private void TurnAroundController()
    {
        //nếu như phía trước không là ground hoặc chạm tường
        if (!isFrontGround || isWall)
        {
            Flip();
            canMove = false;
            enemyRigidbody.linearVelocity = Vector2.zero;
        }
    }

    private void MovementController()
    {
        if (canMove == false)
            return;
        if (player == null || player.gameObject == null)
        {
            PlayerController foundPlayer = FindAnyObjectByType<PlayerController>();
            if (foundPlayer != null)
            {
                player = foundPlayer.transform;
            }
            else
            {
                return;
            }
        }
        else
        {
            if (player.gameObject == null)
            {
                player = null;
                return;
            }

            FlipController(player.position.x);

            enemyRigidbody.linearVelocity = new Vector2(moveSpeed * facingDirection, enemyRigidbody.linearVelocity.y);
        }
    }

    //hàm này sẽ có tham số là xValue là vị trí trục x của player
    // mục đích là chúng ta sẽ chuyển hướng di chuyển của enemy theo hướng của player
    protected override void FlipController(float xValue)
    {

        //nếu vị trí trục x của player bé hơn vị trí trục x của chicken enemy và chicken enemy đang hướng phải hoặc ngược lại thì flip
        if (xValue < transform.position.x && facingRight || xValue > transform.position.x && !facingRight)
        {
            if (canFlip)
            {
                canFlip = false;
                Invoke(nameof(Flip), .3f);
            }
        }
    }

    protected override void Flip()
    {
        base.Flip();
        canFlip = true;
    }

}
