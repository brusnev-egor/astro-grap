using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseController : MonoBehaviour
{
    [SerializeField] GameObject _pauseMenuUI;
    [SerializeField] Volume _blurVolume;

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

    public void Quit()
    {
        GameManager.Instance.Quit();
        _pauseMenuUI.SetActive(false);
    }
}