using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class WorldChunkBase : MonoBehaviour, IWorldChunk, IConfigurableChunk
{
    // [HideInInspector] public List<float> lanes;
    protected ChunkContext context;
    protected bool isActive;
    private IChunkComponent[] lifecycleComponents;

    public virtual List<float> Lanes
    {
        get
        {
            return new List<float>();
        }
    }

    public virtual float ChunkWidth
    {
        get
        {
            return 15f;
        }
    }

    protected virtual void Awake()
    {
        lifecycleComponents = GetComponents<IChunkComponent>();
    }


    public virtual void Initialize(ChunkContext context)
    {
        this.context = context;
        foreach (var c in lifecycleComponents)
            c.OnChunkInitialize(this, context);
    }

    public virtual void OnEnterView()
    {
        isActive = true;
        Debug.Log($"{name} ENTER");
        foreach (var c in lifecycleComponents)
            c.OnChunkEnterView();

    }

    public virtual void OnExitView()
    {
        isActive = false;
        Debug.Log($"{name} EXIT");
        foreach (var c in lifecycleComponents)
            c.OnChunkExitView();

    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Debug.Log("Draw gizmo");
        Gizmos.color = Color.cyan;
        float height = Lanes.Count > 0 ? Math.Abs(Lanes[0]) * 2 + 2 : 8f; // + offset
        Gizmos.DrawWireCube(
            transform.position + new Vector3(ChunkWidth / 2f, 0f, 0f),
            new Vector3(ChunkWidth, height, 0.1f)
        );
        foreach (float lane in Lanes)
            Gizmos.DrawLine(new Vector3(transform.position.x, lane, transform.position.z), new Vector3(transform.position.x + ChunkWidth, lane, transform.position.z));
    }

    public void Apply(ChunkParams parameters)
    {
        foreach (var c in lifecycleComponents)
            c.OnSetParams(parameters);
    }
#endif
}
