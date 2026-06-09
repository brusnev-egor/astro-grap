using UnityEngine;

public class MainMenuIdleItem : MonoBehaviour
{
    [Header("Floating")]
    public float floatAmplitude = 0.2f;
    public float floatFrequency = 1f;

    [Header("Rotation Wobble")]
    public Vector3 rotationAxis = Vector3.forward;
    public float maxAngle = 15f;        // Максимальный угол отклонения
    public float rotationSpeed = 1f;    // Скорость "туда-обратно"

    [Header("Randomness")]
    public float randomOffset;

    private Vector3 startPos;
    private Quaternion startRot;

    void Start()
    {
        startPos = transform.position;
        startRot = transform.rotation;

        randomOffset = Random.Range(0f, 100f);

        // случайная ось для астероидов
        if (rotationAxis == Vector3.zero)
            rotationAxis = Random.onUnitSphere;
    }

    void Update()
    {
        float time = Time.time + randomOffset;

        // ПЛАВНОЕ ПЛАВАНИЕ
        float y = Mathf.Sin(time * floatFrequency) * floatAmplitude;
        float x = Mathf.Cos(time * floatFrequency * 0.5f) * floatAmplitude * 0.5f;

        transform.position = startPos + new Vector3(x, y, 0);

        // ГЛАВНОЕ: вращение туда-обратно
        float angle = Mathf.Sin(time * rotationSpeed) * maxAngle;

        transform.rotation = startRot * Quaternion.AngleAxis(angle, rotationAxis);
    }
}