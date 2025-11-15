using UnityEngine;

public class AttackProjectile : MonoBehaviour, IBossAttack
{
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private float _projectileSpeed = 8f;
    [SerializeField] private int _projectileCount = 8;

    private bool _alternate;

    public void Execute(Vector3 targetPosition)
    {
        float offsetAngle = _alternate ? 22.5f : 0f;
        _alternate = !_alternate;

        for (int i = 0; i < _projectileCount; i++)
        {
            float angle = offsetAngle + i * (360f / _projectileCount);
            Vector3 direction = Quaternion.Euler(0, angle, 0) * Vector3.forward;

            GameObject projectile = Instantiate(_projectilePrefab, transform.position + Vector3.down * 1.5f, Quaternion.identity);
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
                rb.linearVelocity = direction.normalized * _projectileSpeed;
        }
    }
}

