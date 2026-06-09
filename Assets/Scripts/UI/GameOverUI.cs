using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject _gameOverMenuUI;
    [SerializeField] private TextMeshProUGUI _description;
    [SerializeField] private TextMeshProUGUI _reviveButtonText;
    [SerializeField] private GameObject _reviveAdButton;
    [SerializeField] private Image _reviveCoinButton;
    [SerializeField] Volume _blurVolume;

    [Header("Color")]
    [ColorUsage(true, true)]
    [SerializeField] private Color _disabledReviveColor;

    private bool _isWatchedAd = false;
    private int _reviveCoinCount = 0;
    private Material _reviveMaterial;

    void Awake()
    {
        _reviveMaterial = Instantiate(_reviveCoinButton.material);
        _reviveCoinButton.material = _reviveMaterial;
    }

    void Start()
    {
        GameManager.Instance.OnGameOver += ShowUI;
    }

    public void ReviveCoin()
    {
        if (CanRevive())
        {
            _reviveCoinCount++;
            Revive();
        }
    }

    public void ReviveAd()
    {
        _isWatchedAd = true;
        Revive();
    }

    public void Quit()
    {
        GameManager.Instance.Quit();
        _gameOverMenuUI.SetActive(false);
    }

    private void ShowUI()
    {
        int cost = GetReviveCost();
        _description.text = $"You earned <color=#FFFF00>{CurrencyManager.Instance.Current}</color><sprite=0>";
        _reviveButtonText.text = $"Revive - <color=#FFFF00>{cost}</color><sprite=0>";
        if (!CanRevive())
        {
            _reviveMaterial.SetColor("_ColorA", _disabledReviveColor);
            _reviveMaterial.SetColor("_ColorB", _disabledReviveColor);
        }
        if (_isWatchedAd && _reviveAdButton)
        {
            _reviveAdButton.SetActive(false);
        }

        _blurVolume.weight = 1;
        _gameOverMenuUI.SetActive(true);
    }

    private int GetReviveCost()
    {
        return (_reviveCoinCount + 1) * 100;
    }

    private bool CanRevive()
    {
        int cost = GetReviveCost();

        return CurrencyManager.Instance.Current >= cost;
    }

    private void Revive()
    {
        GameManager.Instance.Revive();
        _gameOverMenuUI.SetActive(false);
        _blurVolume.weight = 0;

        int cost = GetReviveCost();
        CurrencyManager.Instance.Add(-cost);
    }
}