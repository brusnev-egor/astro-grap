using UnityEngine;

public class Chunk_MediumVerticalShift : WorldChunkBase
{
    [Header("References")]
    [SerializeField] private Transform station;
    [SerializeField] private GameObject obstacleAbove;
    [SerializeField] private GameObject obstacleBelow;

    [Header("Tuning")]
    [SerializeField] private float verticalOffset = 2.5f;

    public override void Initialize(ChunkContext context)
    {
        base.Initialize(context);

        bool shiftUp = Random.value > 0.5f;

        // Сброс
        obstacleAbove.SetActive(false);
        obstacleBelow.SetActive(false);

        Vector3 pos = station.localPosition;

        if (shiftUp)
        {
            pos.y += verticalOffset;
            obstacleBelow.SetActive(true);
        }
        else
        {
            pos.y -= verticalOffset;
            obstacleAbove.SetActive(true);
        }

        station.localPosition = pos;
    }

    public override void OnEnterView()
    {
        base.OnEnterView();
        // Можно добавить лёгкую пульсацию станции
    }

    public override void OnExitView()
    {
        base.OnExitView();
    }
}
