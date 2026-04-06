using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PauseController : MonoBehaviour
{
    [SerializeField] GameObject _pauseMenuUI;
    [SerializeField] Volume _blurVolume;
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
        _musicMaterial = Instantiate(_musicBtn.material);
        _musicBtn.material = _musicMaterial;

        _soundMaterial = Instantiate(_soundBtn.material);
        _soundBtn.material = _soundMaterial;

        _vibrationMaterial = Instantiate(_vibrationBtn.material);
        _vibrationBtn.material = _vibrationMaterial;

        // TODO: retrieve from storage
        _musicEnabled = true;
        _soundEnabled = true;
        _vibrationEnabled = true;
    }

    public void Pause()
    {
        _blurVolume.weight = 1;
        _pauseMenuUI.SetActive(true);
        GameManager.Instance.Pause();
    }

    public void Resume()
    {
        _blurVolume.weight = 0f;
        _pauseMenuUI.SetActive(false);
        GameManager.Instance.Resume();
    }

    public void MusicToggle()
    {
        if (_musicEnabled)
        {
            _musicMaterial.SetColor("_ColorA", _disabledColor);
            _musicMaterial.SetColor("_ColorB", _disabledColor);
            _musicIcon.color = _disabledIconColor;
        }
        else
        {
            _musicMaterial.SetColor("_ColorA", _enabledColor);
            _musicMaterial.SetColor("_ColorB", _enabledColor);
            _musicIcon.color = Color.white;
        }
        _musicEnabled = !_musicEnabled;
    }

    public void SoundToggle()
    {
        if (_soundEnabled)
        {
            _soundMaterial.SetColor("_ColorA", _disabledColor);
            _soundMaterial.SetColor("_ColorB", _disabledColor);
            _soundIcon.color = _disabledIconColor;
            AudioManager.Instance.MuteSound();
        }
        else
        {
            _soundMaterial.SetColor("_ColorA", _enabledColor);
            _soundMaterial.SetColor("_ColorB", _enabledColor);
            _soundIcon.color = Color.white;
            AudioManager.Instance.UnmuteSound();
        }
        _soundEnabled = !_soundEnabled;
    }

    public void VibrationToggle()
    {
        if (_vibrationEnabled)
        {
            _vibrationMaterial.SetColor("_ColorA", _disabledColor);
            _vibrationMaterial.SetColor("_ColorB", _disabledColor);
            _vibrationIcon.color = _disabledIconColor;
        }
        else
        {
            _vibrationMaterial.SetColor("_ColorA", _enabledColor);
            _vibrationMaterial.SetColor("_ColorB", _enabledColor);
            _vibrationIcon.color = Color.white;
        }
        _vibrationEnabled = !_vibrationEnabled;
    }
}