using UnityEngine;

public class Chunk_SimpleStation : WorldChunkBase
{
    [SerializeField] private GameObject station;
    [SerializeField] private GameObject[] obstacles;

    public override void Initialize(ChunkContext context)
    {
        base.Initialize(context);

        // можно динамически включать / выключать объекты
        foreach (var o in obstacles)
            o.SetActive(Random.value > 0.5f);
    }

    public override void OnEnterView()
    {
        base.OnEnterView();
        // включить анимации, движение и т.д.
    }

    public override void OnExitView()
    {
        base.OnExitView();
        // остановить корутины, эффекты
    }
}
