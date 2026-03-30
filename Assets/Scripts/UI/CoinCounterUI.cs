using UnityEngine;
using TMPro;

public class CoinCounterUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    [Header("Animation")]
    [SerializeField] private float lerpSpeed = 10f;
    [SerializeField] private float scalePunch = 1.2f;
    [SerializeField] private float scaleSpeed = 10f;

    private int targetValue;
    private float displayedValue;

    private Vector3 baseScale;

    void Awake()
    {
        baseScale = transform.localScale;
    }

    void Start()
    {
        if (CurrencyManager.Instance == null)
        {
            Debug.LogError("CurrencyManager Instance is NULL");
            return;
        }

        CurrencyManager.Instance.OnCurrencyChanged += OnCurrencyChanged;
    }

    void OnDisable()
    {
        CurrencyManager.Instance.OnCurrencyChanged -= OnCurrencyChanged;
    }

    // void Update()
    // {
    //     // плавное увеличение числа
    //     displayedValue = Mathf.Lerp(displayedValue, targetValue, Time.deltaTime * lerpSpeed);

    //     text.text = Mathf.RoundToInt(displayedValue).ToString();

    //     // возврат масштаба
    //     transform.localScale = Vector3.Lerp(
    //         transform.localScale,
    //         baseScale,
    //         Time.deltaTime * scaleSpeed
    //     );
    // }

    void OnCurrencyChanged(int newValue)
    {
        targetValue = newValue;
        text.text = Mathf.RoundToInt(newValue).ToString();
        // “поп” эффект
        // transform.localScale = baseScale * scalePunch;
    }
}