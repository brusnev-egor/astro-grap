using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ComboColorGrading : MonoBehaviour
{
    [SerializeField] private Volume volume;

    [Header("Settings")]
    [SerializeField] private int comboThreshold = 3;

    [SerializeField] private float targetSaturation = 20f;
    [SerializeField] private float targetContrast = 15f;
    [SerializeField] private float targetExposure = 0.2f;

    [SerializeField] private float transitionSpeed = 3f;

    private ColorAdjustments colorAdjustments;
    private bool active;

    void Start()
    {
        volume.profile.TryGet(out colorAdjustments);

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
        if (colorAdjustments == null)
            return;

        float targetSat = active ? targetSaturation : 0f;
        float targetCon = active ? targetContrast : 0f;
        float targetExp = active ? targetExposure : 0f;

        colorAdjustments.saturation.value =
            Mathf.Lerp(
                colorAdjustments.saturation.value,
                targetSat,
                Time.unscaledDeltaTime * transitionSpeed
            );

        colorAdjustments.contrast.value =
            Mathf.Lerp(
                colorAdjustments.contrast.value,
                targetCon,
                Time.unscaledDeltaTime * transitionSpeed
            );

        colorAdjustments.postExposure.value =
            Mathf.Lerp(
                colorAdjustments.postExposure.value,
                targetExp,
                Time.unscaledDeltaTime * transitionSpeed
            );
    }
}
