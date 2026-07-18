using UnityEngine;
using Zenject;

public class ProjectileFactory
{
    private readonly PoolFactory _poolFactory;

    [Inject]
    public ProjectileFactory(PoolFactory poolFactory)
    {
        _poolFactory = poolFactory;
    }

    public IProjectile Spawn(Projectile prefab, Vector3 position, IDamageable target, int damage)
    {
        Projectile projectile = _poolFactory.Get(prefab);

        projectile.transform.position = position;

        projectile.Init(target, prefab, _poolFactory, damage);

        return projectile;
    }
}
