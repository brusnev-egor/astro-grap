// using UnityEngine;

// public class EasyChunk_Dock_Obstacle : EasyChunk, IConfigurableChunk
// {
//     [SerializeField] private bool random = true;

//     [Header("Dock")]
//     [SerializeField] private GameObject dockPrefab;

//     [Header("Obstacle")]
//     [SerializeField] private GameObject obstaclePrefab;

//     [Header("Random props")]
//     [SerializeField] private int dockCount = 1;
//     [SerializeField] private int obstacleCount = 1;

//     [Header("Specific props")]
//     [SerializeField] private int[] dockLanes = new int[] { };
//     [SerializeField] private int[] obstacleLanes = new int[] { };

//     public override void Initialize(ChunkContext context)
//     {
//         base.Initialize(context);
//         if (random)
//         {
//             SpawnRandom();
//         }
//         else
//         {
//             SpawnSpecific();
//         }
//     }

//     private void SpawnSpecific()
//     {
//         SpawnObjectsSpecific(dockPrefab, dockLanes);
//         SpawnObjectsSpecific(obstaclePrefab, obstacleLanes);
//     }

//     private void SpawnRandom()
//     {
//         // Spawn Docks
//         SpawnObjectsRandom(dockPrefab, dockCount);

//         // Spawn Obstacles
//         SpawnObjectsRandom(obstaclePrefab, obstacleCount);
//     }

//     private void SpawnObjectsRandom(GameObject prefab, int count)
//     {
//         if (prefab == null)
//         {
//             Debug.LogError("Prefab not assigned");
//             return;
//         }

//         do
//         {
//             if (lanes.Count > 0)
//             {
//                 int laneIndex = Random.Range(0, lanes.Count);
//                 float laneY = lanes[laneIndex];
//                 lanes.RemoveAt(laneIndex);

//                 SpawnObject(prefab, laneY);
//             }
//         }
//         while (--count > 0);
//     }

//     private void SpawnObjectsSpecific(GameObject prefab, int[] indexes)
//     {
//         foreach (int index in indexes)
//         {
//             float laneY = lanes[index];
//             SpawnObject(prefab, laneY);
//         }
//     }

//     private void SpawnObject(GameObject prefab, float laneY)
//     {
//         Vector3 dockPos = new(
//             transform.position.x + ChunkWidth / 2,
//             laneY,
//             transform.position.z
//         );
//         Instantiate(prefab, dockPos, Quaternion.identity, transform);
//     }

//     public void Apply(ChunkParams p)
//     {
//         random = p.random;
//         dockCount = p.dockCount;
//         obstacleCount = p.obstacleCount;

//         dockLanes = p.dockLanes;
//         obstacleLanes = p.obstacleLanes;
//     }
// }