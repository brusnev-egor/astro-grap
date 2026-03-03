using UnityEngine;
using System.Collections;

public class CameraOrthoPunch : MonoBehaviour
{
    [SerializeField] private Camera cam;

    [Header("Punch Settings")]
    [SerializeField] private float punchAmount = 0.6f;
    [SerializeField] private float punchDuration = 0.12f;
    [SerializeField] private float returnSpeed = 6f;

    private float baseSize;

    void Awake()
    {
        if (cam == null)
            cam = GetComponent<Camera>();

        baseSize = cam.orthographicSize;
    }

    public void Punch()
    {
        StopAllCoroutines();
        StartCoroutine(PunchRoutine());
    }

    IEnumerator PunchRoutine()
    {
        float targetSize = baseSize - punchAmount; // уменьшаем size = zoom in

        float t = 0f;
        while (t < punchDuration)
        {
            t += Time.unscaledDeltaTime;
            cam.orthographicSize = Mathf.Lerp(baseSize, targetSize, t / punchDuration);
            yield return null;
        }

        while (Mathf.Abs(cam.orthographicSize - baseSize) > 0.01f)
        {
            cam.orthographicSize = Mathf.Lerp(
                cam.orthographicSize,
                baseSize,
                Time.unscaledDeltaTime * returnSpeed
            );
            yield return null;
        }

        cam.orthographicSize = baseSize;
    }
}
