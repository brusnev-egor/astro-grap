using System;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance;

    public int Current { get; private set; }

    public event Action<int> OnCurrencyChanged;

    void Awake()
    {
        Instance = this;
    }

    public void Add(int amount)
    {
        Current += amount;
        OnCurrencyChanged?.Invoke(Current);
    }

    public void Reset()
    {
        Current = 0;
        OnCurrencyChanged?.Invoke(Current);
    }
}