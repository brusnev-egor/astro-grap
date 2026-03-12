using UnityEngine;

public enum DifficultyLevel
{
    Easy,
    Medium,
    Hard
}

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager Instance { get; private set; }

    [Header("Distance Scaling")]
    [SerializeField] private float maxDistanceForDifficulty = 3000f;

    [Header("Speed")]
    [SerializeField] private float baseSpeed = 8f;
    [SerializeField] private float maxSpeed = 18f;
    [SerializeField] private AnimationCurve speedCurve;

    [Header("Turbo")]
    [SerializeField] private float turboSpeedBonus = 6f;
    [SerializeField] private float turboDuration = 3f;

    private float turboTimer;
    private float turboMultiplier = 0f;

    public float Difficulty01 { get; private set; }
    public float CurrentSpeed { get; private set; }
    public DifficultyLevel CurrentDifficulty;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        float distance = DistanceTracker.Instance.TotalDistance;

        Difficulty01 = Mathf.Clamp01(distance / maxDistanceForDifficulty);

        UpdateSpeed();
        UpdateRisk();
        UpdateTurbo();
    }

    void UpdateSpeed()
    {
        float t = speedCurve.Evaluate(Difficulty01);
        float baseSpeedNow = Mathf.Lerp(baseSpeed, maxSpeed, t);

        CurrentSpeed = baseSpeedNow + turboMultiplier * turboSpeedBonus;
    }

    void UpdateRisk()
    {
        // RiskyMultiplier = riskyChanceCurve.Evaluate(Difficulty01);
    }

    void UpdateTurbo()
    {
        if (turboTimer > 0f)
        {
            turboTimer -= Time.deltaTime;
            turboMultiplier = 1f;

            if (turboTimer <= 0f)
            {
                turboMultiplier = 0f;
            }
        }
    }

    // void OnGUI()
    // {
    //     GUIStyle headStyle = new GUIStyle();
    //     headStyle.fontSize = 48;
    //     headStyle.normal.textColor = Color.white;
    //     GUILayout.Label("Distance: " + DistanceTracker.Instance.TotalDistance.ToString("F0"), headStyle);
    //     GUILayout.Label("Difficulty: " + Difficulty01.ToString("F2"), headStyle);
    //     GUILayout.Label("Speed: " + CurrentSpeed.ToString("F2"), headStyle);
    // }

    public void ActivateTurbo()
    {
        turboTimer = turboDuration;
    }

    public bool IsTurboActive()
    {
        return turboTimer > 0f;
    }

    public float GetTurboDuration()
    {
        return turboDuration;
    }
}