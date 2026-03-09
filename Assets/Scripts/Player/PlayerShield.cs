using System.Collections;
using UnityEngine;

public class PlayerShield : MonoBehaviour
{
    [SerializeField] GameObject shieldVisual;

    private bool _isShieldActive = false;

    public bool IsShieldActive()
    {
        return _isShieldActive;
    }

    public void ActivateShield()
    {
        shieldVisual.SetActive(true);
        _isShieldActive = true;
    }

    public void BreakShield()
    {
        // shieldVisual.SetActive(false);
        StartCoroutine(BreakShieldVisual());
    }

    IEnumerator BreakShieldVisual()
    {
        float t = 0;

        Renderer renderer = shieldVisual.GetComponent<Renderer>();
        Material shieldMaterial = renderer.material;

        while (t < 1)
        {
            t += Time.deltaTime * 2f;

            shieldMaterial.SetFloat("_BreakAmount", t);
            shieldMaterial.SetFloat("_BreakFlash", 1f - t);

            yield return null;
        }

        _isShieldActive = false;
        shieldVisual.SetActive(false);
    }
}