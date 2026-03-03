using UnityEngine;

public class DockRotation : MonoBehaviour
{
    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 40f;
    [SerializeField] private bool randomizeDirection = true;

    private float direction = 1f;

    void Awake()
    {
        if (randomizeDirection)
            direction = Random.value > 0.5f ? 1f : -1f;
    }

    void Update()
    {
        transform.Rotate(
            rotationSpeed * direction * Time.deltaTime,
            0f,
            0f,
            Space.Self
        );
    }
}