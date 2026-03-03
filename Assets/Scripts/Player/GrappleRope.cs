using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class GrappleRope : MonoBehaviour
{
    [SerializeField] private GrappleController grapple;
    [SerializeField] private Transform playerAnchor;

    [Header("Visual")]
    [SerializeField] private Gradient colorByTension;
    [SerializeField] private float baseWidth = 0.06f;
    [SerializeField] private float stretchMultiplier = 0.4f;
    [SerializeField] private float smoothness = 12f;

    private LineRenderer line;
    private float currentTension;

    void Awake()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = 2;
        line.enabled = false;
    }

    void LateUpdate()
    {
        if (!grapple.TryGetRopeVisual(out Vector2 end, out float tension))
        {
            line.enabled = false;
            currentTension = 0f;
            return;
        }

        line.enabled = true;

        currentTension = Mathf.Lerp(
            currentTension,
            tension,
            Time.deltaTime * smoothness
        );

        line.SetPosition(0, playerAnchor.position);
        line.SetPosition(1, end);

        UpdateColorAndWidth();
    }

    void UpdateColorAndWidth()
    {
        Color c = colorByTension.Evaluate(currentTension);
        line.startColor = c;
        line.endColor = c;
    }
}
