using UnityEngine;
using Zenject;

[RequireComponent(typeof(Enemy))]
public class EnemyMover : MonoBehaviour
{
    private EnemyPath _path;
    private BaseHealth _baseHealth;
    private EnemyConfig _enemyConfig;
    private Enemy _enemy;
    private int _currentWaypoint;

    private bool _IsReachBase;

    [Inject]
    private void Construct(EnemyPath enemyPath, BaseHealth baseHealth)
    {
        _path = enemyPath;
        _baseHealth = baseHealth;
    }
    public void Init()
    {
        _currentWaypoint = 0;
        _IsReachBase = false;
    }

    private void Awake()
    {
        _enemy = GetComponent<Enemy>();
        _enemyConfig = _enemy.Config;
    }

    private void Update()
    {
        if (_enemy.IsDie)
            return;

        if (_currentWaypoint >= _path.Waypoints.Count)
        {
            ReachBase();
            return;
        }


        Vector3 target = _path.Waypoints[_currentWaypoint].transform.position;

        transform.position = Vector3.MoveTowards(transform.position, target, _enemyConfig.Speed * Time.deltaTime);

        if(Vector3.Distance(transform.position,target)<0.1f)
        {
            _currentWaypoint++;
        }
    }

    private void ReachBase()
    {
        if (_IsReachBase)
            return;
        _baseHealth.TakeDamage(_enemyConfig.Damage);

        _enemy.ReclaimInPool();
        _IsReachBase = true;
    }
}
