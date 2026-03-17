using UnityEngine;
using System.Collections.Generic;

public class BackgroundLayerSpawner : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private Camera cam;

    [Header("Spawn Entries")]
    [SerializeField] private BackgroundSpawnEntry[] entries;

    [Header("Settings")]
    [SerializeField] private float chunkWidth = 100f;
    [SerializeField] private int chunksOnScreen = 3;

    [Header("Lanes")]
    [SerializeField] private float[] spawnLanes;

    private List<GameObject> activeChunks = new();

    void Start()
    {
        float camX = cam.transform.position.x;

        // ❗ стартуем с центра камеры
        float startX = camX - chunkWidth;

        for (int i = 0; i < chunksOnScreen; i++)
        {
            SpawnAt(startX + i * chunkWidth);
        }
    }

    void Update()
    {
        float camX = cam.transform.position.x;

        float camLeft = camX - GetHalfWidth();
        float camRight = camX + GetHalfWidth();

        // 👉 СПАВН СПРАВА
        while (GetRightmostX() < camRight + chunkWidth)
        {
            SpawnAt(GetRightmostX() + chunkWidth);
        }

        // 👉 УДАЛЕНИЕ СЛЕВА
        for (int i = activeChunks.Count - 1; i >= 0; i--)
        {
            float chunkRight = activeChunks[i].transform.position.x + chunkWidth / 2f;

            if (chunkRight < camLeft - chunkWidth)
            {
                Destroy(activeChunks[i]);
                activeChunks.RemoveAt(i);
            }
        }
    }

    // =========================
    // SPAWN
    // =========================

    void SpawnAt(float x)
    {
        BackgroundSpawnEntry entry = PickEntry();

        if (entry == null || entry.prefab == null)
            return;

        Vector3 pos = new Vector3(
            x,
            PickLaneY(),
            transform.position.z
        );

        GameObject chunk = Instantiate(
            entry.prefab,
            pos,
            entry.prefab.transform.rotation,
            transform
        );

        activeChunks.Add(chunk);
    }

    float GetRightmostX()
    {
        if (activeChunks.Count == 0)
            return cam.transform.position.x;

        float max = float.MinValue;

        foreach (var c in activeChunks)
        {
            if (c == null) continue;
            if (c.transform.position.x > max)
                max = c.transform.position.x;
        }

        return max;
    }

    float GetHalfWidth()
    {
        float height = cam.orthographicSize;
        float width = height * cam.aspect;
        return width;
    }

    // =========================
    // PICK
    // =========================

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

        return spawnLanes[Random.Range(0, spawnLanes.Length)];
    }
}