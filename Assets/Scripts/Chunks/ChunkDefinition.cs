using UnityEngine;

[System.Serializable]
public class ChunkDefinition
{
    public GameObject prefab;

    [Header("Frequency")]
    public int weight = 1;
    public float width = 14f;

    [Tooltip("Минимальная дистанция между появлениями этого чанка")]
    public float minDistanceBetween = 0f;

    [HideInInspector]
    public float lastSpawnDistance = -999f;
}
