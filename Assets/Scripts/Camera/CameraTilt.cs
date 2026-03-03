using UnityEngine;

public class CameraTilt : MonoBehaviour
{
    [SerializeField] private Rigidbody2D playerRb;
    [SerializeField] private float maxTiltAngle = 6f;
    [SerializeField] private float tiltSpeed = 5f;

    private float currentTilt;
    private float lastY;

    void Update()
    {
        float yVelocity = PlayerVelocityY();
        float targetTilt = Mathf.Clamp(-yVelocity * 0.5f, -maxTiltAngle, maxTiltAngle);

        currentTilt = Mathf.Lerp(
            currentTilt,
            targetTilt,
            Time.deltaTime * tiltSpeed
        );

        transform.localRotation = Quaternion.Euler(0f, 0f, currentTilt);
    }

    float PlayerVelocityY()
    {
        float velocityY = (playerRb.position.y - lastY) / Time.deltaTime;
        lastY = playerRb.position.y;

        return velocityY;
    }
}
