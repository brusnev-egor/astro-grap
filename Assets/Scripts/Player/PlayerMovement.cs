using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
    }

    private void FixedUpdate()
    {
        float speed = GameManager.Instance.CurrentSpeed;
        rb.linearVelocity = new Vector2(speed, rb.linearVelocity.y);
    }
}