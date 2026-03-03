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

class ActiveChunk
{
    public MonoBehaviour behaviour;
    public IWorldChunk chunk;
    public bool isInView;

}

