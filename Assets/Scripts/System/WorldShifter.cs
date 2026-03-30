using UnityEngine;

public class WorldShifter : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform world;

    [Header("Settings")]
    [SerializeField] private float shiftThresholdX = 5f;

    private bool paused;

    void LateUpdate()
    {
        if (paused || !player)
            return;

        if (player.position.x > shiftThresholdX)
        {
            float delta = player.position.x - shiftThresholdX;

            // 1️⃣ двигаем мир влево
            world.position -= new Vector3(delta, 0f, 0f);

            // 2️⃣ возвращаем игрока на threshold
            player.position -= new Vector3(delta, 0f, 0f);
        }
    }

    public void SetPaused(bool value)
    {
        paused = value;
    }
}