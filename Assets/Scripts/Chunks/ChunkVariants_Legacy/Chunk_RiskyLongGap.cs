using UnityEngine;

public class Chunk_RiskyLongGap : WorldChunkBase
{
    [Header("References")]
    [SerializeField] private Transform station;
    [SerializeField] private GameObject riskVisual; // опционально (пульсация, glow)

    [Header("Tuning")]
    [SerializeField] private float minXOffset = 12f;
    [SerializeField] private float maxXOffset = 15f;
    [SerializeField] private float maxYOffset = 1.2f;

    public override void Initialize(ChunkContext context)
    {
        base.Initialize(context);

        Vector3 pos = station.localPosition;

        // Далёкая дистанция по X
        pos.x = Random.Range(minXOffset, maxXOffset);

        // Небольшой разброс по Y (чтобы не было всегда идеально ровно)
        pos.y += Random.Range(-maxYOffset, maxYOffset);

        station.localPosition = pos;

        // Визуальный намёк на риск
        if (riskVisual != null)
        {
            riskVisual.SetActive(true);
        }
    }

    public override void OnEnterView()
    {
        base.OnEnterView();

        // Тут можно включить:
        // - усиленное свечение станции
        // - звук «опасности»
        // - лёгкую пульсацию
    }

    public override void OnExitView()
    {
        base.OnExitView();
    }
}
