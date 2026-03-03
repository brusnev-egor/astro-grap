using UnityEngine;
using TMPro;

public class HUDDistance : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI distanceText;

    void Update()
    {
        int meters = Mathf.FloorToInt(DistanceTracker.Instance.TotalDistance);
        distanceText.text = $"{meters} m";
    }
}
