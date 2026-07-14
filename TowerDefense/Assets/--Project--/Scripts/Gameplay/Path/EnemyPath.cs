using System.Collections.Generic;
using UnityEngine;

public class EnemyPath : MonoBehaviour
{
    [SerializeField] private List<Waypoint> _waypoints;

    public IReadOnlyList<Waypoint> Waypoints => _waypoints;


}
