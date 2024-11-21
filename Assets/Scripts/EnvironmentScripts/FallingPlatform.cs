using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    public float fallSpeed = 5f; // Speed at which the cube falls
    private bool isFalling = false; // To track if the cube has started falling
    public int damage = 50;

    void OnTriggerEnter(Collider other)
    {
        // Check if the object that entered the trigger has the "Player" tag
        if (other.CompareTag("Player"))
        {
            isFalling = true; // Start falling if the player is underneath
        }
    }

    void Update()
    {
        // If the cube is falling, move it down
        if (isFalling)
        {
            Fall();

        }
    }

    void Fall()
    {
        // Move the cube downward
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the plank hits the player
        if (collision.gameObject.CompareTag("Player"))
        {


                Destroy(gameObject);
            
        }
    }
}
