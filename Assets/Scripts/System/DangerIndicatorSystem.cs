using System.Collections.Generic;
using UnityEngine;

public class DangerIndicatorSystem : MonoBehaviour
{
    public static DangerIndicatorSystem Instance;

    [SerializeField] private DangerIndicatorUI _indicatorPrefab;
    [SerializeField] private Canvas _canvas;

    private List<DangerIndicatorUI> _indicatorObjects = new List<DangerIndicatorUI>();

    void Awake()
    {
        Instance = this;
    }

    public DangerIndicatorUI Show(Transform hazard)
    {
        DangerIndicatorUI indicator = Instantiate(_indicatorPrefab, _canvas.transform);
        _indicatorObjects.Add(indicator);
        Debug.Log("Set target" + hazard.position);
        indicator.SetTarget(hazard);
        return indicator;
    }

    public void Hide(DangerIndicatorUI indicatorUI)
    {
        indicatorUI.Hide();
        _indicatorObjects.Remove(indicatorUI);
        Destroy(indicatorUI);
    }
}
