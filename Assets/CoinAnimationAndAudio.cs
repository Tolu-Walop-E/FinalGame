using UnityEngine;

public class CoinAnimationAndAudio : MonoBehaviour
{
    [SerializeField] private float spinSpeed = 100f;
    [SerializeField] private float bounceHeight = 0.5f;
    [SerializeField] private float bounceSpeed = 2f;
    [SerializeField] private AudioClip collectSound;
    [SerializeField] private int value = 1;


    private Vector3 startPosition;
    private GameAudioManager audioManager;

    void Start()
    {
        startPosition = transform.position;

        GameObject audioManagerObject = GameObject.FindGameObjectWithTag("Audio");
        if (audioManagerObject != null)
        {
            audioManager = audioManagerObject.GetComponent<GameAudioManager>();
        }
        else
        {
            Debug.LogError("AudioManager not found! Ensure an object with the 'Audio' tag exists.");
        }
    }

    void Update()
    {
        transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime, Space.World);

        float newY = startPosition.y + Mathf.Sin(Time.time * bounceSpeed) * bounceHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (audioManager != null && collectSound != null)
            {
                audioManager.PlaySFX(collectSound);
            }

            if (CoinCounter.Instance != null)
            {
                CoinCounter.Instance.IncreaseCoins(value);
            }
            }
            else
            {
                Debug.LogError("CoinCounter instance is missing in the scene.");
            }

            Destroy(gameObject);
        }
    }

