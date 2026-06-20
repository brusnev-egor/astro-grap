using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private GameObject _settings;
    [SerializeField] private SceneLoader _sceneLoader;
    void Awake()
    {
        Time.timeScale = 1f;
    }

    public void StartGame()
    {
        _sceneLoader.LoadScene("SampleScene");
    }

    public void Shop()
    {
    }

    public void Settings()
    {
        _settings.SetActive(true);
    }

    public void Settings_Close()
    {
        _settings.SetActive(false);
    }
}