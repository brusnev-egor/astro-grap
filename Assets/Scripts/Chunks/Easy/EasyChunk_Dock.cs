using UnityEngine;

public class EasyChunk_Dock : EasyChunk
{
    [Header("Dock")]
    [SerializeField] private GameObject dockPrefab;

    [Header("Spawn")]
    [SerializeField] private Transform spawnPoint;

    public override void Initialize(ChunkContext context)
    {
        base.Initialize(context);

        SpawnDock();
    }

    void SpawnDock()
    {
        Debug.Log("Spawn dock");
        if (dockPrefab == null)
        {
            Debug.LogError("Dock prefab not assigned");
            return;
        }

        if (spawnPoint == null)
        {
            Debug.LogError("Spawn point not assigned");
            return;
        }

        int laneIndex = Random.Range(0, lanes.Length);
        float laneY = lanes[laneIndex];

        Vector3 pos = new Vector3(
            transform.position.x + ChunkWidth / 2,
            laneY,
            spawnPoint.position.z
        );

        Instantiate(dockPrefab, pos, Quaternion.identity, transform);
    }
}