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

    // Start is called before the first frame update
    void Start()
    {
        jumpCount = 0;
        rb = GetComponent<Rigidbody>();
        playerCollider = GetComponent<Collider>();
    }

    // Handle movement input
    public void OnMove(InputValue value)
    {
        moveValue = value.Get<Vector2>();
    }

    // Handle jump input
    public void OnJump(InputValue value)
    {
        if (jumpCount < maxJumps)
        {
            rb.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
            jumpCount++;
        }
    }

    // FixedUpdate is used for physics-based movement
    void FixedUpdate()
    {
        // Restrict movement to X-axis only (Z will stay at 0)
        Vector3 movement = new Vector3(moveValue.x * speed, rb.velocity.y, 0f);  // Zero out Z-axis
        rb.velocity = movement; // Apply movement to Rigidbody velocity

        // Ensure rotation happens in FixedUpdate
        RotatePlayer(moveValue.x);
    }

    // Keep Z position locked to 0
    void Update()
    {
        // Ensures that the Z position is locked to 0, without overriding physics-based movements
        Vector3 position = transform.position;
        position.z = 0f; // Lock Z-axis position to 0
        transform.position = position; // Apply position correction
    }

    // Rotate player based on movement direction (left or right)
    void RotatePlayer(float directionX)
    {
        if (directionX > 0)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);  // Face right
        }
        else if (directionX < 0)
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);  // Face left
        }
    }

    // Handle collision-based respawn or state update when landing on platform
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            jumpCount = 0; // Reset jump count on landing
        }
    }

    // Trigger-based respawn (if Respawn is a trigger)
    void OnTriggerEnter(Collider other)
    {
    }
}
