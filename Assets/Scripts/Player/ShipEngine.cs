using UnityEngine;

public class ShipEngine : MonoBehaviour
{
    [SerializeField] private ParticleSystem engineParticles;

    [Header("Normal")]
    [SerializeField] private float normalLifetime = 0.3f;
    [SerializeField] private float normalSpeed = 3f;
    [SerializeField] private float normalEmission = 20f;

    [Header("Turbo")]
    [SerializeField] private float turboLifetime = 0.6f;
    [SerializeField] private float turboSpeed = 6f;
    [SerializeField] private float turboEmission = 60f;

    [SerializeField] private float smooth = 5f;

    private float currentLifetime;
    private float currentSpeed;
    private float currentEmission;

    private float targetLifetime;
    private float targetSpeed;
    private float targetEmission;

    void Start()
    {
        currentLifetime = normalLifetime;
        currentSpeed = normalSpeed;
        currentEmission = normalEmission;

        targetLifetime = normalLifetime;
        targetSpeed = normalSpeed;
        targetEmission = normalEmission;
    }

    void Update()
    {
        CheckTurbo();
        SmoothUpdate();
        ApplyValues();
    }

    void SmoothUpdate()
    {
        // currentLifetime = Mathf.Lerp(currentLifetime, targetLifetime, Time.deltaTime * smooth);
        // currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.deltaTime * smooth);
        // currentEmission = Mathf.Lerp(currentEmission, targetEmission, Time.deltaTime * smooth);
        currentLifetime = targetLifetime;
        currentSpeed = targetSpeed;
        currentEmission = targetEmission;
    }

    void ApplyValues()
    {
        var main = engineParticles.main;
        main.startLifetime = currentLifetime;
        main.startSpeed = currentSpeed;

        var emission = engineParticles.emission;
        emission.rateOverTime = currentEmission;
    }

    void CheckTurbo()
    {
        if (DifficultyManager.Instance.IsTurboActive())
        {
            targetLifetime = turboLifetime;
            targetSpeed = turboSpeed;
            targetEmission = turboEmission;
        }
        else
        {
            targetLifetime = normalLifetime;
            targetSpeed = normalSpeed;
            targetEmission = normalEmission;
        }
    }
}