using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Collectible : MonoBehaviour
{
    [SerializeField] private int value = 1;

    [Header("FX")]
    [SerializeField] private GameObject collectVfx;

    [SerializeField] private float magnetDistance = 2.5f;
    [SerializeField] private float magnetSpeed = 10f;

    private bool collected;

    void Awake()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (collected)
            return;

        if (!other.CompareTag("Player"))
            return;

        Collect();
    }

    void Collect()
    {
        collected = true;

        // Добавляем валюту
        CurrencyManager.Instance.Add(value);

        // FX
        if (collectVfx != null)
            Instantiate(collectVfx, transform.position, Quaternion.identity);

        // Можно добавить звук тут или через AudioManager
        // AudioManager.Instance.Play(...);

        Destroy(gameObject);
    }
}