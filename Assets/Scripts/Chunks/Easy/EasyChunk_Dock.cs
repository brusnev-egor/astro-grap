using UnityEngine;

// TODO: replace with EasyChunk_Dock_Obstacle
public class EasyChunk_Dock : EasyChunk
{
    [Header("Dock")]
    [SerializeField] private GameObject dockPrefab;

    public override void Initialize(ChunkContext context)
    {
        base.Initialize(context);

        SpawnDock();
    }

    void SpawnDock()
    {
        if (dockPrefab == null)
        {
            Debug.LogError("Dock prefab not assigned");
            return;
        }

        int laneIndex = Random.Range(0, Lanes.Count);
        float laneY = Lanes[laneIndex];

        Vector3 pos = new Vector3(
            transform.position.x + ChunkWidth / 2,
            laneY,
            transform.position.z
        );

        Instantiate(dockPrefab, pos, Quaternion.identity, transform);
    }
}