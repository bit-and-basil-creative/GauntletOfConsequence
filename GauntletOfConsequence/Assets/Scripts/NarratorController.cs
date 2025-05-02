using UnityEngine;

public class NarratorController : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    public void Disappear()
    {
        if (_animator != null)
        {
            _animator.SetTrigger("isDisappearing");
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
}
