using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _player;

    private Renderer _renderer;
    private MaterialPropertyBlock _mpb;

    static readonly int VelocityID = Shader.PropertyToID("_Velocity");

    void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _mpb = new MaterialPropertyBlock();
    }

    void Update()
    {
        _renderer.GetPropertyBlock(_mpb);

        Vector3 v = _player.linearVelocity;

        _mpb.SetVector(VelocityID, new Vector2(v.x, 0));

        _renderer.SetPropertyBlock(_mpb);
    }
}