using UnityEngine;

public class FireballSkill : MonoBehaviour, ISkillEffect
{
    [SerializeField] private GameObject _fireballPrefab;
    [SerializeField] private float _projectileSpeed = 10f;

    private bool _waitingForClick = false;
    private float _damage;
    private int _level;
    private Transform _player;


    public void Activate(float damage, int level, Transform player)
    {
        _damage = damage;
        _level = level;
        _waitingForClick = true;
        _player = player;

    }

    private void Update()
    {
        if (_waitingForClick && Input.GetMouseButtonDown(0))
        {
            GameObject target = GetTargetUnderMouse();
            if (target != null)
            {
                LaunchFireball(target.transform);
                Debug.Log(" Siuu");
                _waitingForClick = false;
            }
            else
            {
                Debug.Log(" No valid target");
            }
        }
    }

    private GameObject GetTargetUnderMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray, 100f);

        foreach (var hit in hits)
        {
            Collider col = hit.collider;

            if (col == null) continue;

            if (col.CompareTag("Enemy") && col is CapsuleCollider)
            {
                return col.gameObject;
            }
        }

        return null;
    }

    private void LaunchFireball(Transform target)
    {
        if (_fireballPrefab == null)       
             return;

        GameObject fireballGO = Instantiate(_fireballPrefab, _player.position + Vector3.up * 1.2f , Quaternion.identity);
        var projectile = fireballGO.GetComponent<FireballProjectile>();
        if (projectile != null)
        {
            projectile.Initialize(target, _damage, _level, _projectileSpeed);
        }

    }
}
