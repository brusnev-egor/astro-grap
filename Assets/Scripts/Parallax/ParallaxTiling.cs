using UnityEngine;

public class ParallaxTiling : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float tileWidth;

    private Transform[] tiles;

    void Awake()
    {
        tiles = new Transform[transform.childCount];
        for (int i = 0; i < tiles.Length; i++)
        {
            tiles[i] = transform.GetChild(i);
        }
    }

    void Update()
    {
        foreach (var tile in tiles)
        {
            if (IsTileBehindCamera(tile))
            {
                MoveTileToFront(tile);
            }
        }
    }

    bool IsTileBehindCamera(Transform tile)
    {
        float cameraLeftEdge = cameraTransform.position.x - tileWidth;
        return tile.position.x + tileWidth < cameraLeftEdge;
    }

    void MoveTileToFront(Transform tile)
    {
        float farthestX = GetFarthestTileX();
        tile.position = new Vector3(
            farthestX + tileWidth,
            tile.position.y,
            tile.position.z
        );
    }

    float GetFarthestTileX()
    {
        float maxX = float.MinValue;
        foreach (var tile in tiles)
        {
            if (tile.position.x > maxX)
                maxX = tile.position.x;
        }
        return maxX;
    }
}
