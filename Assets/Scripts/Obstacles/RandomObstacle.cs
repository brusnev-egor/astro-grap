using System.Linq;
using UnityEngine;

[System.Serializable]
public class ObstacleConfig
{
    public bool Enabled = true;
    public GameObject prefab;
    public Material[] materials;
}

public class RandomObstacle : MonoBehaviour
{
    public ObstacleConfig[] asteroidConfigs;
    public Transform spawnPoint;

    void Start()
    {
        SpawnAsteroid();
    }

    void SpawnAsteroid()
    {
        if (asteroidConfigs.Length == 0) return;

        var filteredConfigs = asteroidConfigs.Where((config) => config.Enabled).ToArray();
        // выбираем конфиг
        ObstacleConfig config = filteredConfigs[Random.Range(0, filteredConfigs.Length)];

        // спавним модель
        GameObject asteroid = Instantiate(
            config.prefab,
            spawnPoint
        );

        // применяем материал из этого же конфига
        ApplyMaterial(asteroid, config.materials);
    }

    void ApplyMaterial(GameObject obj, Material[] mats)
    {
        if (mats == null || mats.Length == 0) return;

        Material randomMat = mats[Random.Range(0, mats.Length)];

        foreach (var rend in obj.GetComponentsInChildren<Renderer>())
        {
            rend.material = new Material(randomMat);
        }
    }
}