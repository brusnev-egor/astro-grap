using UnityEngine;
using System.Collections.Generic;

public class SituationSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform world;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private SituationDirector director;

    [Header("Spawn settings")]
    [SerializeField] private int situationsAhead = 3;
    [SerializeField] private float destroyBehindDistance = 60f;

    private List<ActiveChunk> activeChunks = new();

    private float nextChunkX;
    private float chunkLength;

    private float visibilityPadding = 10f;

    void Update()
    {
        chunkLength = GetChunkLength();
        SpawnSituationsAhead();
        UpdateChunkVisibility();
        CleanupBehind();
    }

    void SpawnSituationsAhead()
    {
        float worldDistance = -world.position.x;

        while (nextChunkX < worldDistance + situationsAhead * chunkLength)
        {
            SpawnSituation();
        }
    }

    void SpawnSituation()
    {
        SituationDefinition situation = director.GetNextSituation();

        foreach (var entry in situation.chunks)
        {
            GameObject chunkGO = Instantiate(entry.prefab, world);

            
            if (chunkGO.TryGetComponent<IConfigurableChunk>(out var configurableChunk))
                configurableChunk.Apply(entry.parameters);

            chunkGO.transform.localPosition =
                new Vector3(nextChunkX, 0f, 0f);

            if (!chunkGO.TryGetComponent<IWorldChunk>(out var chunk))
            {
                Debug.LogError("Chunk prefab must implement IWorldChunk");
                continue;
            }

            ChunkContext context = new()
            {
                WorldRoot = world,
                MainCamera = mainCamera
            };

            chunk.Initialize(context);

            activeChunks.Add(new ActiveChunk
            {
                behaviour = chunk as MonoBehaviour,
                chunk = chunk
            });

            nextChunkX += chunkLength;
        }
    }
    void UpdateChunkVisibility()
    {
        float distanceToChunkPlane = 0f - mainCamera.transform.position.z;

        float camLeft =
            mainCamera.ViewportToWorldPoint(new Vector3(0, 0, distanceToChunkPlane)).x;

        float camRight =
            mainCamera.ViewportToWorldPoint(new Vector3(1, 0, distanceToChunkPlane)).x;

        for (int i = 0; i < activeChunks.Count; i++)
        {
            var entry = activeChunks[i];

            if (entry.behaviour == null)
                continue;

            float half = chunkLength * 0.5f;

            float chunkLeft = entry.behaviour.transform.position.x - half;
            float chunkRight = entry.behaviour.transform.position.x + half;

            bool nowInView =
                chunkRight > camLeft - visibilityPadding &&
                chunkLeft < camRight + visibilityPadding;

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

    private float GetChunkLength()
    {
        // return ChunkSizeConfigGetter.MediumChunkLength;
        switch (DifficultyManager.Instance.CurrentDifficulty)
        {
            case DifficultyLevel.Medium:
                return ChunkSizeConfigGetter.MediumChunkLength;

            case DifficultyLevel.Hard:
                return ChunkSizeConfigGetter.HardChunkLength;

            default:
                return ChunkSizeConfigGetter.EasyChunkLength;
        }
    }
}