using UnityEngine;

public class ChunkIntercept : MonoBehaviour, IChunkComponent
{
    [SerializeField] private InterceptObstacle _interceptObstacle;
    private DangerIndicatorUI _indicator;
    public void OnChunkEnterView()
    {
        DangerIndicatorSystem.Instance.Hide(_indicator);
        _interceptObstacle.Launch();
    }

    public void OnChunkExitView()
    {}

    public void OnChunkInitialize(WorldChunkBase chunk, ChunkContext context)
    {
        float playerY = GameManager.Instance.PlayerY;

        _interceptObstacle.SetHeight(playerY);

        _indicator = DangerIndicatorSystem.Instance.Show(_interceptObstacle.transform);
    }

    public void OnChunkObjectsGenerated(ChunkObjectsData data)
    {}

    public void OnSetParams(ChunkParams chunkParams)
    {}
}