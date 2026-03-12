using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ChunkSpawner_Legacy : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform world;
    [SerializeField] private Camera mainCamera;

    [Header("Spawn Settings")]
    [SerializeField] private float chunkWidth = 14f;
    [SerializeField] private int chunksAhead = 4;
    [SerializeField] private float destroyBehindDistance = 30f;

    [Header("Chunk Pools")]
    [SerializeField] private ChunkDefinition[] easyChunks;
    [SerializeField] private ChunkDefinition[] mediumChunks;
    [SerializeField] private ChunkDefinition[] riskyChunks;

    [SerializeField] float slot0 = 4f;
    [SerializeField] float slot1 = 11f;

    private readonly List<ActiveChunk> activeChunks = new();

    private float nextChunkX;
    private int chunkIndex;
    private ChunkIntensity lastIntensity;
    private int riskyStreak;

    void Start()
    {
        nextChunkX = 0f;
        chunkIndex = 0;
    }

    void Update()
    {
        SpawnChunksAhead();
        UpdateChunkVisibility();
        CleanupBehind();
    }

    // =========================================================
    // SPAWNING
    // =========================================================

    void SpawnChunksAhead()
    {
        float worldDistance = -world.position.x;

        while (nextChunkX < worldDistance + chunksAhead * chunkWidth)
        {
            SpawnChunk(nextChunkX);
            nextChunkX += chunkWidth;
            chunkIndex++;
        }
    }

    void SpawnChunk(float localX)
    {
        ChunkIntensity intensity = GetNextIntensity();

        ChunkDefinition def = PickChunkDefinition(intensity, localX);
        if (def == null)
            def = PickAny(easyChunks);

        GameObject chunkGO = Instantiate(def.prefab, world);
        chunkGO.transform.localPosition = new Vector3(localX, 0f, 0f);

        IWorldChunk chunk = chunkGO.GetComponent<IWorldChunk>();
        MonoBehaviour behaviour = chunk as MonoBehaviour;

        if (chunk == null)
        {
            Debug.LogError("Chunk prefab must implement IWorldChunk");
            Destroy(chunkGO);
            return;
        }

        ChunkContext context = new ChunkContext
        {
            ChunkIndex = chunkIndex,
            WorldDistance = localX,
            Difficulty01 = Mathf.Clamp01(chunkIndex / 50f),
            WorldRoot = world,
            MainCamera = mainCamera
        };

        chunk.Initialize(context);

        activeChunks.Add(new ActiveChunk
        {
            behaviour = behaviour,
            chunk = chunk
        });

        def.lastSpawnDistance = localX;
    }

    // =========================================================
    // VISIBILITY
    // =========================================================

    void UpdateChunkVisibility()
    {
        float distanceToChunkPlane = 0f - mainCamera.transform.position.z;

        float camLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, distanceToChunkPlane)).x;
        float camRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 0, distanceToChunkPlane)).x;

        for (int i = 0; i < activeChunks.Count; i++)
        {
            var entry = activeChunks[i];
            if (entry.behaviour == null)
                continue;

            float chunkLeft = entry.behaviour.transform.position.x;
            float chunkRight = chunkLeft + chunkWidth;

            bool nowInView =
                chunkRight > camLeft &&
                chunkLeft < camRight;

            if (nowInView && !entry.isInView)
            {
                entry.isInView = true;
                entry.chunk.OnEnterView();
            }
            else if (!nowInView && entry.isInView)
            {
                entry.isInView = false;
                entry.chunk.OnExitView();
            }
        }
    }

    // =========================================================
    // CLEANUP
    // =========================================================

    void CleanupBehind()
    {
        float worldDistance = -world.position.x;
        float destroyX = worldDistance - destroyBehindDistance;

        for (int i = activeChunks.Count - 1; i >= 0; i--)
        {
            var entry = activeChunks[i];

            if (entry.behaviour == null)
            {
                activeChunks.RemoveAt(i);
                continue;
            }

            if (entry.behaviour.transform.localPosition.x < destroyX)
            {
                entry.chunk.OnExitView();
                Destroy(entry.behaviour.gameObject);
                activeChunks.RemoveAt(i);
            }
        }
    }

    // =========================================================
    // POOLS
    // =========================================================

    ChunkDefinition PickChunkDefinition(ChunkIntensity intensity, float currentDistance)
    {
        ChunkDefinition[] pool = GetPool(intensity);

        var valid = pool
            .Where(c => currentDistance - c.lastSpawnDistance >= c.minDistanceBetween)
            .ToArray();

        if (valid.Length == 0)
            return null;

        return PickWeighted(valid);
    }

    ChunkDefinition[] GetPool(ChunkIntensity intensity)
    {
        // if (intensity == ChunkIntensity.Risky)
        // {
        //     float multiplier = DifficultyManager.Instance.RiskyMultiplier;

        //     if (Random.value > multiplier)
        //         return easyChunks;
        // }

        return intensity switch
        {
            ChunkIntensity.Easy => easyChunks,
            ChunkIntensity.Medium => mediumChunks,
            ChunkIntensity.Risky => riskyChunks,
            _ => easyChunks
        };
    }

    ChunkDefinition PickWeighted(ChunkDefinition[] defs)
    {
        int totalWeight = defs.Sum(d => d.weight);
        int roll = Random.Range(0, totalWeight);

        foreach (var d in defs)
        {
            if (roll < d.weight)
                return d;

            roll -= d.weight;
        }

        return defs[0];
    }

    ChunkDefinition PickAny(ChunkDefinition[] defs)
    {
        return defs[Random.Range(0, defs.Length)];
    }

    ChunkIntensity GetNextIntensity()
    {
        float difficulty = DifficultyManager.Instance.Difficulty01;

        float riskyChance = Mathf.Lerp(0f, 0.6f, difficulty);
        float mediumChance = Mathf.Lerp(0.2f, 0.5f, difficulty);

        // если был risky — снижаем шанс повторения
        if (lastIntensity == ChunkIntensity.Risky)
        {
            riskyChance *= 0.4f;
        }

        float roll = Random.value;

        ChunkIntensity result;

        if (roll < riskyChance)
        {
            result = ChunkIntensity.Risky;
        }
        else if (roll < riskyChance + mediumChance)
        {
            result = ChunkIntensity.Medium;
        }
        else
        {
            result = ChunkIntensity.Easy;
        }

        // защита от 3 risky подряд
        if (result == ChunkIntensity.Risky)
        {
            riskyStreak++;
            if (riskyStreak >= 2)
            {
                result = ChunkIntensity.Medium;
                riskyStreak = 0;
            }
        }
        else
        {
            riskyStreak = 0;
        }

        lastIntensity = result;
        return result;
    }
}