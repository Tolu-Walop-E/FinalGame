using UnityEngine;

public class WaterMovement : MonoBehaviour
{
    // Wave parameters
    public float waveSpeed = 1f;
    public float waveHeight = 0.5f;
    public float waveFrequency = 1f;

    // Splash effect prefab
    public GameObject splashPrefab;

    // Update is called once per frame
    void Update()
    {
        AnimateWater();
    }

    void AnimateWater()
    {
        // Get the mesh of the water
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter == null) return;

        Mesh mesh = meshFilter.mesh;
        Vector3[] vertices = mesh.vertices;

        // Apply sine wave movement to vertices
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 worldPos = transform.TransformPoint(vertices[i]);
            vertices[i].y = Mathf.Sin(worldPos.x * waveFrequency + Time.time * waveSpeed) * waveHeight;
        }

        mesh.vertices = vertices;
        mesh.RecalculateNormals(); // Optional, improves visual quality
        meshFilter.mesh = mesh;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Trigger splash effect
        if (splashPrefab != null)
        {
            Vector3 splashPosition = new Vector3(other.transform.position.x, transform.position.y, transform.position.z);
            Instantiate(splashPrefab, splashPosition, Quaternion.identity);
        }
    }
}
