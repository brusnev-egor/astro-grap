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
    public Camera MainCamera;

    private bool isGameOver;

    void Awake()
    {
        Instance = this;
        Time.timeScale = 1f;
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

        Vector3 pos = playerObject.transform.position;

        // взрыв
        if (explosionPrefab != null)
            Instantiate(explosionPrefab, pos, Quaternion.identity);

        // камера
        CameraImpulse.Instance.Fire(1.2f);

        // звук
        AudioManager.Instance.Play(AudioManager.Instance.hit, 1f);

        // уничтожаем игрока
        Destroy(playerObject);

        Debug.Log("GAME OVER");

        Pause();

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
            PerfectPopup.Instance.Show();
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        CurrentSpeed = 0f;
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        CurrentSpeed = DifficultyManager.Instance.CurrentSpeed;
    }
}
