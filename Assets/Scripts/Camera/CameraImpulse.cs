using Unity.Cinemachine;
using UnityEngine;

public class CameraImpulse : MonoBehaviour
{
    public static CameraImpulse Instance;

    [SerializeField] private CinemachineImpulseSource impulse;

    void Awake()
    {
        Instance = this;
    }

    public void Fire(float force)
    {
        impulse.GenerateImpulse(force);
    }
}