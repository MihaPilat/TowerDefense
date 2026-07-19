using UnityEngine;

public abstract class Projectile : MonoBehaviour, IProjectile
{
    [SerializeField] protected ProjectileConfig _config;

    protected IDamageable _target;
    protected PoolFactory _poolFactory;
    protected Projectile _prefab;
    protected Vector3 _targetPosition;
    protected DamageInfo _damageInfo;

    public virtual void Init(IDamageable target, Projectile prefab, PoolFactory poolFactory, DamageInfo damageInfo)
    {
        _target = target;
        _prefab = prefab;
        _poolFactory = poolFactory;
        _damageInfo = damageInfo;

        if (target != null)
        {
            _targetPosition = target.Transform.position;

            Vector3 direction = _targetPosition - transform.position;

            if (direction.sqrMagnitude > 0.001f)
            {
                transform.rotation = Quaternion.LookRotation(direction);
            }
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
        Vector3 direction = _targetPosition - transform.position;

        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);

            transform.rotation = lookRotation;
        }

        transform.position = Vector3.MoveTowards(
        transform.position,
        _targetPosition,
        _config.Speed * Time.deltaTime);
    }

    private void CheckImpact()
    {
        if ((transform.position - _targetPosition).sqrMagnitude < 0.01f)
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
