using UnityEngine;
using Unity.Cinemachine;

public class CameraFovController : MonoBehaviour
{
    public static CameraFovController Instance;

    [SerializeField] private CinemachineCamera vcam;

    [Header("Base")]
    [SerializeField] private float baseFov = 36f;

    [Header("Turbo")]
    [SerializeField] private float turboFovBonus = 6f;
    [SerializeField] private float turboInDuration = 0.25f;
    [SerializeField] private float turboOutDuration = 0.25f;

    [Header("Punch")]
    [SerializeField] private float punchDuration = 0.25f;
    [SerializeField] private AnimationCurve punchCurve;

    private float turboOffset;
    private float turboTimer;
    private bool turboActiveLastFrame;

    private float punchTimer;
    private float punchAmount;

    void Awake()
    {
        Instance = this;

        if (punchCurve == null || punchCurve.length == 0)
        {
            punchCurve = new AnimationCurve(
                new Keyframe(0f, 0f),
                new Keyframe(0.15f, 1f),
                new Keyframe(1f, 0f)
            );
        }

        ApplyFov(baseFov);
    }

    void Update()
    {
        UpdateTurbo();
        UpdatePunch();
        ApplyFinalFov();
    }

    // =========================
    // TURBO (детерминированный)
    // =========================

    void UpdateTurbo()
    {
        bool turboActive = DifficultyManager.Instance.IsTurboActive();

        // Если Turbo только что начался
        if (turboActive && !turboActiveLastFrame)
        {
            turboTimer = 0f;
            turboOffset = 0f; // жёсткий сброс
        }

        if (turboActive)
        {
            turboTimer += Time.deltaTime;

            float t = Mathf.Clamp01(turboTimer / turboInDuration);
            turboOffset = turboFovBonus * t;
        }
        else
        {
            // Плавный выход
            turboOffset = Mathf.MoveTowards(
                turboOffset,
                0f,
                turboFovBonus / turboOutDuration * Time.deltaTime
            );
        }

        turboActiveLastFrame = turboActive;
    }

    // =========================
    // PUNCH
    // =========================

    void UpdatePunch()
    {
        if (punchTimer > 0f)
            punchTimer -= Time.deltaTime;
    }

    float GetPunchOffset()
    {
        if (punchTimer <= 0f)
            return 0f;

        float t = 1f - (punchTimer / punchDuration);
        return punchCurve.Evaluate(t) * punchAmount;
    }

    public void Punch(float amount)
    {
        punchAmount = amount;
        punchTimer = punchDuration;
    }

    // =========================
    // APPLY
    // =========================

    void ApplyFinalFov()
    {
        float finalFov =
            baseFov +
            turboOffset +
            GetPunchOffset();

        ApplyFov(finalFov);
    }

    void ApplyFov(float value)
    {
        Debug.Log("Apply fov = " + value);
        var lens = vcam.Lens;
        lens.FieldOfView = value;
        vcam.Lens = lens;
    }
}