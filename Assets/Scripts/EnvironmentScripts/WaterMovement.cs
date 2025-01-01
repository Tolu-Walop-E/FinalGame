using UnityEngine;

public class WaterWaveMotion : MonoBehaviour
{
    public float waveSpeed = 1.0f; // Speed of the wave movement
    public float waveAmplitude = 0.5f; // Height of the waves
    public float waveFrequency = 1.0f; // Frequency of the waves (how close waves are)
    public Vector3 waveDirection = new Vector3(1, 0, 1); // Direction of wave propagation

    private Vector3 initialPosition; // Store the initial position of the cube

    void Start()
    {
        initialPosition = transform.position; // Record the starting position
    }

    void Update()
    {
        // Calculate wave offset using a sine wave
        float waveOffset = Mathf.Sin(Time.time * waveFrequency + Vector3.Dot(transform.position, waveDirection) * waveSpeed) * waveAmplitude;

        // Apply the wave offset to the cube's Y position
        transform.position = new Vector3(initialPosition.x, initialPosition.y + waveOffset, initialPosition.z);
    }
}
