using UnityEngine;

public class DockPulse : MonoBehaviour
{
    [SerializeField] private Renderer targetRenderer;
    [SerializeField] private float pulseSpeed = 2f;
    [SerializeField] private float intensity = 2f;

    private Material mat;

    void Awake()
    {
        mat = targetRenderer.material;
    }

    void Update()
    {
        float emission =
            1f + Mathf.Sin(Time.time * pulseSpeed) * 0.5f;

        mat.SetColor("_EmissionColor",
            Color.yellow * emission * intensity);
    }
}