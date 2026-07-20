using UnityEngine;

public class AoEProjectile : Projectile
{
    [SerializeField] private LayerMask _enemyLayerMask;

    protected override void Impact()
    {

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _config.ExplosionRadius, _enemyLayerMask);

        foreach (Collider collider in hitColliders)
        {
            if (collider.TryGetComponent(out IDamageable damageableTarget))
            {
                damageableTarget.TakeDamage(_damageInfo);
            }
        }

        ReturnToPool();
    }

    protected override void UpdateDestination()
    {
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _config.ExplosionRadius);
    }
}
