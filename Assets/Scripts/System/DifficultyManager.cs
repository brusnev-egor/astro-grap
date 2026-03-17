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

    [Header("Difficulty thresholds")]
    [SerializeField] private float mediumDistance = 250f;
    [SerializeField] private float hardDistance = 500f;

    [Header("Difficulty FOV")]
    [SerializeField] private float mediumFov = 42;
    [SerializeField] private float hardFov = 48;

    [Header("Speed")]
    [SerializeField] private float baseSpeed = 8f;
    [SerializeField] private float maxSpeed = 18f;
    [SerializeField] private float speedIncreaseDistance = 2000f;
    [SerializeField] private AnimationCurve speedCurve;

    [Header("Turbo")]
    [SerializeField] private float turboSpeedBonus = 6f;
    [SerializeField] private float turboDuration = 3f;

    private float turboTimer;
    private float turboMultiplier;

    // TODO: remove with ChunkSpawner_Legacy
    public float Difficulty01 { get; private set; }

    public float CurrentSpeed { get; private set; }
    public DifficultyLevel CurrentDifficulty { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        float distance = DistanceTracker.Instance.TotalDistance;

        UpdateDifficulty(distance);
        UpdateSpeed(distance);
        UpdateTurbo();
    }

    void UpdateDifficulty(float distance)
    {
        if (distance >= hardDistance)
        {
            CurrentDifficulty = DifficultyLevel.Hard;
        }
        else if (distance >= mediumDistance)
        {
            CameraFovController.Instance.ExpandFov(mediumFov);
            CurrentDifficulty = DifficultyLevel.Medium;
        }
        else
            CurrentDifficulty = DifficultyLevel.Easy;
    }

    void UpdateSpeed(float distance)
    {
        float t = Mathf.Clamp01(distance / speedIncreaseDistance);

        float curve = speedCurve.Evaluate(t);

        float baseSpeedNow = Mathf.Lerp(baseSpeed, maxSpeed, curve);

        CurrentSpeed = baseSpeedNow + turboMultiplier * turboSpeedBonus;
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

    void OnGUI()
    {
        GUIStyle headStyle = new GUIStyle();
        headStyle.fontSize = 48;
        headStyle.normal.textColor = Color.white;
        GUILayout.Label("Distance: " + DistanceTracker.Instance.TotalDistance.ToString("F0"), headStyle);
        GUILayout.Label("Difficulty: " + CurrentDifficulty, headStyle);
        GUILayout.Label("Speed: " + CurrentSpeed.ToString("F2"), headStyle);
    }

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