using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public float CurrentSpeed { get; private set; }
    public float PlayerY { get; private set; }

    [Header("References")]
    [SerializeField] private GameObject playerObject;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private DistanceTracker distanceTracker;
    [SerializeField] private SceneLoader _sceneLoader;

    public Camera MainCamera;
    public Camera UICamera;
    public event Action OnGameOver;
    public bool IsPaused;

    private bool isGameOver;

    void Awake()
    {
        Instance = this;
        Time.timeScale = 1f;
        IsPaused = false;
    }

    void Update()
    {
        UpdateSpeed();
        if (playerObject)
        {
            PlayerY = playerObject.transform.position.y;
        }
    }

    void UpdateSpeed()
    {
        CurrentSpeed = DifficultyManager.Instance.CurrentSpeed;
    }

    private void DestroyPlayer()
    {
        Vector3 pos = playerObject.transform.position;

        // взрыв
        if (explosionPrefab != null)
            Instantiate(explosionPrefab, pos, Quaternion.identity);

        // камера
        CameraImpulse.Instance.Fire(1.2f);

        // звук
        AudioManager.Instance.Play(AudioManager.Instance.hit, 1f);

        // уничтожаем игрока
        // Destroy(playerObject);
        playerObject.SetActive(false);
        Debug.Log("GAME OVER");
    }

    public void OnPlayerHitObstacle()
    {
        if (isGameOver)
            return;

        if (playerObject.TryGetComponent(out PlayerShield playerShield) && playerShield.IsShieldActive())
        {
            playerShield.BreakShield();
            return;
        }

        isGameOver = true;

        DestroyPlayer();

        Pause();
        OnGameOver?.Invoke();

        ComboSystem.Instance.ResetCombo();
    }

    public void OnPlayerDock(bool isPerfect)
    {
        ComboSystem.Instance.RegisterDock(isPerfect);
        if (isPerfect)
        {
            Debug.Log("PERFECT DOCK!");

            // TimeEffects.Instance.SlowMotion(0.2f, 0.18f);
            // CameraShake.Instance.Shake(1.2f, 0.15f);
            // CameraImpulse.Instance.Fire(1.2f);
            // cameraPunch.Punch();
            // CameraFovController.Instance.Punch(2.5f);
            // PerfectPopup.Instance.Show();
            // AudioManager.Instance.Play(perfectSound, 1f);
        }
        else
        {
            // TimeEffects.Instance.SlowMotion(0.25f, 0.12f);
            // CameraShake.Instance.Shake(1.2f, 0.1f);
            // CameraImpulse.Instance.Fire(1.2f);
            // CameraFovController.Instance.Punch(2.5f);
        }
    }


    public void Restart()
    {
        Time.timeScale = 1f;
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        _sceneLoader.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        CurrentSpeed = 0f;
        AudioManager.Instance.PauseMusic();
        IsPaused = true;
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        CurrentSpeed = DifficultyManager.Instance.CurrentSpeed;
        AudioManager.Instance.ResumeMusic();
        IsPaused = false;
    }

    public void Revive()
    {
        isGameOver = false;
        playerObject.SetActive(true);
        Resume();
    }

    public void Quit()
    {
        // SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1f;
        DestroyPlayer();
        _sceneLoader.LoadScene("MainMenu");
    }
}
