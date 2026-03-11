using UnityEngine;

[CreateAssetMenu(menuName = "Background/Background Spawn Entry")]
public class BackgroundSpawnEntry : ScriptableObject
{
    public GameObject prefab;

    [Tooltip("Вероятность появления при попытке спавна")]
    [Range(0f,1f)]
    public float spawnChance = 1f;

    [Tooltip("Вес среди других объектов")]
    public float weight = 1f;
}