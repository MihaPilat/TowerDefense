using UnityEngine;

public class ArrowProjectile : Projectile
{

    protected override void UpdateDestination()
    {
        if (_target != null)
        {
            TargetPosition = _target.Transform.position;
        }
    }
    protected override void Impact()
    {
        if (_target != null)
        {
            _target.TakeDamage(_config.Damage);
        }

        ReturnToPool();
    }
}
