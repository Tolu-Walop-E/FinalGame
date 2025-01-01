using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float leftSpeed = 5f;
    public float rightSpeed = 5f;
    public float distance = 8f;

    private bool movingRight = true;
    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position; // Record the starting position
    }

    private void Update()
    {
        MoveCube(); // Handle the movement of the platform
    }

    private void MoveCube()
    {
        // Move the platform left or right
        if (movingRight)
        {
            transform.Translate(Vector3.right * rightSpeed * Time.deltaTime);
            if (transform.position.x >= startPosition.x + distance)
            {
                movingRight = false; // Reverse direction when reaching the right limit
            }
        }
        else
        {
            transform.Translate(Vector3.left * leftSpeed * Time.deltaTime);
            if (transform.position.x <= startPosition.x - distance)
            {
                movingRight = true; // Reverse direction when reaching the left limit
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Check if the colliding object is the Player
        {
            Debug.Log("Player entered platform."); // Debugging log
            other.transform.SetParent(this.transform); // Parent the player to the platform
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // Check if the exiting object is the Player
        {
            Debug.Log("Player exited platform."); // Debugging log
            other.transform.SetParent(null); // Unparent the player from the platform
        }
    }
}
