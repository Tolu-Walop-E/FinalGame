using UnityEngine;

public class GameAudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource SFXSource;

    public AudioClip background;
    public AudioClip death;
    public AudioClip checkpoint;
    public AudioClip coin;
    public AudioClip walkingSound;

    [Range(0f, 1f)] public float musicVolume = 1f; // Slider for music volume
    [Range(0f, 1f)] public float sfxVolume = 1f;  // Slider for SFX volume

    void Start()
    {
        if (background != null)
        {
            musicSource.clip = background;
            musicSource.loop = true;
            musicSource.Play();
        }
        UpdateVolume();
    }

    public void PlaySFX(AudioClip clip)
    {
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
        if (SFXSource != null)
        {
            SFXSource.pitch = speed;
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
