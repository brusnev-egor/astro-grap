using UnityEngine;

public class DistanceTracker : MonoBehaviour
{
    public static DistanceTracker Instance { get; private set; }

    public float TotalDistance { get; private set; }

    private bool isPaused;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (isPaused)
            return;

        var currentSpeed = GameManager.Instance.CurrentSpeed;

        TotalDistance += currentSpeed * Time.deltaTime;
    }

    public void SetPaused(bool paused)
    {
        isPaused = paused;
    }

    public void ResetDistance()
    {
        TotalDistance = 0f;
    }
}