using UnityEngine;

public class BackgroundCamera : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;

    private float _prevFov;

    void Start()
    {
        transform.localPosition = new Vector3(0, 0, _mainCamera.nearClipPlane + 0.01f);
    }

    void Update()
    {
        float currentFov = _mainCamera.fieldOfView;
        if (_prevFov != currentFov)
        {
            _prevFov = currentFov;

            float d = _mainCamera.nearClipPlane + 0.01f;

            float height = 2f * Mathf.Tan(_mainCamera.fieldOfView * 0.5f * Mathf.Deg2Rad) * d;
            float width = height * _mainCamera.aspect;

            transform.localScale = new Vector3(width, height, 1);
        }
    }
}