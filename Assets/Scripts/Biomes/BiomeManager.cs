using UnityEngine;
using System;

public class BiomeManager : MonoBehaviour
{
    public static BiomeManager Instance { get; private set; }

    [Header("Biomes")]
    [SerializeField] private BiomeData[] biomes;

    [Header("Settings")]
    [SerializeField] private float transitionDuration = 3f;

    public BiomeData CurrentBiome { get; private set; }

    public event Action<BiomeData> OnBiomeChanged;

    private bool isTransitioning;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        UpdateBiome(force: true);
    }

    void Update()
    {
        // UpdateBiome();
    }

    void UpdateBiome(bool force = false)
    {
        float distance = DistanceTracker.Instance.TotalDistance;

        BiomeData nextBiome = null;

        foreach (var biome in biomes)
        {
            if (distance >= biome.minDistance)
                nextBiome = biome;
        }

        if (nextBiome == null)
            return;

        if (!force && nextBiome == CurrentBiome)
            return;

        CurrentBiome = nextBiome;

        OnBiomeChanged?.Invoke(CurrentBiome);
    }
}
