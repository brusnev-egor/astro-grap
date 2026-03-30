using System.Collections.Generic;
using UnityEngine;

public interface IWorldChunk
{
    /// <summary>
    /// Вызывается сразу после спавна чанка
    /// </summary>
    void Initialize(ChunkContext context);

    /// <summary>
    /// Когда чанк впервые появляется в камере
    /// </summary>
    void OnEnterView();

    /// <summary>
    /// Когда чанк полностью покидает камеру
    /// </summary>
    void OnExitView();
}

public class ChunkObjectsData
{
    public List<Transform> docks = new();
    public List<Transform> obstacles = new();

    public List<float> occupiedLanes = new();
}

public interface IChunkComponent
{
    void OnChunkInitialize(WorldChunkBase chunk, ChunkContext context);
    void OnChunkEnterView();
    void OnChunkExitView();
    void OnSetParams(ChunkParams chunkParams);
    void OnChunkObjectsGenerated(ChunkObjectsData data);
}

public interface IConfigurableChunk
{
    void Apply(ChunkParams parameters);
}


class ActiveChunk
{
    public MonoBehaviour behaviour;
    public IWorldChunk chunk;
    public bool isInView;
}
