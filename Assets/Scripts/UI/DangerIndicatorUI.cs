using UnityEngine;

public class DangerIndicatorUI : MonoBehaviour
{
    [SerializeField] private RectTransform rect;

    private Transform target;
    private bool active;

    public void SetTarget(Transform hazard)
    {
        target = hazard;

        Camera worldCamera = GameManager.Instance.MainCamera;

        Vector3 screenPos = worldCamera.WorldToScreenPoint(target.position);

        // фиксируем X справа
        float fixedX = transform.position.x;

        rect.position = new Vector3(
            fixedX,
            screenPos.y,
            0f
        );
        active = true;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        active = false;
        gameObject.SetActive(false);
    }
}
