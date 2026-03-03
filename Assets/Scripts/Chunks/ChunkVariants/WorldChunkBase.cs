using UnityEngine;

public abstract class WorldChunkBase : MonoBehaviour, IWorldChunk
{
    protected ChunkContext context;
    protected bool isActive;

    private float chunkWidth = 14f;

    public virtual void Initialize(ChunkContext context)
    {
        this.context = context;
    }

    public virtual void OnEnterView()
    {
        isActive = true;
        Debug.Log($"{name} ENTER");
    }

    public virtual void OnExitView()
    {
        isActive = false;
        Debug.Log($"{name} EXIT");
    }

    #if UNITY_EDITOR
        void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;

            Gizmos.DrawWireCube(
                transform.position + new Vector3(chunkWidth / 2f, 0f, 0f),
                new Vector3(chunkWidth, 8f, 0.1f)
            );
        }
    #endif
}
