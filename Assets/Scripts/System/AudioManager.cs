using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Source")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource musicSource;

    [Header("Clips")]
    public AudioClip grappleShoot;
    public AudioClip dockSuccess;
    public AudioClip ropeBreak;
    public AudioClip hit;

    void Awake()
    {
        Instance = this;
        if (musicSource && PlayerPrefsManager.IsMusicEnabled())
        {
            musicSource.Play();
        }
    }

    public void Play(AudioClip clip, float volume = 1f)
    {
        if (clip == null)
            return;

        if (!PlayerPrefsManager.IsSoundEnabled())
            return;

        sfxSource.PlayOneShot(clip, volume);
    }

    public void PauseMusic()
    {
        musicSource.Pause();
    }

    public void ResumeMusic()
    {
        musicSource.UnPause();
    }

    public void MuteSound()
    {
        sfxSource.enabled = false;
        PlayerPrefsManager.SetSound(0);
    }

    public void UnmuteSound()
    {
        sfxSource.enabled = true;
        PlayerPrefsManager.SetSound(1);
    }

    public void MuteMusic()
    {
        if (musicSource)
        {
            musicSource.Stop();
            musicSource.enabled = false;
        }
        PlayerPrefsManager.SetMusic(0);
    }

    public void UnmuteMusic()
    {
        if (musicSource)
        {
            musicSource.enabled = true;
            musicSource.Play();
            if (GameManager.Instance.IsPaused)
            {
                musicSource.Pause();
            }
        }
        PlayerPrefsManager.SetMusic(1);
    }
}
