using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public delegate void AggroEventHandler(EnemyAI aggroedEnemy);
    public event AggroEventHandler OnAggro;

    private State _currentState;

    private EnemyGroupManager _groupManager;

    private Transform _chaseTarget;
    private Collider _attackCollider;
    private Vector3 _startPosition;
    private Transform _player;

    [Header("References")]
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private HealthController _playerHealth;

    [Header("Range Settings")]
    [SerializeField] float _detectionRange = 10f;
    [SerializeField] float _attackRange = 2f;
    [SerializeField] float _chaseRange = 15f;
    [SerializeField] float _patrolRadius = 5f;

    [Header("Attack Settings")]
    [SerializeField] float _attackCooldown = 1.5f;
    [SerializeField] float _attackDamage = 10f;

    [Header("Behaviour Settings")]
    [SerializeField] float _waitTimeAtPoint = 2f;

    private bool _canAttack = true;
    private float _patrolTimer;
    private bool _waitingAtPoint = false;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _startPosition = transform.position;
        _currentState = State.Patrol;
        ChooseNewPatrolPoint();
    }

    void Update()
    {
        switch (_currentState)
        {
            case State.Patrol:
                Patrol();
                break;
            case State.Chase:
                ChasePlayer();
                break;
            case State.Attack:
                AttackPlayer();
                break;
            case State.Return:
                ReturnToStart();
                break;
        }
    }

    void Patrol()
    {
        _patrolTimer -= Time.deltaTime;
        if (!_waitingAtPoint && _agent.remainingDistance < 0.5f)
        {
            StartCoroutine(WaitBeforeNextPatrol());
        }
        DetectPlayer();
    }

    IEnumerator WaitBeforeNextPatrol()
    {
        _waitingAtPoint = true;
        yield return new WaitForSeconds(_waitTimeAtPoint);
        ChooseNewPatrolPoint();
        _waitingAtPoint = false;
    }

    void ChooseNewPatrolPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * _patrolRadius;
        randomDirection += _startPosition;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, _patrolRadius, NavMesh.AllAreas))
        {
            _agent.SetDestination(hit.position);
        }
        else
        {
            _agent.SetDestination(_startPosition);
        }
        _patrolTimer = Random.Range(3f, 6f);
    }

    void ChasePlayer()
    {
        if (_chaseTarget == null) _chaseTarget = _player;
        _agent.SetDestination(_chaseTarget.position);

        if (Vector3.Distance(transform.position, _chaseTarget.position) < _attackRange)
        {
            _currentState = State.Attack;
        }
        else if (Vector3.Distance(transform.position, _chaseTarget.position) > _chaseRange)
        {
            _currentState = State.Return;
        }
    }

    void AttackPlayer()
    {
        _agent.ResetPath();
        transform.LookAt(_chaseTarget);

        if (Vector3.Distance(transform.position, _chaseTarget.position) > _attackRange)
        {
            _currentState = State.Chase;
            return;
        }
    }

    void ReturnToStart()
    {
        _agent.SetDestination(_startPosition);
        if (Vector3.Distance(transform.position, _startPosition) < 0.5f)
        {
            _currentState = State.Patrol;
            ChooseNewPatrolPoint();
        }
    }

    void DetectPlayer()
    {
        if (Vector3.Distance(transform.position, _player.position) < _detectionRange)
        {
            _currentState = State.Chase;
            _chaseTarget = _player;
            OnAggro?.Invoke(this);
        }
    }

    public void SetChaseTarget(Transform target)
    {
        _chaseTarget = target;
        _currentState = State.Chase;
    }

    public Transform GetChaseTarget()
    {
        return _chaseTarget;
    }

    void ResetAttack()
    {
        _canAttack = true;
    }

    public void SetGroupManager(EnemyGroupManager manager)
    {
        _groupManager = manager;
    }

    void OnDestroy()
    {
        if (_groupManager != null)
        {
            _groupManager.enemies.Remove(this);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _detectionRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _attackRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _chaseRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_startPosition, _patrolRadius);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && _currentState == State.Attack && _canAttack)
        {
            if (_playerHealth != null)
            {
                _playerHealth.TakeDamage(_attackDamage, false);
                _canAttack = false;
                Invoke(nameof(ResetAttack), _attackCooldown);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && _currentState == State.Attack && _canAttack)
        {
            if (_playerHealth != null)
            {
                _playerHealth.TakeDamage(_attackDamage, false);
                _canAttack = false;
                Invoke(nameof(ResetAttack), _attackCooldown);
            }
        }
    }
}

public enum State
{
    Patrol,
    Chase,
    Attack,
    Return,
}
