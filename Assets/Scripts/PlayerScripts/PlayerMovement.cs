using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Vector2 moveValue;
    public float jumpSpeed = 5f;
    public float speed;
    private int maxJumps = 2;
    public int jumpCount;
    private Vector3 movementCheck;
    private Rigidbody rb;
    
    private Collider playerCollider;

   
    // Start is called before the first frame update
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
        if (jumpCount < maxJumps)
        {
            rb.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
            jumpCount++;
        }
    }

    void FixedUpdate()
    {
        

        movementCheck.x = Input.GetAxisRaw("Horizontal");
        movementCheck.y = 0f;
        movementCheck.z = 0f;

        if (movementCheck.x != 0)
        {
            RotatePlayer(movementCheck.x);
        }

        Vector3 movement = new Vector3(moveValue.x, 0.0f, 0.0f);  // Ensure Z movement is 0
        Vector3 newPosition = rb.position + (movement * speed * Time.fixedDeltaTime);
        rb.MovePosition(newPosition);
    }

    void Update()
    {
      
    }

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

    // Collision-based respawn
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