using UnityEngine;
using TMPro;

public class ComboUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI comboText;
    [SerializeField] private CanvasGroup canvasGroup;

    void Start()
    {
        ComboSystem.Instance.OnComboChanged += UpdateCombo;
        ComboSystem.Instance.OnComboReset += HideCombo;

        canvasGroup.alpha = 0f;
    }

    void UpdateCombo(int combo)
    {
        // comboText.text = $"x{combo}";
        // canvasGroup.alpha = 1f;

        // if (combo >= 3)
        // CameraImpulse.Instance.Fire(1.2f);

    }

    void HideCombo()
    {
        canvasGroup.alpha = 0f;
    }
}
