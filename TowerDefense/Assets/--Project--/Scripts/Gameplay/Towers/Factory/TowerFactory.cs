using UnityEngine;
using Zenject;

public class TowerFactory
{
    private readonly IInstantiator _instantiator;

    public TowerFactory(IInstantiator instantiator)
    {
        _instantiator = instantiator;
    }

    public Tower Create(Tower prefab, Vector3 position)
    {
        Tower tower = _instantiator.InstantiatePrefabForComponent<Tower>(prefab);

        tower.transform.position = position;

        return tower;
    }
}
