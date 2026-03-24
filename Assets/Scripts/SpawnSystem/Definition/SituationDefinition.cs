using UnityEngine;

[System.Serializable]
public class ChunkParams
{
    public bool random;

    public int dockCount;
    public int obstacleCount;

    public int[] dockLanes;
    public int[] obstacleLanes;
}

[System.Serializable]
public class ChunkEntry
{
    public GameObject prefab;

    public ChunkParams parameters;
}

[CreateAssetMenu(menuName = "Runner/Situation")]
public class SituationDefinition : ScriptableObject
{
    [Header("Chunks sequence")]
    public ChunkEntry[] chunks;

    [Header("Spawn weight")]
    public float weight = 1;
}