using UnityEngine;

public class CoinAnimationAndAudio : MonoBehaviour
{
    [SerializeField] private float spinSpeed = 100f; // Speed of coin spin
    [SerializeField] private float bounceHeight = 0.5f; // Height of the bounce
    [SerializeField] private float bounceSpeed = 2f; // Speed of the bounce
    [SerializeField] private AudioClip collectSound; // Sound to play when the coin is collected
    public int value;
    private Vector3 startPosition; // Original position of the coin
    private GameAudioManager audioManager; // Reference to the audio manager

    void Start()
    {
        // Store the original position of the coin
        startPosition = transform.position;

        // Get reference to the AudioManager
        GameObject audioManagerObject = GameObject.FindGameObjectWithTag("Audio");
        if (audioManagerObject != null)
        {
            audioManager = audioManagerObject.GetComponent<GameAudioManager>();
        }
        else
        {
            Debug.LogError("AudioManager not found! Ensure an object with the 'AudioManager' tag exists.");
        }
    }

    void Update()
    {
        // Spin the coin
        transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime, Space.World);

        // Make the coin bounce up and down
        float newY = startPosition.y + Mathf.Sin(Time.time * bounceSpeed) * bounceHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player collects the coin
        if (other.CompareTag("Player"))
        {
            // Play collect sound
            if (audioManager != null && collectSound != null)
            {
                audioManager.PlaySFX(collectSound);
            }
            CoinCounter.instance.IncreaseCoins(value);

            // Destroy the coin
            Destroy(gameObject);
        }
    }
}
