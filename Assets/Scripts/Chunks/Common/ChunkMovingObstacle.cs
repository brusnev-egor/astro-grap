using UnityEngine;

public class ChunkMovingObstacle : MonoBehaviour, IChunkComponent
{
    [SerializeField] private MovingObstacle[] _movingObstacles;
    [SerializeField] private bool isRandomPositions = false;

    private WorldChunkBase _worldChunk;

    void Awake()
    {
        _worldChunk = GetComponent<WorldChunkBase>();
    }
    public void OnChunkEnterView()
    {
        var lanes = _worldChunk.Lanes;

        foreach (var obstacle in _movingObstacles)
        {
            if (isRandomPositions)
            {
                int laneIndex = Random.Range(0, lanes.Count);
                var pos = obstacle.transform.position;
                obstacle.transform.position = new Vector3(pos.x, lanes[laneIndex], pos.y);
            }
            obstacle.StartMovement();
        }
    }

    public void OnChunkExitView()
    {
        foreach (var obstacle in _movingObstacles)
        {
            obstacle.StopMovement();
        }
    }

    public void OnChunkInitialize(WorldChunkBase chunk, ChunkContext context)
    {
    }

    public void OnSetParams(ChunkParams chunkParams)
    {
    }
}