using UnityEngine;

public class DangerIndicatorUI : MonoBehaviour
{
    [SerializeField] private RectTransform rect;

    private Transform target;
    private bool active;

    public void SetTarget(Transform hazard)
    {
        target = hazard;

        Camera worldCam = GameManager.Instance.MainCamera;
        Camera uiCam = GameManager.Instance.UICamera; // ВАЖНО!

        Vector3 screenPos = worldCam.WorldToScreenPoint(target.position);

        RectTransform canvasRect = rect.root as RectTransform;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            screenPos,
            uiCam, // ← ВОТ КЛЮЧЕВОЙ МОМЕНТ
            out Vector2 localPoint
        );

        float rightEdge = canvasRect.rect.width / 2f;

        rect.localPosition = new Vector3(
            rightEdge - 100,
            localPoint.y,
            0f
        );

        gameObject.SetActive(true);
    }

    public void Hide()
    {
        active = false;
        gameObject.SetActive(false);
    }
}