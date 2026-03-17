using UnityEngine;

public class AsteroidIdleRotation : MonoBehaviour
{
    [Header("Speed")]
    [SerializeField] private float minSpeed = 5f;
    [SerializeField] private float maxSpeed = 20f;

    private Vector3 rotationAxis;
    private float rotationSpeed;

    void Awake()
    {
        // случайная ось (нормализуем чтобы скорость не ломалась)
        rotationAxis = Random.onUnitSphere;

        // случайная скорость
        rotationSpeed = Random.Range(minSpeed, maxSpeed);

        // случайное направление (влево / вправо)
        if (Random.value > 0.5f)
            rotationSpeed *= -1f;
    }

    void Update()
    {
        transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime, Space.Self);
    }
    // void Update()
    // {
    //     Vector3 wobble = new Vector3(
    //         Mathf.Sin(Time.time * 0.7f),
    //         Mathf.Sin(Time.time * 0.9f),
    //         Mathf.Sin(Time.time * 0.6f)
    //     ) * 0.1f;

    //     Vector3 finalAxis = (rotationAxis + wobble).normalized;

    //     transform.Rotate(finalAxis, rotationSpeed * Time.deltaTime, Space.Self);
    // }
}