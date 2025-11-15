using System.Collections;
using UnityEngine;

public class DummyController : MonoBehaviour
{
    [SerializeField] Animator _animator;
    [SerializeField] HealthController _healthController;
    [SerializeField] CapsuleCollider _capsuleCollider;


    private void OnTriggerEnter(Collider other)
    {
        _animator.SetTrigger("Hit");
        if (_healthController.GetCurrentHP() <= 0)
        {
            Die();
            Debug.Log("Dummy died..");
        }
    }
    private void Die()
    {
        _animator.SetTrigger("Died");
        StartCoroutine(HandleDeathAnimation());
    }

    private IEnumerator HandleDeathAnimation()
    {
        _capsuleCollider.enabled = false;

        yield return new WaitForSeconds(2f);

        _healthController.ResetHealth();

        _capsuleCollider.enabled = true;
    }

}
