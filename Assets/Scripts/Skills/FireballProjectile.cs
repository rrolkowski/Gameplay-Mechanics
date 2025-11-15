using UnityEngine;

public class FireballProjectile : MonoBehaviour
{
    private Transform _target;
    private float _damage;
    private int _level;
    private float _speed;

    [SerializeField] GameObject _explosionEffect;
    [SerializeField] private GameObject _burnVFXPrefab;

    public void Initialize(Transform target, float damage, int level, float speed)
    {
        _target = target;
        _damage = damage;
        _level = level;
        _speed = speed;
    }

    private void Update()
    {
        if (_target == null)
        {
            Destroy(gameObject);
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, _target.position, _speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, _target.position) < 0.3f)
        {
            Explode();
        }
    }

    private void Explode()
    {
        if (_explosionEffect != null)
        {
            GameObject vfxInstance = Instantiate(_explosionEffect, transform.position, Quaternion.identity);
            Destroy(vfxInstance, 2f);
        }

        var health = _target.GetComponent<HealthController>();
        if (health != null)
        {
            health.TakeDamage(_damage, false);
            BurnEffect burn = _target.GetComponent<BurnEffect>();
            if (burn == null)
                burn = _target.gameObject.AddComponent<BurnEffect>();
                burn.burnVFXPrefab = _burnVFXPrefab;

            burn.StartBurn(_level);
        }

        Destroy(gameObject);
    }
}
