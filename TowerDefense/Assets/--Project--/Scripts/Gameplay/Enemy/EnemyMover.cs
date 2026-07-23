using UnityEngine;
using UnityEngine.AI;
using Zenject;

[RequireComponent(typeof(Enemy))]
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMover : MonoBehaviour
{
    private EnemyPath _path;
    private BaseHealth _baseHealth;
    private EnemyConfig _enemyConfig;
    private Enemy _enemy;

    private NavMeshAgent _agent;
    private int _currentWaypointIndex;
    private bool _isReachedBase;

    [Inject]
    private void Construct(EnemyPath enemyPath, BaseHealth baseHealth)
    {
        _path = enemyPath;
        _baseHealth = baseHealth;
    }
    public void Init()
    {
        _currentWaypointIndex = 0;
        _isReachedBase = false;

        _agent.enabled = false;

        if (_path.Waypoints != null && _path.Waypoints.Count > 0)
        {
            Vector3 targetPos = _path.Waypoints[0].transform.position;
            Vector3 direction = targetPos - transform.position;
            direction.y = 0;

            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(direction);
            }
        }

        _agent.enabled = true;
        _agent.speed = _enemyConfig.Speed;
        _agent.isStopped = false;

        SetNextDestination();
    }

    private void Awake()
    {
        _enemy = GetComponent<Enemy>();
        _enemyConfig = _enemy.Config;

        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (_enemy.IsDie || _isReachedBase)
            return;

        if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
        {
            _currentWaypointIndex++;

            if (_currentWaypointIndex >= _path.Waypoints.Count)
            {
                ReachBase();
            }
            else
            {
                SetNextDestination();
            }
        }
    }

    private void OnEnable()
    {
        _enemy.OnDied += StopMovement;
    }

    private void OnDisable()
    {
        _enemy.OnDied -= StopMovement;

        if (_agent != null && _agent.enabled)
            _agent.enabled = false;
    }

    private void StopMovement()
    {
        if (_agent.isActiveAndEnabled)
        {
            _agent.isStopped = true;
            _agent.velocity = Vector3.zero;
            _agent.enabled = false;
        }
    }

    private void SetNextDestination()
    {
        if (_path.Waypoints == null || _path.Waypoints.Count == 0)
            return;

        Vector3 targetPosition = _path.Waypoints[_currentWaypointIndex].transform.position;
        _agent.SetDestination(targetPosition);
    }

    private void ReachBase()
    {
        if (_isReachedBase)
            return;

        _isReachedBase = true;
        _baseHealth.TakeDamage(_enemyConfig.Damage);

        if (_agent.isActiveAndEnabled)
            _agent.isStopped = true;

        _agent.enabled = false;

        _enemy.ReclaimInPool();
    }
}
