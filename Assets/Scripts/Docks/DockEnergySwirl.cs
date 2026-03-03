using UnityEngine;

public class DockEnergySwirl : MonoBehaviour
{
    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 60f;

    [Header("Pulse")]
    [SerializeField] private float pulseSpeed = 2f;
    [SerializeField] private float minScale = 0.9f;
    [SerializeField] private float maxScale = 1.05f;

    [Header("Emission")]
    [SerializeField] private Renderer targetRenderer;
    [SerializeField] private Color emissionColor = new Color(1f, 0.5f, 0f);
    [SerializeField] private float minEmission = 2f;
    [SerializeField] private float maxEmission = 6f;

    private Material mat;

    void Awake()
    {
        mat = targetRenderer.material;
    }

    void Update()
    {
        Rotate();
        PulseScale();
        PulseEmission();
    }

    void Rotate()
    {
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }

    void PulseScale()
    {
        float t = (Mathf.Sin(Time.time * pulseSpeed) + 1f) * 0.5f;
        float scale = Mathf.Lerp(minScale, maxScale, t);
        transform.localScale = Vector3.one * scale;
    }

    void PulseEmission()
    {
        float t = (Mathf.Sin(Time.time * pulseSpeed) + 1f) * 0.5f;
        float emission = Mathf.Lerp(minEmission, maxEmission, t);

        mat.SetColor("_EmissionColor", emissionColor * emission);
    }
}