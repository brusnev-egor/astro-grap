using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class TurboVisualController : MonoBehaviour
{
    [Header("Post Processing")]
    [SerializeField] private Volume volume;

    [Header("Particles")]
    [SerializeField] private ParticleSystem speedLines;

    [Header("Hyper Settings")]
    [SerializeField] private float vignetteTarget = 0.45f;
    [SerializeField] private float chromaTarget = 0.25f;
    [SerializeField] private float saturationBoost = 15f;
    [SerializeField] private float smooth = 4f;

    private Vignette vignette;
    private ChromaticAberration chroma;
    private ColorAdjustments colorAdjust;

    void Start()
    {
        volume.profile.TryGet(out vignette);
        volume.profile.TryGet(out chroma);
        volume.profile.TryGet(out colorAdjust);
    }

    void Update()
    {
        bool turbo = DifficultyManager.Instance.IsTurboActive();

        UpdatePost(turbo);
        UpdateParticles(turbo);
    }

    void UpdateParticles(bool turbo)
    {
        if (turbo)
        {
            if (!speedLines.isPlaying)
                speedLines.Play();
        }
        else
        {
            if (speedLines.isPlaying)
                speedLines.Stop();
        }
    }

    void UpdatePost(bool turbo)
    {
        float t = turbo ? 1f : 0f;

        if (vignette != null)
        {
            vignette.intensity.value =
                Mathf.Lerp(
                    vignette.intensity.value,
                    t * vignetteTarget,
                    Time.deltaTime * smooth
                );
        }

        if (chroma != null)
        {
            chroma.intensity.value =
                Mathf.Lerp(
                    chroma.intensity.value,
                    t * chromaTarget,
                    Time.deltaTime * smooth
                );
        }

        if (colorAdjust != null)
        {
            colorAdjust.saturation.value =
                Mathf.Lerp(
                    colorAdjust.saturation.value,
                    t * saturationBoost,
                    Time.deltaTime * smooth
                );
        }
    }
}