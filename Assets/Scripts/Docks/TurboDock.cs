using UnityEngine;

public class TurboDock : Dock
{
    [SerializeField] private bool increaseCombo = true;

    public override void OnDocked(GrappleController player, bool perfect)
    {
        base.OnDocked(player, perfect);

        DifficultyManager.Instance.ActivateTurbo();

        // if (increaseCombo)
            // ComboSystem.Instance.AddCombo(1);

        // CameraShake.Instance.Shake(0.8f, 0.2f);
        // CameraFovController.Instance.Punch(3f);

        // AudioManager.Instance.Play(AudioManager.Instance.turbo, 1f);
    }
}