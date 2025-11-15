using UnityEngine;

public class BossController : MonoBehaviour
{

    [SerializeField] private float _attackCooldown = 3f;
    [SerializeField] private float _aggroRange = 12f;
    [SerializeField] private Transform _player;
    [SerializeField] private float _delayBeforeFirstAttack = 1f;

    private IBossAttack[] _attacks;
    private float _aggroStartTime;
    private float _nextAttackTime;
    private bool _hasAggro;

    private void Start()
    {
        _attacks = GetComponents<IBossAttack>();
        if (_player == null)
            _player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void Update()
    {
        if (_player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, _player.position);

        if (!_hasAggro && distanceToPlayer <= _aggroRange)
        {
            _hasAggro = true;
            _aggroStartTime = Time.time;
        }

        if (!_hasAggro)
            return;

        RotateTowardsPlayer();

        if (Time.time < _aggroStartTime + _delayBeforeFirstAttack)
            return;


        if (Time.time >= _nextAttackTime)
        {
            IBossAttack selected = _attacks[Random.Range(0, _attacks.Length)];
            selected.Execute(_player.position);
            _nextAttackTime = Time.time + _attackCooldown;
        }

        if (_hasAggro && distanceToPlayer > _aggroRange + 5f)
        {
            _hasAggro = false;
        }
    }

    private void RotateTowardsPlayer()
    {
        Vector3 direction = _player.position - transform.position;
        direction.y = 0;
        if (direction != Vector3.zero)
            transform.forward = direction.normalized;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _aggroRange);
    }
}
