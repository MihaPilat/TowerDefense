using UnityEngine;
using Zenject;

[RequireComponent(typeof(Enemy))]
public class EnemyMover : MonoBehaviour
{
    private EnemyPath _path;
    private EnemyConfig _enemyConfig;

    private int _currentWaypoint;

    [Inject]
    private void Construct(EnemyPath enemyPath)
    {
        _path = enemyPath;
    }

    private void Awake()
    {
        _enemyConfig = GetComponent<Enemy>().Config;
    }

    private void Update()
    {
        if (_currentWaypoint >= _path.Waypoints.Count)
            return;

        Vector3 target = _path.Waypoints[_currentWaypoint].transform.position;

        transform.position = Vector3.MoveTowards(transform.position, target, _enemyConfig.Speed * Time.deltaTime);

        if(Vector3.Distance(transform.position,target)<0.1f)
        {
            _currentWaypoint++;
        }
    }
}
