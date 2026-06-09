using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
        // SceneManager.LoadScene("SampleScene");
        _sceneLoader.LoadScene("SampleScene");
    }

    public void Shop()
    {
        Debug.Log("Shop");
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