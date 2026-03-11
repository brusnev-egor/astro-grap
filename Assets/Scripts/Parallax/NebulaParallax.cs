using UnityEngine;

public class NebulaParallax : MonoBehaviour
{
    [SerializeField] float parallaxFactor = 0.05f;

    private Material mat;
    private Vector2 uvOffset;

    void Start()
    {
        mat = GetComponent<MeshRenderer>().material;
    }

    void Update()
    {
        float speed = GameManager.Instance.CurrentSpeed;

        uvOffset.x += speed * parallaxFactor * Time.deltaTime * 0.01f;
        mat.SetVector("_UVOffset", uvOffset);
    }
}