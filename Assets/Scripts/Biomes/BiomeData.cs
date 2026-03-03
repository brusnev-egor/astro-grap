using UnityEngine;

[CreateAssetMenu(menuName = "Space/Biome")]
public class BiomeData : ScriptableObject
{
    [Header("Info")]
    public string biomeName;

    [Header("Activation")]
    [Tooltip("Минимальная дистанция, с которой этот биом может появиться")]
    public float minDistance;

    [Header("Background")]
    public GameObject[] backgroundPrefabs;

    [Header("Visual")]
    public Color ambientColor = Color.white;
    public Color colorFilter = Color.white;

    [Header("Gameplay")]
    [Range(0.5f, 2f)]
    public float difficultyMultiplier = 1f;

    [Header("Audio")]
    public AudioClip music;
}
