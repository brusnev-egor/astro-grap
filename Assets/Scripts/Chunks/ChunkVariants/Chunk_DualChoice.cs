using UnityEngine;

public class Chunk_DualChoice : WorldChunkBase
{
    [Header("References")]
    [SerializeField] private Transform stationSafe;
    [SerializeField] private Transform stationRisky;

    [Header("Tuning")]
    [SerializeField] private float safeMinX = 6f;
    [SerializeField] private float safeMaxX = 8f;

    [SerializeField] private float riskyMinX = 11f;
    [SerializeField] private float riskyMaxX = 14f;

    [SerializeField] private float verticalOffset = 2.5f;

    public override void Initialize(ChunkContext context)
    {
        base.Initialize(context);

        bool riskyOnTop = Random.value > 0.5f;

        // X позиции
        Vector3 safePos = stationSafe.localPosition;
        safePos.x = Random.Range(safeMinX, safeMaxX);

        Vector3 riskyPos = stationRisky.localPosition;
        riskyPos.x = Random.Range(riskyMinX, riskyMaxX);

        // Y позиции
        if (riskyOnTop)
        {
            riskyPos.y += verticalOffset;
            safePos.y -= verticalOffset;
        }
        else
        {
            riskyPos.y -= verticalOffset;
            safePos.y += verticalOffset;
        }

        stationSafe.localPosition = safePos;
        stationRisky.localPosition = riskyPos;
    }
}
