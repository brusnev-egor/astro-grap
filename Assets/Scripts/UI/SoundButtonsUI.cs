using UnityEngine;
using UnityEngine.UI;

public class SoundButtonsUI : MonoBehaviour
{
    [Header("Toggles Ref")]
    [SerializeField] private Image _soundBtn;
    [SerializeField] private Image _soundIcon;
    [SerializeField] private Image _musicBtn;
    [SerializeField] private Image _musicIcon;
    [SerializeField] private Image _vibrationBtn;
    [SerializeField] private Image _vibrationIcon;
    [Header("Toggles Color")]
    [ColorUsage(true, true)]
    [SerializeField] private Color _enabledColor;
    [ColorUsage(true, true)]
    [SerializeField] private Color _disabledColor;
    [SerializeField] private Color _disabledIconColor;

    private Material _musicMaterial;
    private Material _soundMaterial;
    private Material _vibrationMaterial;

    private bool _musicEnabled;
    private bool _soundEnabled;
    private bool _vibrationEnabled;

    void Awake()
    {
        _musicEnabled = PlayerPrefsManager.IsMusicEnabled();
        _soundEnabled = PlayerPrefsManager.IsSoundEnabled();
        _vibrationEnabled = PlayerPrefsManager.IsVibrationEnabled();

        _musicMaterial = Instantiate(_musicBtn.material);
        _musicBtn.material = _musicMaterial;

        _soundMaterial = Instantiate(_soundBtn.material);
        _soundBtn.material = _soundMaterial;

        _vibrationMaterial = Instantiate(_vibrationBtn.material);
        _vibrationBtn.material = _vibrationMaterial;

        PaintButton(_musicMaterial, _musicIcon, _musicEnabled);
        PaintButton(_soundMaterial, _soundIcon, _soundEnabled);
        PaintButton(_vibrationMaterial, _vibrationIcon, _vibrationEnabled);
    }

    private void PaintButton(Material material, Image icon,  bool enabled)
    {
        if (enabled)
        {
            material.SetColor("_ColorA", _enabledColor);
            material.SetColor("_ColorB", _enabledColor);
            icon.color = _disabledIconColor;
            icon.color = Color.white;
        }
        else
        {
            material.SetColor("_ColorA", _disabledColor);
            material.SetColor("_ColorB", _disabledColor);
            icon.color = _disabledIconColor;
        }
    }

    public void MusicToggle()
    {
        _musicEnabled = !_musicEnabled;
        PaintButton(_musicMaterial, _musicIcon, _musicEnabled);
        if (_musicEnabled)
        {
            AudioManager.Instance.UnmuteMusic();
        }
        else
        {
            AudioManager.Instance.MuteMusic();
        }
    }

    public void SoundToggle()
    {
        _soundEnabled = !_soundEnabled;
        PaintButton(_soundMaterial, _soundIcon, _soundEnabled);
        if (_soundEnabled)
        {
            AudioManager.Instance.UnmuteSound();
        }
        else
        {
            AudioManager.Instance.MuteSound();
        }
        
    }

    public void VibrationToggle()
    {
        _vibrationEnabled = !_vibrationEnabled;
        PaintButton(_vibrationMaterial, _vibrationIcon, _vibrationEnabled);
    }
}