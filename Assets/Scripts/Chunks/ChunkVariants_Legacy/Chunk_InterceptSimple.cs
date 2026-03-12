using UnityEngine;

public class Chunk_InterceptSimple : WorldChunkBase
{
    [SerializeField] private InterceptObstacle obstacle;

    public override void Initialize(ChunkContext context)
    {
        base.Initialize(context);

        float playerY = GameManager.Instance.PlayerY;

        obstacle.SetHeight(playerY);

        DangerIndicatorSystem.Instance.Show(obstacle.transform);
    }

    public override void OnEnterView()
    {
        base.OnEnterView();
        DangerIndicatorSystem.Instance.Hide();
        obstacle.Launch();
    }
}
