using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CanvasGroup))]
public class PerfectPopup : MonoBehaviour
{
    public static PerfectPopup Instance;

    [SerializeField] private float fadeInTime = 0.1f;
    [SerializeField] private float visibleTime = 0.4f;
    [SerializeField] private float fadeOutTime = 0.25f;

    private CanvasGroup canvasGroup;
    private Coroutine currentRoutine;

    void Awake()
    {
        Instance = this;
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
    }

    public void Show()
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(ShowRoutine());
    }

    IEnumerator ShowRoutine()
    {
        // Fade In
        yield return Fade(0f, 1f, fadeInTime);

        // Stay visible
        yield return new WaitForSecondsRealtime(visibleTime);

        // Fade Out
        yield return Fade(1f, 0f, fadeOutTime);
    }

    IEnumerator Fade(float from, float to, float duration)
    {
        float t = 0f;
        canvasGroup.alpha = from;

        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Lerp(from, to, t / duration);
            yield return null;
        }

        canvasGroup.alpha = to;
    }
}
