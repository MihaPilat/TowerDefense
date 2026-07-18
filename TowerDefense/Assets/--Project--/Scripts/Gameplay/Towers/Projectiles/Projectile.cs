using UnityEngine;

public abstract class Projectile : MonoBehaviour, IProjectile
{
    [SerializeField] protected ProjectileConfig _config;

    protected IDamageable _target;
    protected PoolFactory _poolFactory;
    protected Projectile _prefab;
    protected Vector3 TargetPosition;

    public virtual void Init(IDamageable target, Projectile prefab, PoolFactory poolFactory)
    {
        _target = target;
        _prefab = prefab;
        _poolFactory = poolFactory;

        if (target != null)
        {
            TargetPosition = target.Transform.position;
        }
    }

    private void Update()
    {
        UpdateDestination();

        Move();

        CheckImpact();
    }

    protected abstract void UpdateDestination();

    private void Move()
    {
        transform.position = Vector3.MoveTowards(
        transform.position,
        TargetPosition,
        _config.Speed * Time.deltaTime);
    }

    private void CheckImpact()
    {
        if ((transform.position - TargetPosition).sqrMagnitude < 0.01f)
        {
            Impact();
        }
    }
    protected abstract void Impact();

    protected void ReturnToPool()
    {
        _poolFactory.Reclaim(this, _prefab);
    }
}
