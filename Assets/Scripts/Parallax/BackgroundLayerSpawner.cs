using UnityEngine;
using System.Collections.Generic;

public class BackgroundLayerSpawner : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private GameObject[] prefabs;
    [SerializeField] private float chunkWidth = 100f;
    [SerializeField] private int chunksAhead = 3;

    private List<GameObject> activeChunks = new();
    private float nextSpawnX;

    void Start()
    {
        nextSpawnX = cameraTransform.position.x - chunkWidth;

        for (int i = 0; i < chunksAhead; i++)
        {
            SpawnChunk();
        }
    }

    void Update()
    {
        if (cameraTransform.position.x + chunkWidth > nextSpawnX - chunkWidth)
        {
            SpawnChunk();
        }

        Cleanup();
    }

    void SpawnChunk()
    {
        int index = Random.Range(0, prefabs.Length);

        Vector3 pos = new Vector3(nextSpawnX, transform.position.y, transform.position.z);

        GameObject chunk = Instantiate(prefabs[index], pos, prefabs[index].transform.rotation, transform);

        activeChunks.Add(chunk);

        nextSpawnX += chunkWidth;
    }

    void Cleanup()
    {
        float cameraLeftEdge = cameraTransform.position.x - chunkWidth * 2f;

        for (int i = activeChunks.Count - 1; i >= 0; i--)
        {
            if (activeChunks[i].transform.position.x < cameraLeftEdge)
            {
                Destroy(activeChunks[i]);
                activeChunks.RemoveAt(i);
            }
        }
    }
}
