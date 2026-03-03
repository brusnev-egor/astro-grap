using UnityEngine;

public class Chunk_MediumMovingObstacle : WorldChunkBase
{
    [Header("References")]
    [SerializeField] private Transform station;
    [SerializeField] private MovingObstacle movingObstacle;

    [Header("Tuning")]
    [SerializeField] private float stationYOffset = 2.0f;

    public override void Initialize(ChunkContext context)
    {
        base.Initialize(context);

        // Слегка смещаем станцию, чтобы игрок менял траекторию
        Vector3 pos = station.localPosition;
        pos.y += Random.value > 0.5f ? stationYOffset : -stationYOffset;
        station.localPosition = pos;
    }

    public override void OnEnterView()
    {
        base.OnEnterView();
        movingObstacle.StartMovement();
    }

    public override void OnExitView()
    {
        base.OnExitView();
        movingObstacle.StopMovement();
    }
}
