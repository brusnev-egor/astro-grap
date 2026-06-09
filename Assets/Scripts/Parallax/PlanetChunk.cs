using UnityEngine;

public class PlanetChunk : MonoBehaviour
{
    [SerializeField] private GameObject planet;
    [SerializeField] private float minScale = 1f;
    [SerializeField] private float maxScale = 4f;
    void Start()
    {
        float scale = Random.Range(minScale, maxScale);
        planet.transform.localScale = new Vector3(scale, scale, scale);
    }
}