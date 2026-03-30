using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(WorldChunkBase))]
public class ChunkCollectibleGenerator : MonoBehaviour, IChunkComponent
{
    enum SpawnSegment
    {
        Left,
        Middle,
        Right
    }

    enum Pattern
    {
        Line,
        Offset
    }

    [Header("Setup")]
    [SerializeField] private GameObject collectiblePrefab;

    [Header("Spawn chance")]
    [Range(0f, 1f)]
    [SerializeField] private float spawnChance = 0.7f;
    [SerializeField] private float minDistanceFromObjects = 1.2f;

    [Header("Amount")]
    [SerializeField] private int minCount = 3;
    [SerializeField] private int maxCount = 6;

    private WorldChunkBase _chunk;
    private List<float> _lanes;
    private float _chunkWidth;

    void Awake()
    {
        _chunk = GetComponent<WorldChunkBase>();
        _lanes = _chunk.Lanes;
        _chunkWidth = _chunk.ChunkWidth;
    }

    public void OnChunkInitialize(WorldChunkBase chunk, ChunkContext context) { }
    public void OnChunkEnterView() { }
    public void OnChunkExitView() { }
    public void OnSetParams(ChunkParams chunkParams) { }

    public void OnChunkObjectsGenerated(ChunkObjectsData data)
    {
        if (collectiblePrefab == null)
            return;

        if (Random.value > spawnChance)
            return;

        var pattern = PickPattern(data);

        switch (pattern)
        {
            case Pattern.Line:
                SpawnLine(data);
                break;

            case Pattern.Offset:
                SpawnOffset(data);
                break;
        }
    }

    // =========================================================
    // PATTERNS
    // =========================================================

    Pattern PickPattern(ChunkObjectsData data)
    {
        // если есть dock → чаще line
        // if (data.docks.Count > 0 && Random.value < 0.6f)
        //     return Pattern.Line;

        // return Pattern.Offset;
        return Pattern.Line;
    }

    SpawnSegment PickSegment()
    {
        float roll = Random.value;

        // if (roll < 0.4f) return SpawnSegment.Left;
        if (roll < 0.6f) return SpawnSegment.Middle;

        return SpawnSegment.Right;
    }

    // =========================================================
    // LINE (по станции)
    // =========================================================

    void SpawnLine(ChunkObjectsData data)
    {
        if (data.docks.Count == 0)
            return;

        float lane = data.docks[Random.Range(0, data.docks.Count)].position.y;

        SpawnAlongLane(lane, data);
    }

    // =========================================================
    // OFFSET (рядом со станцией)
    // =========================================================

    void SpawnOffset(ChunkObjectsData data)
    {
        if (data.docks.Count == 0)
            return;

        float dockLane = data.docks[Random.Range(0, data.docks.Count)].position.y;

        // исключаем lane станции и занятые obstacle
        var blockedLanes = data.obstacles
            .Select(o => o.position.y)
            .ToHashSet();

        var validLanes = _lanes
            .Where(l => l != dockLane && !blockedLanes.Contains(l))
            .ToList();

        if (validLanes.Count == 0)
            return;

        float lane = validLanes[Random.Range(0, validLanes.Count)];

        SpawnAlongLane(lane, data);
    }

    // =========================================================
    // CORE SPAWN
    // =========================================================

    void SpawnAlongLane(float laneY, ChunkObjectsData data)
    {
        int count = Random.Range(minCount, maxCount + 1);

        float chunkStartX = transform.position.x;
        float width = _chunkWidth;

        SpawnSegment segment = PickSegment();

        float segmentStart = 0f;
        float segmentEnd = 1f;

        switch (segment)
        {
            case SpawnSegment.Left:
                segmentStart = 0f;
                segmentEnd = 0.33f;
                break;

            case SpawnSegment.Middle:
                segmentStart = 0.33f;
                segmentEnd = 0.66f;
                break;

            case SpawnSegment.Right:
                segmentStart = 0.66f;
                segmentEnd = 1f;
                break;
        }

        float segmentWidth = width * (segmentEnd - segmentStart);
        float startX = chunkStartX + width * segmentStart;

        float spacing = segmentWidth / (count + 1);

        for (int i = 1; i <= count; i++)
        {
            float x = startX + spacing * i;

            Vector3 pos = new Vector3(x, laneY, transform.position.z);

            if (!IsPositionFree(pos, data))
                continue;

            Instantiate(collectiblePrefab, pos, Quaternion.identity, transform);
        }
    }

    bool IsPositionFree(Vector3 pos, ChunkObjectsData data)
    {
        foreach (var dock in data.docks)
        {
            if (Vector2.Distance(pos, dock.position) < minDistanceFromObjects)
                return false;
        }

        foreach (var obstacle in data.obstacles)
        {
            if (Vector2.Distance(pos, obstacle.position) < minDistanceFromObjects)
                return false;
        }

        return true;
    }
}