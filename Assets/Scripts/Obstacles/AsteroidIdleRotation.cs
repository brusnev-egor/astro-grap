using UnityEngine;

public class AsteroidIdleRotation : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private Vector3 rotationAxis = new Vector3(0.2f, 0.7f, 0.4f);

    void Update()
    {
        Debug.Log("Rotate = " + rotationSpeed);
        transform.Rotate(rotationSpeed * Time.deltaTime * rotationAxis);
    }
}