using UnityEngine;
using System;

public class ComboSystem : MonoBehaviour
{
    public static ComboSystem Instance;

    [Header("Settings")]
    [SerializeField] private float comboTimeout = 2.5f;

    public int CurrentCombo { get; private set; }

    public event Action<int> OnComboChanged;
    public event Action OnComboReset;

    private float timer;
    private bool comboActive;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (!comboActive)
            return;

        timer -= Time.deltaTime;

        if (timer <= 0f)
            ResetCombo();
    }

    // Вызвать при Dock
    public void RegisterDock(bool isPerfect)
    {
        if (isPerfect)
        {
            CurrentCombo++;
            comboActive = true;
            timer = comboTimeout;

            OnComboChanged?.Invoke(CurrentCombo);
        }
        else
        {
            ResetCombo();
        }
    }

    public void ResetCombo()
    {
        if (CurrentCombo > 0)
        {
            CurrentCombo = 0;
            comboActive = false;
            OnComboReset?.Invoke();
        }
    }
}
