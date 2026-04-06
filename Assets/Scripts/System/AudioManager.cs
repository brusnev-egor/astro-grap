using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Source")]
    [SerializeField] private AudioSource sfxSource;

    [Header("Clips")]
    public AudioClip grappleShoot;
    public AudioClip dockSuccess;
    public AudioClip ropeBreak;
    public AudioClip hit;

    void Awake()
    {
        Instance = this;
    }

    public void Play(AudioClip clip, float volume = 1f)
    {
        if (clip == null)
            return;

        sfxSource.PlayOneShot(clip, volume);
    }

    public void MuteSound()
    {
        sfxSource.enabled = false;
    }

    public void UnmuteSound()
    {
        sfxSource.enabled = true;
    }
}
