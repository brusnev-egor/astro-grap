using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class GrappleRope : MonoBehaviour
{
    [SerializeField] private GrappleController grapple;
    [SerializeField] private Transform playerAnchor;

    [Header("Visual")]
    [SerializeField] private float baseWidth = 0.06f;
    [SerializeField] private float stretchMultiplier = 0.4f;
    [SerializeField] private float smoothness = 12f;

    private LineRenderer line;

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
            return;
        }

        line.enabled = true;

        line.SetPosition(0, playerAnchor.position);
        line.SetPosition(1, end);
    }
}
