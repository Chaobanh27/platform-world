using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FallingPlatformTrapController : MonoBehaviour
{
    private Animator fallingPlatformAnimator;
    private Rigidbody2D fallingPlatformRigidbody;
    private BoxCollider2D[] fallingPlatformCollider;

    [Header("Platform movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float travelDistance;
    private Vector3[] wayPoint;
    private int wayPointIndex;
    private bool canMove;

    [Header("Platform Fall")]
    [SerializeField] private float collisionSpeed;
    [SerializeField] private float collisionDuration;
    private float collisionTimer;
    private bool isCollision;
    [SerializeField] private float fallDelay;

    private void Awake()
    {
        fallingPlatformAnimator = GetComponent<Animator>();
        fallingPlatformRigidbody = GetComponent<Rigidbody2D>();
        fallingPlatformCollider = GetComponents<BoxCollider2D>();
    }

    // Start is called before the first frame update
    private IEnumerator Start()
    {
        WayPointController();

        float randomDelay = Random.Range(0, 0.6f);

        yield return new WaitForSeconds(randomDelay);

        canMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        CollisionController();
        MovementController();
    }

    //thiết lập waypoint để platform di chuyển lên xuống
    private void WayPointController()
    {
        wayPoint = new Vector3[2];

        float yOffset = travelDistance / 2;

        wayPoint[0] = transform.position + new Vector3 (0, yOffset, 0);
        wayPoint[1] = transform.position + new Vector3 (0, -yOffset, 0);
    }

    //tạo logic di chuyển lên xuống vô tận cho platform
    private void MovementController()
    {
        if (canMove == false) return;

        transform.position = Vector2.MoveTowards(transform.position, wayPoint[wayPointIndex], moveSpeed * Time.deltaTime);

        float wayPointsDistance = Vector2.Distance(transform.position, wayPoint[wayPointIndex]);

        if(wayPointsDistance == 0) 
        {
            wayPointIndex++;

            if(wayPointIndex >= wayPoint.Length) 
            {
                wayPointIndex = 0;
            }
        }

    }

    private void CollisionController()
    {
        if (collisionTimer < 0) return;

        collisionTimer = collisionTimer - Time.deltaTime;

        transform.position = Vector2.MoveTowards(transform.position, transform.position + (Vector3.down * 10), collisionSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isCollision) return;

        PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();

        if(playerController == null) return;
        if(playerController != null)
        {
            Invoke("SwitchOffPlatform", fallDelay);
            collisionTimer = collisionDuration;
            isCollision = true;
        }

    }

    private void SwitchOffPlatform()
    {
        fallingPlatformAnimator.SetTrigger("isIdle");

        canMove = false;

        fallingPlatformRigidbody.isKinematic = false;
        fallingPlatformRigidbody.gravityScale = 3.5f;
        fallingPlatformRigidbody.linearDamping = .5f;

        foreach (BoxCollider2D collider in fallingPlatformCollider)
        {
            collider.enabled = false;
        }
    }
}
