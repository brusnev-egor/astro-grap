using UnityEngine;

public class EasyChunk : WorldChunkBase
{
    [Header("Lanes")]
    [SerializeField] protected float[] lanes = new float[] { -4f, 0f, 4f };
    protected override float ChunkWidth
    {
        get
        {
            return ChunkSizeConfigGetter.EasyChunkLength;
        }
    }
}