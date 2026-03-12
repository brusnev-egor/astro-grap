using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private bool isTriggered;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (isTriggered)
            return;

        if (!collision.CompareTag("Player"))
            return;

        isTriggered = true;

        GameManager.Instance.OnPlayerHitObstacle();
    }
}
