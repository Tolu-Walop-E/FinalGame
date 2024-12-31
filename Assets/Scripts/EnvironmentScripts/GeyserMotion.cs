using UnityEngine;

public class GeyserMotion : MonoBehaviour
{
    public ParticleSystem particleSystem; // Reference to the particle system
    public float activeDuration = 5f; // Duration the particle system is active
    public float breakDuration = 6f; // Duration the particle system is inactive
    public float upwardForce = 10f; // Force applied to the player

    private bool isActive = false; // Whether the particle system is currently active

    private void Start()
    {
        if (particleSystem == null)
        {
            Debug.LogError("Particle system not assigned.");
            return;
        }

        StartCoroutine(ControlParticleSystem());
    }

    private System.Collections.IEnumerator ControlParticleSystem()
    {
        while (true)
        {
            // Activate the particle system
            isActive = true;
            particleSystem.Play();
            Debug.Log("Particle system activated.");
            yield return new WaitForSeconds(activeDuration);

            // Deactivate the particle system
            isActive = false;
            particleSystem.Stop();
            Debug.Log("Particle system deactivated.");
            yield return new WaitForSeconds(breakDuration);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (isActive && other.CompareTag("Player"))
        {
            Rigidbody playerRigidbody = other.GetComponent<Rigidbody>();
            if (playerRigidbody != null)
            {
                // Apply upward force to the player
                playerRigidbody.AddForce(Vector3.up * upwardForce, ForceMode.VelocityChange);
                Debug.Log("Player boosted by particle system.");
            }
        }
    }
}
