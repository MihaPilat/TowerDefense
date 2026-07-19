using UnityEngine;

public interface IDamageable
{
    Transform Transform { get; }
    void TakeDamage(DamageInfo damageInfo);
}
