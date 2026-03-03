using UnityEngine;
using System.Collections;

public class TimeEffects : MonoBehaviour
{
    public static TimeEffects Instance;

    void Awake()
    {
        Instance = this;
    }

    public void SlowMotion(float scale, float duration)
    {
        StartCoroutine(SlowMoRoutine(scale, duration));
    }

    IEnumerator SlowMoRoutine(float scale, float duration)
    {
        Time.timeScale = scale;
        Time.fixedDeltaTime = 0.02f * scale;

        yield return new WaitForSecondsRealtime(duration);

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
    }

    public void HitStop(float duration)
    {
        StartCoroutine(HitStopRoutine(duration));
    }

    IEnumerator HitStopRoutine(float duration)
    {
        float prevScale = Time.timeScale;
        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(duration);

        Time.timeScale = prevScale;
    }

}
