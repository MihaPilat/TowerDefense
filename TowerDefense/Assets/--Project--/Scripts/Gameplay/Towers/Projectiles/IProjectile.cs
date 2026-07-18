using UnityEngine;

public interface IProjectile
{
    void Init(
        IDamageable target,
        Projectile prefab,
        PoolFactory poolFactory);
}
