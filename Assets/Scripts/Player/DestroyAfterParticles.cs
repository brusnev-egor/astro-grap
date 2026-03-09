using UnityEngine;

public class DestroyAfterParticles : MonoBehaviour
{
    void Start()
    {
        ParticleSystem ps = GetComponentInChildren<ParticleSystem>();
        Destroy(gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
    }
}