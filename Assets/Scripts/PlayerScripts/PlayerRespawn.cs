using UnityEngine;

public class PlayerCheckpoint : MonoBehaviour
{
    public Transform[] checkpoints; // Reference to your checkpoint objects
    private Vector3 lastCheckpoint; // The position of the last checkpoint the player has reached
    private bool hasCheckpoint = false; // To track if a checkpoint has been reached

    // Called when the player touches a checkpoint
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the player collides with a checkpoint object
        if (collision.gameObject.CompareTag("Checkpoint"))
        {
            // Save the player's position as the checkpoint position
            lastCheckpoint = transform.position;
            hasCheckpoint = true; // Mark that the player has reached a checkpoint
            Debug.Log("Checkpoint reached at: " + lastCheckpoint);
        }

        // Check if the player collides with the ground
        if (collision.gameObject.CompareTag("Respawn"))
        {
            // If the player falls and touches the ground, respawn them at the last checkpoint
            Respawn();
        }
    }

    // Respawn the player at the last checkpoint
    private void Respawn()
    {
        if (hasCheckpoint)
        {
            transform.position = lastCheckpoint; // Move the player to the last checkpoint position
            Debug.Log("Player respawned at checkpoint: " + lastCheckpoint);
        }
        else
        {
            Debug.Log("No checkpoint reached yet.");
        }
    }
}
