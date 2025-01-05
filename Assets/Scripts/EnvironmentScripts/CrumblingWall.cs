using UnityEngine;

public class CrumblingWall : MonoBehaviour
{
    public GameObject brokenCubePrefab; // Assign the small cube prefab in the Inspector
    public int cubeCount = 10;          // Number of cubes the wall breaks into
    public Vector3 cubeScale = new Vector3(0.5f, 0.5f, 0.5f); // Size of the broken cubes
    public float explosionForce = 500f; // Explosion force to apply on the broken cubes
    public float destroyDelay = 5f;     // Delay time before destroying the cubes
    private GameAudioManager audioManager;

    private void Awake()
    {
        GameObject audioManagerObject = GameObject.FindGameObjectWithTag("Audio");
        if (audioManagerObject != null)
        {
            audioManager = audioManagerObject.GetComponent<GameAudioManager>();
        }
        else
        {
            Debug.LogError("AudioManager not found! Ensure there is a GameObject with the tag 'Audio' and it has a GameAudioManager component.");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the player collided with the wall
        if (collision.gameObject.CompareTag("Player"))
        {
            BreakWall(collision.gameObject);
        }
    }

    private void BreakWall(GameObject player)
    {
        // Play the breaking sound effect
        if (audioManager != null)
        {
            audioManager.PlaySFX(audioManager.breakIce);
        }

        // Get the current position and size of the wall
        Vector3 wallPosition = transform.position;
        Vector3 wallSize = transform.localScale;

        // Destroy the wall object
        Destroy(gameObject);

        // Spawn smaller cubes to replace the wall
        for (int i = 0; i < cubeCount; i++)
        {
            // Generate random positions within the wall bounds
            Vector3 randomPosition = wallPosition + new Vector3(
                Random.Range(-wallSize.x / 2, wallSize.x / 2),
                Random.Range(-wallSize.y / 2, wallSize.y / 2),
                Random.Range(-wallSize.z / 2, wallSize.z / 2)
            );

            // Instantiate the broken cube
            GameObject brokenCube = Instantiate(brokenCubePrefab, randomPosition, Quaternion.identity);

            // Scale the cube
            brokenCube.transform.localScale = cubeScale;

            // Add Rigidbody if not already added to the prefab
            Rigidbody rb = brokenCube.GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = brokenCube.AddComponent<Rigidbody>();
            }

            // Ignore collision with the player
            Collider cubeCollider = brokenCube.GetComponent<Collider>();
            Collider playerCollider = player.GetComponent<Collider>();
            if (cubeCollider != null && playerCollider != null)
            {
                Physics.IgnoreCollision(cubeCollider, playerCollider, true);
            }

            // Apply explosion force to the cube to simulate explosion
            float explosionRadius = Mathf.Max(wallSize.x, wallSize.y, wallSize.z);
            rb.AddExplosionForce(explosionForce, wallPosition, explosionRadius);

            // Optionally add some rotation to simulate random rotation of the broken pieces
            brokenCube.transform.Rotate(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));

            // Destroy the cube after the specified delay
            Destroy(brokenCube, destroyDelay);
        }
    }
}
