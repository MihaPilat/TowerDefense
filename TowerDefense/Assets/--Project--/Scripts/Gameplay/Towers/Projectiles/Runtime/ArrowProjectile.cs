using UnityEngine;

public class ArrowProjectile : Projectile
{

    protected override void UpdateDestination()
    {
        if (_target != null && _target.Transform != null && _target.Transform.gameObject.activeInHierarchy)
        {
            TargetPosition = _target.Transform.position;
        }
        else
        {
            _target = null;
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
