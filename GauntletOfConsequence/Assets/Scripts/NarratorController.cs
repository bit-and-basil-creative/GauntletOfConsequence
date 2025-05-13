using System.Collections;
using UnityEngine;

public class NarratorController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip whoosh;

    public void Disappear()
    {
        if (_animator != null)
        {
            StartCoroutine(DisappearAfterDelay(2.0f));
        }
    }

    public void Reappear()
    {
        if (_animator != null)
        {
            _animator.SetTrigger("isAppearing");
        }
    }

    public void Idle()
    {
        if (_animator != null)
        {
            _animator.SetBool("isIdle", true);
        }
    }

    public void CastSpell()
    {
        if (_animator != null)
        {
            _animator.SetTrigger("isCasting");
        }
    }

    public void ClearIdle()
    {
        _animator.SetBool("isIdle", false);
    }

    private IEnumerator DisappearAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        _animator.SetBool("isDisappearing", true);
        audioSource.PlayOneShot(whoosh);
    }
}
