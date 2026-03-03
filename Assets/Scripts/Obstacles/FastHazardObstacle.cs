using UnityEngine;

public class FastHazardObstacle : Obstacle
{
    [SerializeField] private float launchSpeed = 25f;

    private bool launched;

    public void Launch()
    {
        launched = true;
    }

    void Update()
    {
        if (!launched) return;

        var worldSpeed = GameManager.Instance.CurrentSpeed;

        transform.position -= (launchSpeed + worldSpeed) * Time.deltaTime * Vector3.right;
    }
}
