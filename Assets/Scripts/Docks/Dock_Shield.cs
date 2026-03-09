using System.Collections;
using UnityEngine;

public class DockShield : Dock
{
    [SerializeField] private Transform shieldCore;

    public override void OnDocked(GrappleController player, bool perfect)
    {
        if (!player.GetComponent<PlayerShield>().IsShieldActive())
        {
            shieldCore.gameObject.SetActive(false);

            player.GetComponent<PlayerShield>().ActivateShield();
        }

    }
}