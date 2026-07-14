using UnityEngine;
using Zenject;

public class EnemyMover : MonoBehaviour
{
    [SerializeField] private float _speed = 3f;

    private EnemyPath _path;

    private int _currentWaypoint;

    [Inject]
    private void Construct(EnemyPath enemyPath)
    {
        _path = enemyPath;
    }

    private void Update()
    {
        if (_currentWaypoint >= _path.Waypoints.Count)
            return;

        Vector3 target = _path.Waypoints[_currentWaypoint].transform.position;

        transform.position = Vector3.MoveTowards(transform.position, target, _speed * Time.deltaTime);

        if(Vector3.Distance(transform.position,target)<0.1f)
        {
            _currentWaypoint++;
        }
    }
}
