using System.Collections;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.UIElements;

public class MeteorRainSkill : MonoBehaviour, ISkillEffect
{
    [SerializeField] private GameObject _vfxPrefab;
    [SerializeField] private float _duration = 5f;
    [SerializeField] private float _tickInterval = 0.5f;
    [SerializeField] private float _radius = 3f;

    private float _damage;


    private Vector3? _aoeCenter = null;
    public void Activate(float damage, int level, Transform player)
    {
        _damage = damage;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray, 100f);

        foreach (var hit in hits)
        {
            if (hit.collider.CompareTag("Ground"))
            {

                Vector3 position = hit.point;
                _aoeCenter = position;

                if (_vfxPrefab != null)
                {
                    GameObject vfx = Instantiate(_vfxPrefab, position + Vector3.up * 10, Quaternion.identity);
                    Destroy(vfx, _duration + 0.5f);
                }

                StartCoroutine(DoAOE(position));
                return;
            }
        }
    }

    private IEnumerator DoAOE(Vector3 position)
    {
        
        float timer = 0f;
        while (timer < _duration)
        {
            ApplyDamageTick(position);
            yield return new WaitForSeconds(_tickInterval);
            timer += _tickInterval;
        }

        Destroy(gameObject);
    }

    private void ApplyDamageTick(Vector3 center)
    {
        Collider[] hits = Physics.OverlapSphere(center, _radius);
        foreach (var col in hits)
        {
            if (col is CapsuleCollider)
            {
                if (col.CompareTag("Enemy"))
                {
                    var health = col.GetComponent<HealthController>();
                    health?.TakeDamage(_damage, false);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (_aoeCenter.HasValue)
        {
            Gizmos.color = new Color(1f, 0f, 0f, 0.35f);
            Gizmos.DrawSphere(_aoeCenter.Value, 0.1f);
            Gizmos.DrawWireSphere(_aoeCenter.Value, _radius);
        }
    }

}

