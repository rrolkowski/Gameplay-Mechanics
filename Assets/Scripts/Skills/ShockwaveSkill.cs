using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public class ShockwaveSkill : MonoBehaviour, ISkillEffect
{
    [Header("Effect Settings")]
    [SerializeField] private float _expansionTime = 1f;

    [Header("References")]
    [SerializeField] private GameObject _shockwaveVFX;

    private float _currentRadius = 0f;
    private HashSet<GameObject> _hitEnemies = new();
    private Vector3 _origin;

    public void Activate(float damage, int level, Transform player)
    {
        float baseRadius = 3f;
        float finalRadius = baseRadius + level * 1.1f;

        _origin = player.position;

        if (_shockwaveVFX != null)
        {
            GameObject vfx = Instantiate(_shockwaveVFX, _origin, Quaternion.identity);
            Destroy(vfx, _expansionTime + 1f);
        }

        StartCoroutine(DoShockwave(_origin, finalRadius, damage));
    }

    private IEnumerator DoShockwave(Vector3 origin, float maxRadius, float damage)
    {
        float elapsedTime = 0f;
        _currentRadius = 0f;
        _hitEnemies.Clear();

        while (elapsedTime < _expansionTime)
        {
            elapsedTime += Time.deltaTime;
            _currentRadius = Mathf.Lerp(0, maxRadius, elapsedTime / _expansionTime);

            Collider[] hits = Physics.OverlapSphere(origin, _currentRadius);
            foreach (var col in hits)
            {
                if (col is CapsuleCollider)
                {
                    if (col.CompareTag("Enemy") && !_hitEnemies.Contains(col.gameObject))
                    {
                        var health = col.GetComponent<HealthController>();
                        if (health != null)
                        {
                            health.TakeDamage(damage, false);
                            _hitEnemies.Add(col.gameObject);
                        }
                    }
                }
            }

            yield return null;
        }

        Destroy(gameObject, 1f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(_origin, _currentRadius);
    }
}
