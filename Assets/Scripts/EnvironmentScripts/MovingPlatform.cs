using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float leftSpeed = 5f;
    public float rightSpeed = 5f;
    public float distance = 8f;

    private bool movingRight = true;
    private Vector3 startPosition;

    void Start()
    {

        startPosition = transform.position;
    }

    void Update()
    {
        MoveCube();
    }

    void MoveCube()
    {

        if (movingRight)
        {
            transform.Translate(Vector3.right * rightSpeed * Time.deltaTime);


            if (transform.position.x >= startPosition.x + distance)
            {
                movingRight = false;
            }
        }
        else
        {
            transform.Translate(Vector3.left * leftSpeed * Time.deltaTime);


            if (transform.position.x <= startPosition.x - distance)
            {
                movingRight = true;
            }
        }
    }
}