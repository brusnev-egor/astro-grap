using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(WorldChunkBase))]
public class ChunkObstacleDockGenerator: MonoBehaviour, IChunkComponent
{
    [SerializeField] private GameObject[] _dockPrefabs;
    [SerializeField] private GameObject[] _obstaclePrefabs;
    [SerializeField] private float _xSlot;
    private ChunkParams _params;
    private List<float> _lanes;
    private float _chunkWidth;
    private ChunkObjectsData _generatedData = new();

    private WorldChunkBase _worldChunk;
    private IChunkComponent[] _chunkComponents;

    void Awake()
    {
        _worldChunk = GetComponent<WorldChunkBase>();
        _chunkComponents = GetComponents<IChunkComponent>();
        _lanes = _worldChunk.Lanes;
        _chunkWidth = _worldChunk.ChunkWidth;
        if (_xSlot == 0)
        {
            _xSlot = _chunkWidth / 2;
        }
    }

    public void Generate()
    {
        if (_params == null)
            return;

        _generatedData = new ChunkObjectsData();

        if (_params.random)
            SpawnRandom();
        else
            SpawnSpecific();

        NotifyComponents();
    }

    private void NotifyComponents()
    {
        foreach (var c in _chunkComponents)
        {
            if (c == this)
                continue;

            c.OnChunkObjectsGenerated(_generatedData);
        }
    }

    private void SpawnSpecific()
    {
        SpawnObjectsSpecific(_dockPrefabs, _params.dockLanes);
        SpawnObjectsSpecific(_obstaclePrefabs, _params.obstacleLanes);
    }

    private void SpawnRandom()
    {
        // Spawn Docks
        SpawnObjectsRandom(_dockPrefabs, _params.dockCount);

        // Spawn Obstacles
        SpawnObjectsRandom(_obstaclePrefabs, _params.obstacleCount);
    }

    private void SpawnObjectsRandom(GameObject[] prefabs, int count)
    {
        if (count == 0)
        {
            return;
        }
        if (prefabs.Length == 0)
            {
                // Debug.LogError("Prefab not assigned");
                return;
            }

        do
        {
            if (_lanes.Count > 0)
            {
                GameObject prefab = PickRandomObject(prefabs);
                int laneIndex = Random.Range(0, _lanes.Count);
                float laneY = _lanes[laneIndex];
                _lanes.RemoveAt(laneIndex);

                SpawnObject(prefab, laneY);
            }
        }
        while (--count > 0);
    }

    private void SpawnObjectsSpecific(GameObject[] prefabs, int[] indexes)
    {
        foreach (int index in indexes)
        {
            float laneY = _lanes[index];
            GameObject prefab = PickRandomObject(prefabs);
            SpawnObject(prefab, laneY);
        }
    }

    private void SpawnObject(GameObject prefab, float laneY)
    {
        Vector3 pos = new(
            transform.position.x + _xSlot,
            laneY,
            transform.position.z
        );

        GameObject go = Instantiate(prefab, pos, Quaternion.identity, transform);

        if (go.CompareTag("Dock"))
            _generatedData.docks.Add(go.transform);
        else
            _generatedData.obstacles.Add(go.transform);

        _generatedData.occupiedLanes.Add(laneY);
    }

    private GameObject PickRandomObject(GameObject[] prefabs)
    {
        int index = Random.Range(0, prefabs.Length);
        return prefabs[index];
    }

    public void OnChunkInitialize(WorldChunkBase chunk, ChunkContext context)
    {
        Generate();
    }

    public void OnChunkEnterView()
    {
        // throw new System.NotImplementedException();
    }

    public void OnChunkExitView()
    {
        // throw new System.NotImplementedException();
    }

    public void OnSetParams(ChunkParams chunkParams)
    {
        _params = chunkParams;
    }

    public void OnChunkObjectsGenerated(ChunkObjectsData data)
    {
        throw new System.NotImplementedException();
    }
}