using UnityEngine;

public class CrumblingWall : MonoBehaviour
{
    public GameObject brokenCubePrefab; // Assign the small cube prefab in the Inspector
    public int cubeCount = 10;          // Number of cubes the wall breaks into
    public Vector3 cubeScale = new Vector3(0.5f, 0.5f, 0.5f); // Size of the broken cubes
    public float explosionForce = 500f; // Explosion force to apply on the broken cubes

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the player collided with the wall
        if (collision.gameObject.CompareTag("Player"))
        {
            BreakWall();
        }
    }

    private void BreakWall()
    {
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

            // Ensure the cube is non-collidable initially, if needed
            Collider collider = brokenCube.GetComponent<Collider>();
            if (collider != null)
            {
                collider.enabled = false; // Enable the collider for the cubes
            }

            // Add Rigidbody if not already added to the prefab
            Rigidbody rb = brokenCube.GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = brokenCube.AddComponent<Rigidbody>(); // Add a Rigidbody component if it's missing
            }

            // Apply explosion force to the cube to simulate explosion
            rb.AddExplosionForce(explosionForce, wallPosition, 5f); // Adjust radius as needed

            // Optionally add some rotation to simulate random rotation of the broken pieces
            brokenCube.transform.Rotate(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
        }
    }
}
