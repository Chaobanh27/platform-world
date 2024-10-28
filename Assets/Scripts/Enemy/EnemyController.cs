using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;

public class EnemyController : MonoBehaviour
{
    private SpriteRenderer enemySR;
    [SerializeField] protected Transform player;
    protected Animator enemyAnimator;
    protected Rigidbody2D enemyRigidbody;
    protected Collider2D[] enemyColliders;

    [Header("Enemy Movement")]
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float idleDuration;
    [SerializeField] protected float idleTimer;
    protected bool canMove = true;
    protected int facingDirection = -1;
    protected bool facingRight = false;

    [Header("Enemy Death")]
    [SerializeField] protected float deathCollisionSpeed;
    [SerializeField] protected float deathRotationSpeed;
    protected int deathRotationDirection = 1;
    protected bool isDead;

    [Header("Enemy Collision")]
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected LayerMask groundCheck;
    [SerializeField] protected float playerCheckDistance;
    [SerializeField] protected LayerMask playerCheck;
    [SerializeField] protected Transform ground;
    protected bool isPlayer;
    protected bool isGround;
    protected bool isWall;
    protected bool isFrontGround;


    protected virtual void Awake()
    {
        enemySR = GetComponent<SpriteRenderer>();
        enemyAnimator = GetComponent<Animator>();
        enemyRigidbody = GetComponent<Rigidbody2D>();
        enemyColliders = GetComponentsInChildren<Collider2D>();
    }


    // Start is called before the first frame update
    protected virtual void Start()
    {
        if (enemySR.flipX == true && !facingRight)
        {
            enemySR.flipX = false;
            Flip();
        }

        PlayerManagerController.onPlayerRespawn += UpdatePlayerController;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        CollisionController();
        AnimationController();

        idleTimer -= Time.deltaTime;

        if(isDead)
        {
            DeathRotationController();
        }
    }

    private void UpdatePlayerController()
    {
        if (player == null)
        {
            Transform playerController = PlayerManagerController.instance.playerController.transform;
            player = playerController;
        }
    }

    public virtual void DeathController()
    {
        foreach(var collider in enemyColliders)
        {
            collider.enabled = false;
        }

        enemyAnimator.SetTrigger("isHit");
        enemyRigidbody.linearVelocity = new Vector2(enemyRigidbody.linearVelocity.x, deathCollisionSpeed);
        isDead = true;
        if(Random.Range(0, 100) < 50)
        {
            deathRotationDirection *= -1;
        }
        PlayerManagerController.onPlayerRespawn -= UpdatePlayerController;
        Destroy(gameObject, 10);
    }

    private void DeathRotationController()
    {
        transform.Rotate(0,0, (deathRotationSpeed * deathRotationDirection) * Time.deltaTime);
    }

    protected virtual void FlipController(float xInput)
    {

        //nhân vật sẽ xoay theo điều kiện dưới đây
        if (xInput < 0 && facingRight || xInput > 0 && !facingRight)
        {
            Flip();
        }
    }

    protected virtual void Flip()
    {
        //thay đổi biến facingDirection khi thay đổi hướng nhân vật để dùng cho việc đổi hướng cùng nhân vật Raycast và Gizmos sau này
        facingDirection = facingDirection * -1;
        transform.Rotate(0, 180, 0);
        facingRight = !facingRight;
    }

    protected virtual void AnimationController()
    {
        enemyAnimator.SetFloat("xVelocity", enemyRigidbody.linearVelocity.x);
    }

    protected virtual void CollisionController()
    {
        //Dùng Physics2D.Raycast để thực hiện kiểm tra va chạm (collision detection) dựa trên tia (ray) trong không gian 2D
        isGround = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundCheck);
        isFrontGround = Physics2D.Raycast(ground.position, Vector2.down, groundCheckDistance, groundCheck);
        isWall = Physics2D.Raycast(transform.position, Vector2.right * facingDirection, wallCheckDistance, groundCheck);
        isPlayer = Physics2D.Raycast(transform.position, Vector2.right * facingDirection, playerCheckDistance, playerCheck);
    }

    //vẽ đường gizmos để nó hiển thị song song với Raycast phía trên bởi vì Raycast không thể thấy được
    protected virtual void OnDrawGizmos()
    {
        //dùng gizmos vẽ 1 đường thẳng để kết hợp với Raycast kiểm tra việc tiếp xúc với ground của nhân vật
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - groundCheckDistance));
        Gizmos.DrawLine(ground.position, new Vector2(ground.position.x, ground.position.y - groundCheckDistance));
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + (wallCheckDistance * facingDirection), transform.position.y));
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + (playerCheckDistance * facingDirection), transform.position.y));
    }
}
