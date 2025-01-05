using UnityEngine;
using UnityEngine.UI; // Import UI namespace

public class GameAudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource SFXSource;
    [SerializeField] private Slider musicSlider; // Reference to music slider
    [SerializeField] private Slider sfxSlider;   // Reference to SFX slider

    public AudioClip background;
    public AudioClip death;
    public AudioClip checkpoint;
    public AudioClip coin;
    public AudioClip walkingSound;
    public AudioClip jumpingSound;

    [Range(0f, 1f)] public float musicVolume = 1f;
    [Range(0f, 1f)] public float sfxVolume = 1f;

    private float currentPitch = 1f;

    void Start()
    {
        // Initialize sliders
        if (musicSlider != null)
        {
            musicSlider.value = musicVolume;
            musicSlider.onValueChanged.AddListener(SetMusicVolume); // Link slider to method
        }

        if (sfxSlider != null)
        {
            sfxSlider.value = sfxVolume;
            sfxSlider.onValueChanged.AddListener(SetSFXVolume); // Link slider to method
        }

        // Start background music
        if (background != null)
        {
            musicSource.clip = background;
            musicSource.loop = true;
            musicSource.Play();
        }
        UpdateVolume();
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume); // Ensure volume stays between 0 and 1
        if (musicSource != null)
        {
            musicSource.volume = musicVolume;
        }
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume); // Ensure volume stays between 0 and 1
        if (SFXSource != null)
        {
            SFXSource.volume = sfxVolume;
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.pitch = currentPitch;
        SFXSource.PlayOneShot(clip, sfxVolume);
    }

    public void PauseSFX()
    {
        SFXSource.Pause();
    }

    public void PlaySFXLoop(AudioClip clip)
    {
        if (SFXSource.clip != clip || !SFXSource.isPlaying)
        {
            SFXSource.clip = clip;
            SFXSource.loop = true;
            SFXSource.Play();
        }
        SFXSource.pitch = currentPitch;
    }

    public void StopSFX()
    {
        if (SFXSource.isPlaying)
        {
            SFXSource.Stop();
            SFXSource.loop = false;
        }
    }

    public void SetSFXSpeed(float speed)
    {
        currentPitch = speed;
        if (SFXSource.isPlaying)
        {
            SFXSource.pitch = currentPitch;
        }
    }

    void Update()
    {
        UpdateVolume();
    }

    private void UpdateVolume()
    {
        if (musicSource != null)
        {
            musicSource.volume = musicVolume;
        }
        if (SFXSource != null)
        {
            SFXSource.volume = sfxVolume;
        }
    }
}
