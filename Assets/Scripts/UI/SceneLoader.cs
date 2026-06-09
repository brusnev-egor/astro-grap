using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private float _transitionTime = 1f;
    public void LoadScene(string name)
    {
        StartCoroutine(LoadLevel(name));
    }

    public void LoadScene(int index)
    {
        StartCoroutine(LoadLevel(index));
    }

    private IEnumerator LoadLevel(int index)
    { 
        _animator.SetTrigger("Start");
        yield return new WaitForSeconds(_transitionTime);
        SceneManager.LoadScene(index);
    }

    private IEnumerator LoadLevel(string name)
    {
        _animator.SetTrigger("Start");
        yield return new WaitForSeconds(_transitionTime);
        SceneManager.LoadScene(name);
    }
}
