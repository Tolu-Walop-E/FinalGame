using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    public Transform[] checkpoints; // Array of checkpoints
    private Vector3 lastCheckpoint; // Player's last checkpoint
    private bool hasCheckpoint = false; // Track if a checkpoint has been reached
    private GameAudioManager audioManager; // Reference to the audio manager

    private void Start()
    {
        // Initialize the audio manager
        GameObject audioManagerObject = GameObject.FindGameObjectWithTag("Audio");
        if (audioManagerObject != null)
        {
            audioManager = audioManagerObject.GetComponent<GameAudioManager>();
        }
        else
        {
            Debug.LogError("AudioManager not found! Ensure an object with the 'AudioManager' tag exists.");
        }

        // Initialize the first checkpoint
        if (checkpoints == null || checkpoints.Length == 0)
        {
            Debug.LogError("Checkpoints are not assigned! Ensure at least one checkpoint is set in the inspector.");
            return;
        }

        lastCheckpoint = checkpoints[0].position;
        hasCheckpoint = true;
        Debug.Log("Default checkpoint set to: " + lastCheckpoint);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Checkpoint"))
        {
            lastCheckpoint = transform.position;
            hasCheckpoint = true;
            Debug.Log("Checkpoint reached at: " + lastCheckpoint);

            if (audioManager != null)
            {
                audioManager.PlaySFX(audioManager.checkpoint); // Play sound for reaching a checkpoint
            }
        }
        else if (other.CompareTag("Respawn"))
        {
            Respawn();
        }
    }

    public void Respawn()
    {
        if (hasCheckpoint)
        {
            if (audioManager != null)
            {
                audioManager.PlaySFX(audioManager.checkpoint); // Play respawn sound
            }

            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.zero; // Reset velocity to prevent unintended physics effects
            }

            transform.position = lastCheckpoint;
            Debug.Log("Player respawned at checkpoint: " + lastCheckpoint);
        }
        else
        {
            Debug.LogError("No valid checkpoint set. Ensure a default checkpoint is initialized.");
        }
    }
}
