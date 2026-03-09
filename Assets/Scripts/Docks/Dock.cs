using UnityEngine;

public class Dock : MonoBehaviour
{
    public virtual void OnDocked(GrappleController grapple, bool isPerfect)
    {
        GameManager.Instance.OnPlayerDock(isPerfect);
        if (isPerfect)
        {
            // AudioManager.Instance.Play(perfectSound, 1f);
            AudioManager.Instance.Play(AudioManager.Instance.dockSuccess, 1f);
        }
        else
        {
            AudioManager.Instance.Play(AudioManager.Instance.dockSuccess, 1f);
        }
    }
}
