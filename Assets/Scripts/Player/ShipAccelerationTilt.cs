using UnityEngine;

public class ShipAccelerationTilt : MonoBehaviour
{
    [SerializeField] private Transform visual;
    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private float tiltMultiplier = 0.15f;
    [SerializeField] private float maxTilt = 8f;
    [SerializeField] private float smooth = 5f;

    private float lastSpeed;
    private float currentTilt;

    void Update()
    {
        float speed = rb.linearVelocity.x;
        float accel = (speed - lastSpeed) / Time.deltaTime;
        lastSpeed = speed;

        float targetTilt =
            Mathf.Clamp(-accel * tiltMultiplier, -maxTilt, maxTilt);

        currentTilt = Mathf.Lerp(
            currentTilt,
            targetTilt,
            Time.deltaTime * smooth
        );

        visual.localRotation =
            Quaternion.Euler(0f, 0f, currentTilt);
    }
}