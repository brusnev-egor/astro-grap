using UnityEngine;
using System.Collections.Generic;

public class BackgroundLayerSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform worldTransform;

    [Header("Spawn Entries")]
    [SerializeField] private BackgroundSpawnEntry[] entries;

    [Header("Settings")]
    [SerializeField] private float chunkWidth = 100f;
    [SerializeField] private int chunksAhead = 3;

    [Header("Lanes")]
    [SerializeField] private float[] spawnLanes;
    [SerializeField] private float laneRandomOffset = 5f;

    private List<GameObject> activeChunks = new();

    // 👉 теперь это "дистанция", а не world position
    private float nextSpawnDistance;

    void Start()
    {
        nextSpawnDistance = 0f;

        for (int i = 0; i < chunksAhead; i++)
        {
            SpawnChunk();
        }
    }

    void Update()
    {
        float worldDistance = -worldTransform.position.x;

        // ✅ правильный spawn loop (без дыр)
        while (worldDistance + chunksAhead * chunkWidth > nextSpawnDistance)
        {
            SpawnChunk();
        }

        Cleanup(worldDistance);
    }

    void SpawnChunk()
    {
        BackgroundSpawnEntry entry = PickEntry();

        float spawnX = nextSpawnDistance;

        Vector3 pos = new Vector3(
            spawnX,
            PickLaneY(),
            transform.position.z
        );

        if (entry != null && entry.prefab != null)
        {
            GameObject chunk = Instantiate(
                entry.prefab,
                pos,
                entry.prefab.transform.rotation,
                transform
            );

            activeChunks.Add(chunk);
        }

        // ❗ ВСЕГДА двигаем дистанцию
        nextSpawnDistance += chunkWidth;
    }

    BackgroundSpawnEntry PickEntry()
    {
        List<BackgroundSpawnEntry> candidates = new();

        foreach (var entry in entries)
        {
            if (entry.prefab == null)
                continue;

            if (Random.value <= entry.spawnChance)
                candidates.Add(entry);
        }

        if (candidates.Count == 0)
            return null;

        float totalWeight = 0f;

        foreach (var c in candidates)
            totalWeight += c.weight;

        float roll = Random.value * totalWeight;

        foreach (var c in candidates)
        {
            roll -= c.weight;
            if (roll <= 0f)
                return c;
        }

        return candidates[0];
    }

    float PickLaneY()
    {
        if (spawnLanes == null || spawnLanes.Length == 0)
            return transform.position.y;

        float lane = spawnLanes[Random.Range(0, spawnLanes.Length)];

        return lane;
    }

    void Cleanup(float worldDistance)
    {
        float destroyDistance = worldDistance - chunkWidth * 2f;

        for (int i = activeChunks.Count - 1; i >= 0; i--)
        {
            if (activeChunks[i] == null)
            {
                activeChunks.RemoveAt(i);
                continue;
            }

            float chunkDistance = activeChunks[i].transform.position.x;

            if (chunkDistance < destroyDistance)
            {
                Destroy(activeChunks[i]);
                activeChunks.RemoveAt(i);
            }
        }
    }
}