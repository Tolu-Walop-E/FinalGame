using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Vector2 moveValue;
    public float jumpSpeed = 5f;
    public float speed;
    public float walkingSFXSpeed = 0.8f; // SFX speed for walking
    public float runningSFXSpeed = 1f; // SFX speed for running
    private int maxJumps = 2;
    public int jumpCount;
    private Rigidbody rb;
    private Collider playerCollider;

    private bool onIcePlatform = false;
    public float iceSpeedMultiplier = 1.5f; // Speed boost on ice
    public float iceFriction = 0.98f; // Friction for sliding on ice
    public float stopThreshold = 0.1f; // Threshold to stop sliding completely

    // Wall Sliding Variables
    public float wallSlideSpeed = 2f; // Speed of sliding down the wall
    private bool isTouchingWall = false; // Check if the player is touching a wall
    private bool isWallSliding = false;
    private Animator animator;
    // Wall Jump Variables
    public float wallJumpXForce = 5f; // Force applied on the X-axis during a wall jump
    public float wallJumpYForce = 7f; // Force applied on the Y-axis during a wall jump
    public LayerMask climbableLayer; // LayerMask for climbable platforms

    // Ladder Climbing Variables
    public float climbSpeed = 5f; // Speed of climbing
    private bool isClimbing = false; // Is the player currently climbing
    private bool isOnLadder = false; // Is the player on a ladder

    // Fall Damage Variables
    private PlayerRespawn respawnManager; // Reference to the respawn manager
    public float maxHealth = 100f; // Maximum health for the player
    private float currentHealth;  // Current health of the player
    private float fallStartHeight; // Height from where the fall starts
    private bool isFalling = false; // Is the player currently falling?
    public float fallDamageThreshold = 10f; // Height difference to trigger fall damage

    private bool isWalking = false;
    private bool isRunning = false;

    private GameAudioManager audioManager;
    [SerializeField] private AudioClip walkingSound;
    void Start()
    {
        jumpCount = 0;
        rb = GetComponent<Rigidbody>();
        playerCollider = GetComponent<Collider>();
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();

        respawnManager = GetComponent<PlayerRespawn>();
        if (respawnManager == null)
        {
            Debug.LogError("PlayerRespawn script not found on the player!");
        }

        GameObject audioManagerObject = GameObject.FindGameObjectWithTag("Audio");
        if (audioManagerObject != null)
        {
            audioManager = audioManagerObject.GetComponent<GameAudioManager>();
        }
        else
        {
            Debug.LogError("AudioManager not found! Ensure an object with the 'AudioManager' tag exists.");
        }
    }
    private void UpdateMovementStates()
    {
        if (animator != null)
        {
            isWalking = animator.GetBool("isWalking");
            isRunning = animator.GetBool("isRunning");

            Debug.Log($"isWalking: {isWalking}, isRunning: {isRunning}");
            HandleSFXSpeed();
        }
    }
    private void HandleSFXSpeed()
    {
        if (audioManager != null)
        {
            if (isWalking && !isRunning)
            {
                audioManager.SetSFXSpeed(walkingSFXSpeed);
            }
            else if (isRunning)
            {
                audioManager.SetSFXSpeed(runningSFXSpeed);
            }
        }
    }

    public void OnMove(InputValue value)
    {
        moveValue = value.Get<Vector2>();

        if (moveValue.x != 0)
        {
            if (audioManager != null && walkingSound != null)
            {
                HandleSFXSpeed();
                audioManager.PlaySFXLoop(walkingSound);
            }
        }
        else
        {
            if (audioManager != null)
            {
                audioManager.StopSFX();
            }
        }
    }


    public void OnJump(InputValue value)
    {
        if (isWallSliding)
        {
            PerformWallJump();
        }
        else if (isClimbing)
        {
            // Climbing does not use jumps
            return;
        }
        else if (jumpCount < maxJumps)
        {
            rb.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
            jumpCount++;
        }
    }

    void FixedUpdate()
    {
        if (isClimbing)
        {
            HandleClimbing();
        }
        else if (onIcePlatform)
        {
            HandleIceMovement(); // Handle ice platform movement
        }
        else
        {
            // Regular movement on ground
            Vector3 movement = new Vector3(moveValue.x * speed, rb.velocity.y, 0f);
            rb.velocity = movement;

            RotatePlayer(moveValue.x);
            HandleWallSlide();
            UpdateMovementStates();
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log($"Player took {damage} damage. Current health: {currentHealth}");

        if (currentHealth <= 0f)
        {
            respawnManager.Respawn();
        }
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

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ladder"))
        {
            isOnLadder = true;
            rb.useGravity = false; // Disable gravity when entering the ladder
            rb.velocity = Vector3.zero; // Reset velocity for smoother climbing
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ladder"))
        {
            isOnLadder = false;
            isClimbing = false;
            rb.useGravity = true; // Re-enable gravity when leaving the ladder
        }
    }

    private void HandleClimbing()
    {
        if (isOnLadder) // Only climb if the player is on a ladder
        {
            float verticalInput = moveValue.y;

            // Move vertically based on input
            Vector3 climbingVelocity = new Vector3(0, verticalInput * climbSpeed, 0);
            rb.velocity = climbingVelocity;

            // Ensure player doesn't fall while climbing
            rb.useGravity = false;

            if (Mathf.Abs(verticalInput) > 0.1f)
            {
                isClimbing = true;
            }
            else
            {
                isClimbing = false;
                rb.velocity = Vector3.zero; // Stop movement when no input
            }
        }
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            jumpCount = 0; // Reset jump count on landing

            // Check for fall damage
            if (isFalling)
            {
                float fallDistance = fallStartHeight - transform.position.y; // Calculate fall distance
                if (fallDistance > fallDamageThreshold)
                {
                    respawnManager.Respawn(); // Respawn if fall damage exceeds threshold
                }
                isFalling = false; // Reset falling state
            }

            // Check if the platform is climbable
            if (IsClimbable(collision.gameObject))
            {
                isTouchingWall = true;
                Debug.Log("Touching climbable wall.");
            }
            if (collision.gameObject.layer == LayerMask.NameToLayer("IcePlatforms"))
            {
                onIcePlatform = true;
                rb.velocity = new Vector3(rb.velocity.x * iceSpeedMultiplier, rb.velocity.y, rb.velocity.z); // Boost initial velocity
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
        if (collision.gameObject.layer == LayerMask.NameToLayer("IcePlatforms"))
        {
            onIcePlatform = false;
        }

        // Start tracking fall
        if (collision.gameObject.CompareTag("Platform"))
        {
            fallStartHeight = transform.position.y; // Record height at start of fall
            isFalling = true; // Mark as falling
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

    private void HandleIceMovement()
    {
        bool isPressingMoveKeys = moveValue.x != 0;

        if (isPressingMoveKeys)
        {
            // If the player is pressing movement keys, apply regular movement with boosted speed
            Vector3 movement = new Vector3(moveValue.x * speed * iceSpeedMultiplier, rb.velocity.y, 0f);
            rb.velocity = movement;
        }
        else
        {
            // Apply sliding when no movement keys are pressed
            rb.velocity = new Vector3(rb.velocity.x * iceFriction, rb.velocity.y, 0f);

            // Stop completely if the velocity is near zero
            if (Mathf.Abs(rb.velocity.x) < stopThreshold)
            {
                rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
            }
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
