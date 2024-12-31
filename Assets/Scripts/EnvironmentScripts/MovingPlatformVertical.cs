using UnityEngine;

public class MovingPlatformVertical : MonoBehaviour
{
    public float upSpeed = 5f; // Speed of upward movement
    public float downSpeed = 5f; // Speed of downward movement
    public float distance = 3f; // Distance the platform moves up and down

    private bool movingDown = true; // Whether the platform is moving down
    private Vector3 startPosition; // Starting position of the platform

    private void Start()
    {
        startPosition = transform.position; // Record the starting position
    }

    private void Update()
    {
        MovePlatform(); // Handle the movement of the platform
    }

    private void MovePlatform()
    {
        if (movingDown)
        {
            // Move the platform downward
            transform.Translate(Vector3.down * downSpeed * Time.deltaTime);

            // Reverse direction if it reaches the lower limit
            if (transform.position.y <= startPosition.y - distance)
            {
                movingDown = false;
            }
        }
        else
        {
            // Move the platform upward
            transform.Translate(Vector3.up * upSpeed * Time.deltaTime);

            // Reverse direction if it reaches the upper limit
            if (transform.position.y >= startPosition.y + distance)
            {
                movingDown = true;
            }
        }
    }


}
