using System.Collections;
using UnityEngine;

public class AttackMelee : MonoBehaviour, IBossAttack
{
    [SerializeField] private float _radius = 3f;
    [SerializeField] private int _damage = 30;
    [SerializeField] private float _delay = 0.6f;
    [SerializeField] private GameObject _telegraphPrefab;
    [SerializeField] private float _offsetForward = 2.5f;

    public void Execute(Vector3 targetPosition)
    {
        StartCoroutine(ExecuteMelee());
    }

    private IEnumerator ExecuteMelee()
    {
        Vector3 slamPosition = transform.position + transform.forward * _offsetForward;

        if (_telegraphPrefab != null)
        {
            Vector3 spawnPos = transform.position + transform.forward * _offsetForward;
            spawnPos.y = 0.1f;

            Quaternion rot = Quaternion.Euler(-90f, transform.eulerAngles.y, 0f);

            GameObject telegraph = Instantiate(_telegraphPrefab, spawnPos, rot);
            Destroy(telegraph, _delay);
        }

        yield return new WaitForSeconds(_delay);

        Collider[] hits = Physics.OverlapSphere(slamPosition, _radius);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                var health = hit.GetComponent<HealthController>();
                health?.TakeDamage(_damage, false);
            }
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying) return;

        Vector3 attackPos = transform.position + transform.forward * _offsetForward;
        attackPos.y = 0.1f;

        Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
        Gizmos.DrawWireSphere(attackPos, _radius);
    }
#endif
}
