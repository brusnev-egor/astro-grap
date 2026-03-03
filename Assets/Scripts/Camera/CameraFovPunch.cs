using UnityEngine;
using Unity.Cinemachine;

public class CameraFovPunch : MonoBehaviour
{
    public static CameraFovPunch Instance;

    [SerializeField] private CinemachineCamera vcam;

    [Header("Punch Settings")]
    [SerializeField] private float punchDuration = 0.25f;
    [SerializeField] private AnimationCurve punchCurve;

    private float baseFov;
    private float timer;
    private float punchAmount;

    void Awake()
    {
        Instance = this;
        baseFov = vcam.Lens.FieldOfView;

        // если кривая не задана — создаём дефолтную
        if (punchCurve == null || punchCurve.length == 0)
        {
            punchCurve = new AnimationCurve(
                new Keyframe(0f, 0f),
                new Keyframe(0.15f, 1f),
                new Keyframe(1f, 0f)
            );
        }
    }

    void Update()
    {
        // if (timer > 0f)
        // {
        //     timer -= Time.deltaTime;

        //     float t = 1f - (timer / punchDuration);
        //     float curveValue = punchCurve.Evaluate(t);

        //     vcam.Lens.FieldOfView =
        //         baseFov + curveValue * punchAmount;
        // }
        // else
        // {
        //     vcam.Lens.FieldOfView = baseFov;
        // }
    }

    public void Punch(float amount)
    {
        punchAmount = amount;
        timer = punchDuration;
    }
}