using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ComboVignette : MonoBehaviour
{
    [SerializeField] private Volume volume;

    [Header("Settings")]
    [SerializeField] private int comboThreshold = 3;
    [SerializeField] private float maxIntensity = 0.35f;
    [SerializeField] private float pulseSpeed = 4f;
    [SerializeField] private float fadeSpeed = 4f;

    private Vignette vignette;
    private float currentIntensity;
    private bool active;

    void Start()
    {
        volume.profile.TryGet(out vignette);

        ComboSystem.Instance.OnComboChanged += HandleComboChanged;
        ComboSystem.Instance.OnComboReset += HandleComboReset;
    }

    void HandleComboChanged(int combo)
    {
        active = combo >= comboThreshold;
    }

    void HandleComboReset()
    {
        active = false;
    }

    void Update()
    {
        if (vignette == null)
            return;

        if (active)
        {
            float pulse = (Mathf.Sin(Time.unscaledTime * pulseSpeed) + 1f) * 0.5f;
            float target = pulse * maxIntensity;

            currentIntensity = Mathf.Lerp(
                currentIntensity,
                target,
                Time.unscaledDeltaTime * fadeSpeed
            );
        }
        else
        {
            currentIntensity = Mathf.Lerp(
                currentIntensity,
                0f,
                Time.unscaledDeltaTime * fadeSpeed
            );
        }

        vignette.intensity.value = currentIntensity;
    }
}
