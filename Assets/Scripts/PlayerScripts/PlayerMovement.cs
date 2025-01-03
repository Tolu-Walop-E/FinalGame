using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Vector2 moveValue;
    public float jumpSpeed = 5f;
    public float speed;
    private int maxJumps = 2;
    public int jumpCount;
    private Rigidbody rb;
    private Collider playerCollider;

    // Wall Sliding Variables
    public float wallSlideSpeed = 2f; // Speed of sliding down the wall
    private bool isTouchingWall = false; // Check if the player is touching a wall
    private bool isWallSliding = false;

    // Wall Jump Variables
    public float wallJumpXForce = 5f; // Force applied on the X-axis during a wall jump
    public float wallJumpYForce = 7f; // Force applied on the Y-axis during a wall jump
    public LayerMask climbableLayer; // LayerMask for climbable platforms

    void Start()
    {
        jumpCount = 0;
        rb = GetComponent<Rigidbody>();
        playerCollider = GetComponent<Collider>();
    }

    public void OnMove(InputValue value)
    {
        moveValue = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        if (isWallSliding)
        {
            PerformWallJump();
        }
        else if (jumpCount < maxJumps)
        {
            rb.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
            jumpCount++;
        }
    }

    void FixedUpdate()
    {
        // Regular movement on ground
        Vector3 movement = new Vector3(moveValue.x * speed, rb.velocity.y, 0f);
        rb.velocity = movement;

        RotatePlayer(moveValue.x);
        HandleWallSlide();
    }

    void Update()
    {
        Vector3 position = transform.position;
        position.z = 0f;
        transform.position = position;
    }

    void RotatePlayer(float directionX)
    {
        if (directionX > 0)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else if (directionX < 0)
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            jumpCount = 0; // Reset jump count on landing

            // Check if the platform is climbable
            if (IsClimbable(collision.gameObject))
            {
                isTouchingWall = true;
                Debug.Log("Touching climbable wall.");
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Platform") && IsClimbable(collision.gameObject))
        {
            isTouchingWall = false;
            isWallSliding = false;
            Debug.Log("Left climbable wall.");
        }
    }

    private void HandleWallSlide()
    {
        if (isTouchingWall && rb.velocity.y < 0)
        {
            isWallSliding = true;
            rb.velocity = new Vector3(rb.velocity.x, -wallSlideSpeed, rb.velocity.z);
            Debug.Log("Wall sliding...");
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void PerformWallJump()
    {
        if (isWallSliding)
        {
            Debug.Log("Performing wall jump...");
            // Jump off the wall in the opposite direction
            float jumpDirection = transform.rotation.eulerAngles.y == 0 ? -1 : 1; // Determine direction based on player facing
            rb.velocity = new Vector3(wallJumpXForce * jumpDirection, wallJumpYForce, 0f);
            isWallSliding = false;
            jumpCount = 1; // Use one jump after wall jump
        }
    }

    private bool IsClimbable(GameObject platform)
    {
        // Check if the platform is in the Climbable layer
        return ((1 << platform.layer) & climbableLayer) != 0;
    }
}
