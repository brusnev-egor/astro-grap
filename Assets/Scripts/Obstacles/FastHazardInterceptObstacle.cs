using UnityEngine;

public class InterceptObstacle : Obstacle
{
    [SerializeField] private float horizontalSpeed = 20f;

    private bool launched;

    public void SetHeight(float y)
    {
        transform.position = new Vector3(
            transform.position.x,
            y,
            transform.position.z
        );
    }

    public void Launch()
    {
        launched = true;
    }

    void Update()
    {
        if (!launched)
            return;

        transform.position -=
            Vector3.right * horizontalSpeed * Time.deltaTime;
    }
}
