using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public float CurrentSpeed { get; private set; }
    public float PlayerY { get; private set; }

    [Header("References")]
    [SerializeField] private GameObject playerObject;

    [SerializeField] private DistanceTracker distanceTracker;
    // [SerializeField] private CameraOrthoPunch cameraPunch;

    private bool isGameOver;

    void Awake()
    {
        Instance = this;
        Time.timeScale = 1f;
    }

    void Update()
    {
        UpdateSpeed();
        PlayerY = playerObject.transform.position.y;
    }

    void UpdateSpeed()
    {
        CurrentSpeed = DifficultyManager.Instance.CurrentSpeed;
    }

    public void OnPlayerHitObstacle()
    {
        if (isGameOver)
            return;

        CameraImpulse.Instance.Fire(1.2f);
        AudioManager.Instance.Play(AudioManager.Instance.hit, 1f);

        isGameOver = true;

        Debug.Log("GAME OVER");

        // временно — пауза
        Time.timeScale = 0f;
        CurrentSpeed = 0f;

        // UI
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
}
