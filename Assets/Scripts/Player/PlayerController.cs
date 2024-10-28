using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Khởi tạo biến 
    //Với get Chỉ cho phép đọc giá trị bên ngoài class và với private set chỉ cho phép thay đổi giá trị bên trong class
    public PlayerInputAction playerInputAction { get; private set; }
    private Rigidbody2D playerRigidbody;
    private Animator playerAnimator;
    private CapsuleCollider2D playerCapsuleCollider;

    private DifficultyType gameDifficulty;
    private GameManagerController gameManagerController;

    [Header("Movement")]
    private Vector2 moveInput;
    //private float xInput;
    //private float yInput;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float doubleJumpForce;
    private bool canDoubleJump;

    [Header("Buffer & Coyote jump")]
    [SerializeField] private float bufferJumpWindow = .25f;
    private float bufferJumpActivated = -1;
    [SerializeField] private float coyoteJumpWindow = .25f;
    private float coyoteJumpActivated = -1;

    [Header("Wall Jump")]
    [SerializeField] private float wallJumpDuration = 1;
    [SerializeField] private Vector2 wallJumpForce;
    [SerializeField] private bool isWallJumping;

    [Header("Collision")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private LayerMask groundCheck;
    [Space]
    [SerializeField] private Transform enemyDetection;
    [SerializeField] private float enemyCheckRadius;
    [SerializeField] private LayerMask enemyCheck;
    private bool isGround;
    private bool isAirborne;
    private bool isWall;

    [Header("Knockback")]
    [SerializeField] private float knockBackDuration = 1;
    [SerializeField] private Vector2 knockBackPower;
    [SerializeField] private bool isKnocked;

    [Header("Slip")]
    private bool facingRight = true;
    private int facingDirection = 1;

    [Header("Player Visual")]
    [SerializeField] private AnimatorOverrideController[] animatorOverrideControllers;
    [SerializeField] private int skinId;
    [SerializeField] private GameObject deathVFX;


    [Header("Others")]
    [SerializeField] private bool canBeControlled = false;

    private float defaultGravityScale;

    //Hàm Awake sẽ kích hoạt để khởi tạo các biến trước khi ứng dụng chạy
    private void Awake()
    {
        //Gán component của nhân vật vào biến tương ứng
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponentInChildren<Animator>();
        playerCapsuleCollider = GetComponent<CapsuleCollider2D>();
        playerInputAction = new PlayerInputAction();
    }

    private void OnEnable()
    {
        playerInputAction.Enable();

        playerInputAction.Player.Jump.performed += ctx => JumpController();
        playerInputAction.Player.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        playerInputAction.Player.Movement.canceled += ctx => moveInput = Vector2.zero;
    }

    private void OnDisable()
    {
        playerInputAction.Disable();

        playerInputAction.Player.Jump.performed -= ctx => JumpController();
        playerInputAction.Player.Movement.performed -= ctx => moveInput = ctx.ReadValue<Vector2>();
        playerInputAction.Player.Movement.canceled -= ctx => moveInput = Vector2.zero;
    }

    //Hàm Start sẽ kích hoạt trước khi bắt đầu frame đầu tiên
    void Start()
    {
        //lấy gravity ban đầu làm mặc định
        defaultGravityScale = playerRigidbody.gravityScale;
        gameManagerController = GameManagerController.instance;

        UpdateGameDifficulty();

        //thiết lập trạng tháy respawn ban đầu là false tức là chưa respawn
        RespawnFinished(false);

        UpdateSkinController();
    }


    //Hàm Update sẽ kích hoạt với mỗi frame xuất hiện
    void Update()
    {
        UpdateAirborneStatus();

        if (canBeControlled == false) 
        {
            AnimationController();
            CollisionController();
            return;
        };
        if (isKnocked) return;

        EnemyDetectionController();
        //InputController();
        FlipController();
        WallSlideController();
        MovementController();
        CollisionController();
        AnimationController();
    }
    private void UpdateGameDifficulty()
    {
        DifficultyManagerController difficultyManagerController = DifficultyManagerController.instance;

        if (difficultyManagerController != null)
            gameDifficulty = difficultyManagerController.difficulty;
    }
    public void Damage()
    {
        if (gameDifficulty == DifficultyType.Medium)
        {

            if (gameManagerController.CollectedFruits() <= 0)
            {
                Die();
                gameManagerController.RestartLevel();
            }
            else
                gameManagerController.RemoveFruit();

            return;
        }

        if (gameDifficulty == DifficultyType.Hard)
        {
            Die();
            gameManagerController.RestartLevel();
        }
    }
    public void UpdateSkinController()
    {
        SkinManagerController skinManager = SkinManagerController.instance;

        if (skinManager == null)
            return;

        playerAnimator.runtimeAnimatorController = animatorOverrideControllers[skinManager.chosenSkinId];
    }

    private void EnemyDetectionController()
    {
        if (playerRigidbody.linearVelocity.y >= 0)
            return;

        //OverlapCircleAll sẽ trả về mảng các collider2d mà tiếp xúc trong bán kính hình tròn
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemyDetection.position, enemyCheckRadius, enemyCheck);

        //lặp qua các phần tử trong mảng collisers và lấy game object có component EnemyController 
        foreach(var enemy in colliders)
        {
            EnemyController newEnemy = enemy.GetComponent<EnemyController>();

            //nếu như có tồn tại thì kích hoạt hàm DeathController
            if(newEnemy != null)
            {
                newEnemy.DeathController();
                JumpController();
            }
        }
    }

    public void PushController(Vector2 direction, float duration)
    {
        StartCoroutine(PushCouroutine(direction, duration));
    }
    private IEnumerator PushCouroutine(Vector2 direction, float duration)
    {
        canBeControlled = false;
        //vận tốc nhân vật lúc này sẽ là 1 vector2(0,0) nhân vật không thể chuyển động
        playerRigidbody.linearVelocity = Vector2.zero;

        //Dùng AddForce để tạo 1 xung lực là ForceMode2D.Impulse vào nhân vật với hướng là direction
        playerRigidbody.AddForce(direction, ForceMode2D.Impulse);

        yield return new WaitForSeconds(duration);

        canBeControlled = true;
    }
    //Hàm kiểm tra xem nhân vật đã hoàn thành respawn chưa
    public void RespawnFinished(bool finished)
    {
        //nếu rồi thì chỉnh gravity về mặt định,kích hoạt collider của nhân vật và thiết lập có thể điều khiên nhân vật
        if (finished)
        {
            playerRigidbody.gravityScale = defaultGravityScale;
            canBeControlled = true;
            playerCapsuleCollider.enabled = true;
        }
        //nếu chưa thì ngược lại cái trên
        else{
            playerRigidbody.gravityScale = 0;
            canBeControlled = false;
            playerCapsuleCollider.enabled = false;
        }
    }

    public void Die()
    {
        //tạo ra 1 bản sao của hiệu ứng chết
        GameObject newDeathVFX = Instantiate(deathVFX, transform.position, Quaternion.identity);
        //phá hủy nhân vật
        Destroy(gameObject);

        //phá hủy bản sao sau .5f
        Destroy(newDeathVFX, .5f);
    }

    private void UpdateAirborneStatus()
    {
        if (isGround && isAirborne)
            LandingController();
        if(!isAirborne && !isGround)
            BecomeAirborne();
    }

    private void BecomeAirborne()
    {
        isAirborne = true;

        if (playerRigidbody.linearVelocity.y < 0)
            ActiveCoyoteJump();
    }

    private void LandingController()
    {
        isAirborne = false;
        canDoubleJump = true;
        AttemptBufferJump();
    }

    //private void InputController()
    //{
    //    //Sử dụng Input.GetAxis("Horizontal") để lấy giá trị trả về từ trục ngang của bàn phím và gán vào biến xInput
    //    xInput = Input.GetAxis("Horizontal");
    //    yInput = Input.GetAxis("Vertical");


    //    //Sử dụng Input.GetKeyDown(KeyCode.Space) để kích hoạt hành động nhảy khi user bấm nút space
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        JumpController();
    //        ActiveBufferJump();
    //    }
    //}

    private void ActiveBufferJump()
    {
        if(isAirborne)
            bufferJumpActivated = Time.time;
    }

    private void AttemptBufferJump()
    {
        if(Time.time < bufferJumpActivated + bufferJumpWindow)
        {
            bufferJumpActivated = Time.time - 1;
            playerRigidbody.linearVelocity = new Vector2(playerRigidbody.linearVelocity.x, jumpForce);
        }
    }

    private void ActiveCoyoteJump()
    {
        coyoteJumpActivated = Time.time;
    }

    private void CancelCoyoteJump()
    {
        coyoteJumpActivated = Time.time - 1;
    }

    private void AnimationController()
    {
        playerAnimator.SetFloat("xVelocity", playerRigidbody.linearVelocity.x);
        playerAnimator.SetFloat("yVelocity", playerRigidbody.linearVelocity.y);
        playerAnimator.SetBool("isGround", isGround);
        playerAnimator.SetBool("isWall", isWall);
    }

    public void KnockBackController(float damageSourceXPosition)
    {
        float knockbackDirection = 1;

        if (transform.position.x < damageSourceXPosition)
            knockbackDirection = -1;

        if (isKnocked) return;

        StartCoroutine(KnockBackRoutine());

        playerAnimator.SetTrigger("isKnocked");

        playerRigidbody.linearVelocity = new Vector2(knockBackPower.x * knockbackDirection, knockBackPower.y);

        CameraManagerController.instance.CameraShake();

    }

    private IEnumerator KnockBackRoutine()
    {
        isKnocked = true;

        yield return new WaitForSeconds(knockBackDuration);

        isKnocked = false;
    }

    private void JumpController()
    {
        bool coyoteJumpAvailable = Time.time < coyoteJumpActivated + coyoteJumpWindow;
        //nếu như nhân vật tiếp xúc với mặt đất thì mới cho phép nhảy để tránh việc spam nhảy liên tục
        if (isGround || coyoteJumpAvailable)
        {
            playerRigidbody.linearVelocity = new Vector2(playerRigidbody.linearVelocity.x, jumpForce);
        }
        //nếu như nhân vật tiếp xúc với tường và không ở trên mặt đất
        else if(isWall && !isGround)
        {
            WallJumpController();
        }
        else if (canDoubleJump)
        {
            DoubleJumpController();
        }

        CancelCoyoteJump();
    }

    private void DoubleJumpController()
    {
        StopCoroutine(WallJumRountine());
        isWallJumping = false;
        canDoubleJump = false;
        playerRigidbody.linearVelocity = new Vector2(playerRigidbody.linearVelocity.x, doubleJumpForce);
    }

    private void FlipController()
    {
        //nhân vật sẽ xoay theo điều kiện dưới đây
        if (moveInput.x < 0 && facingRight || moveInput.x > 0 && !facingRight)
        {
            Flip();
        }
    }

    private void Flip()
    {
        //thay đổi biến facingDirection khi thay đổi hướng nhân vật để dùng cho việc đổi hướng cùng nhân vật Raycast và Gizmos sau này
        facingDirection = facingDirection * -1;
        transform.Rotate(0, 180, 0);
        facingRight = !facingRight;
    }

    private void WallJumpController()
    {
        canDoubleJump = true;
        playerRigidbody.linearVelocity = new Vector2(wallJumpForce.x * -facingDirection, wallJumpForce.y);

        Flip();

        //Dừng tất cả các coroutine đang chạy trước đó
        StopAllCoroutines();

        // Bắt đầu một coroutine mới gọi là WallJumRountine() để xử lý hành động nhảy trên tường trong một khoảng thời gian nhất định
        StartCoroutine(WallJumRountine());
    }

    private IEnumerator WallJumRountine()
    {
        isWallJumping = true;
        //Dừng coroutine trong một khoảng thời gian được chỉ định, sau đó sẽ tiếp tục thực hiện các lệnh phía sau
        yield return new WaitForSeconds(wallJumpDuration);
        isWallJumping = false;
    }

    private void WallSlideController()
    {
        bool canWallSlide = isWall && playerRigidbody.linearVelocity.y < 0;
        //biến yModifier để nhân vật trượt xuống nhanh hơn 
        float yModifer = moveInput.y < 0 ? 1 : .05f;

        if (canWallSlide == false) return;

        playerRigidbody.linearVelocity = new Vector2(playerRigidbody.linearVelocity.x, playerRigidbody.linearVelocity.y * yModifer);
    }

    private void MovementController()
    {
        if (isWall) return;
        if (isWallJumping) return;
        if (isKnocked) return;

        //Gán vector2 mới vào vận tốc của nhân vật và lấy xInput * moveSpeed để tính toán tốc độ di chuyển của nhân vật
        playerRigidbody.linearVelocity = new Vector2(moveInput.x * moveSpeed, playerRigidbody.linearVelocity.y);
    }

    private void CollisionController()
    {
        //Dùng Physics2D.Raycast để thực hiện kiểm tra va chạm (collision detection) dựa trên tia (ray) trong không gian 2D
        isGround = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundCheck);
        isWall = Physics2D.Raycast(transform.position, Vector2.right * facingDirection, wallCheckDistance, groundCheck);
    }

    //vẽ đường gizmos để nó hiển thị song song với Raycast phía trên bởi vì Raycast không thể thấy được
    private void OnDrawGizmos()
    {
        //dùng gizmos vẽ 1 đường thẳng để kết hợp với Raycast kiểm tra việc tiếp xúc với ground của nhân vật
        Gizmos.DrawWireSphere(enemyDetection.position, enemyCheckRadius);
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - groundCheckDistance));
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + (wallCheckDistance * facingDirection), transform.position.y));
    }
}
