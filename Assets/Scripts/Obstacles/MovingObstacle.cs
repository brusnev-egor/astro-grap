using UnityEngine;

public class MovingObstacle : Obstacle
{
    [Header("Movement")]
    [SerializeField] private float amplitude = 1.5f;
    [SerializeField] private float speed = 1.5f;

    private Vector3 startLocalPos;
    private bool isActive;
    private float timeOffset;

    void Awake()
    {
        startLocalPos = transform.localPosition;
        timeOffset = Random.value * 10f; // чтобы не двигались синхронно
    }

    void Update()
    {
        if (!isActive)
            return;

        float y = Mathf.Sin((Time.time + timeOffset) * speed) * amplitude;
        transform.localPosition = startLocalPos + Vector3.up * y;
    }

    public void StartMovement()
    {
        isActive = true;
    }

    public void StopMovement()
    {
        isActive = false;
        transform.localPosition = startLocalPos;
    }
}
