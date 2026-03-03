using UnityEngine;

public class DangerIndicatorSystem : MonoBehaviour
{
    public static DangerIndicatorSystem Instance;

    [SerializeField] private DangerIndicatorUI indicator;

    void Awake()
    {
        Instance = this;
        indicator.Hide();
    }

    public void Show(Transform hazard)
    {
        indicator.SetTarget(hazard);
    }

    public void Hide()
    {
        indicator.Hide();
    }
}
