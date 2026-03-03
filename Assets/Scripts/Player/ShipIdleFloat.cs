using UnityEngine;

public class ShipIdleFloat : MonoBehaviour
{
    [SerializeField] private Transform visual;
    [SerializeField] private float amplitude = 0.06f;
    [SerializeField] private float speed = 1.5f;

    private Vector3 baseLocalPos;

    void Awake()
    {
        baseLocalPos = visual.localPosition;
    }

    void Update()
    {
        float offset = Mathf.Sin(Time.time * speed) * amplitude;

        visual.localPosition = baseLocalPos + new Vector3(0f, offset, 0f);
    }
}