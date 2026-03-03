using UnityEngine;

public class BackgroundTiling : MonoBehaviour
{
    [SerializeField] private float tileWidth = 20f;

    private Transform[] tiles;

    private void Awake()
    {
        tiles = new Transform[transform.childCount];
        for (int i = 0; i < tiles.Length; i++)
        {
            tiles[i] = transform.GetChild(i);
        }
    }

    public void Shift(float deltaX)
    {
        foreach (var tile in tiles)
        {
            tile.position += Vector3.left * deltaX;
        }

        RepositionIfNeeded();
    }

    private void RepositionIfNeeded()
    {
        foreach (var tile in tiles)
        {
            if (tile.position.x < -tileWidth)
            {
                tile.position += Vector3.right * tileWidth * tiles.Length;
            }
        }
    }
}
