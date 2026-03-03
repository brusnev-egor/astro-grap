using UnityEngine;

public class Chunk_FastIncoming : WorldChunkBase
{
    [SerializeField] private FastHazardObstacle hazard;

    public override void Initialize(ChunkContext context)
    {
        base.Initialize(context);
        DangerIndicatorSystem.Instance.Show(hazard.transform);
    }

    public override void OnEnterView()
    {
        base.OnEnterView();
        DangerIndicatorSystem.Instance.Hide();
        hazard.Launch();
    }
}
