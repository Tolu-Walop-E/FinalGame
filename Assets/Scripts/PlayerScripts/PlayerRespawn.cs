using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    Animator animator;
    public Transform[] checkpoints; // Array of checkpoints
    private Vector3 lastCheckpoint; // Player's last checkpoint
    private bool hasCheckpoint = false; // Track if a checkpoint has been reached


    private void Start()
    {
        animator = GetComponent<Animator>();
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

    public void Respawn()
    {
        // Move the player to the last checkpoint
        //reset all animations
        if (animator.GetBool("animateFalling"))
        {
            animator.SetBool("animateClimbing", false);
            animator.ResetTrigger("animateJumping");
            animator.ResetTrigger("animateDoubleJumping");
            animator.SetBool("animateFalling", false);
            animator.SetTrigger("animateLanding");
        }

        if (hasCheckpoint)
        {

            transform.position = lastCheckpoint;
            Debug.Log("Player respawned at checkpoint: " + lastCheckpoint);
        }
        else
        {
            Debug.LogError("No valid checkpoint set. Ensure a default checkpoint is initialized.");
        }
    }
}
