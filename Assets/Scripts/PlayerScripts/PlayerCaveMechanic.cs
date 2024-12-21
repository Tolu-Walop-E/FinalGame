using UnityEngine;

public class PlayerCaveMechanic : MonoBehaviour
{
    public Light PlayerPoint; // Assign your PlayerPoint light in the Inspector

    private void Start()
    {
        // Ensure the light is off at the start
        if (PlayerPoint != null)
        {
            PlayerPoint.enabled = false;
        }
        else
        {
            Debug.LogError("PlayerPoint light is not assigned in the Inspector!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player collides with the trigger
        if (other.CompareTag("Player") && PlayerPoint != null)
        {
            PlayerPoint.enabled = true; // Turn on the PlayerPoint light
        }
    }
}
