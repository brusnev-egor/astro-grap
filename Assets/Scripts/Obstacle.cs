using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private bool isTriggered;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isTriggered)
            return;

        if (!collision.collider.CompareTag("Player"))
            return;

        isTriggered = true;

        GameManager.Instance.OnPlayerHitObstacle();
    }
}
