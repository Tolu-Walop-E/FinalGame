using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    public Transform[] checkpoints; // Array of checkpoints
    private Vector3 lastCheckpoint; // Player's last checkpoint
    private bool hasCheckpoint = false; // Track if a checkpoint has been reached


    private void Start()
    {
        // Initialize the first checkpoint as the default if available
        if (checkpoints != null && checkpoints.Length > 0)
        {
            lastCheckpoint = checkpoints[0].position;
            hasCheckpoint = true;
            Debug.Log("Default checkpoint set to: " + lastCheckpoint);
        }
        else
        {
            Debug.LogWarning("No checkpoints assigned.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Checkpoint logic
        if (other.CompareTag("Checkpoint"))
        {
            lastCheckpoint = transform.position;
            hasCheckpoint = true;
            Debug.Log("Checkpoint reached at: " + lastCheckpoint);
        }
        else if (other.CompareTag("Respawn"))
        {
            Respawn();
        }
    }

    private void Respawn()
    {
        if (hasCheckpoint)
        {
            // Move the player to the last checkpoint
            transform.position = lastCheckpoint;
            Debug.Log("Player respawned at checkpoint: " + lastCheckpoint);
        }
        else
        {
            Debug.LogError("No valid checkpoint set. Ensure a default checkpoint is initialized.");
        }
    }
}
