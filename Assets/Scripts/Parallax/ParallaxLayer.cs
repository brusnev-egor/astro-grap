using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    [Range(0f, 1f)]
    public float parallaxFactor = 0.5f;

    [Range(0f, 1f)]
    public float parallaxFactorY = 0.1f;

    private float baseY;

    void Awake()
    {
        baseY = transform.position.y;
    }

    public void ApplyShift(float deltaX)
    {
        transform.position -= new Vector3(deltaX * parallaxFactor, 0f, 0f);
    }

    public void ApplyVertical(float deltaY)
    {
        Vector3 pos = transform.position;
        pos.y = baseY - deltaY * parallaxFactorY;
        transform.position = pos;
    }
}
