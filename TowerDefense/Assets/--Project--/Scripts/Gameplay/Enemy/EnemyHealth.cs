using System;
using UnityEngine;

public class EnemyHealth
{
    public event Action<int, int> OnHealthChanged;
    public event Action<int, DamageType> OnDamageTaken;
    public event Action OnDied;

    private readonly ResistanceConfig _resistances;

    public int MaxHealth { get;}
    public int CurrentHealth { get; private set; }

    public EnemyHealth(int maxHealth, ResistanceConfig resistances)
    {
        MaxHealth = maxHealth;
        CurrentHealth = maxHealth;
        _resistances = resistances;
    }
    public void Init()
    {
        CurrentHealth = MaxHealth;
        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
    }

    public void TakeDamage(DamageInfo damageInfo)
    {
        if (CurrentHealth <= 0)
            return;

        int finalDamage = CalculateMitigatedDamage(damageInfo.Damage, damageInfo.Type);

        CurrentHealth -= finalDamage;

        OnDamageTaken?.Invoke(finalDamage, damageInfo.Type);

        Debug.Log($"I am taked {finalDamage} {damageInfo.Type}");

        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);

        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    private int CalculateMitigatedDamage(int baseDamage, DamageType damageType)
    {
        float damageAfterResist = baseDamage;

        switch (damageType)
        {
            case DamageType.Physical:
                damageAfterResist = baseDamage * (1f - _resistances.PhysicalResistance);
                break;

            case DamageType.Magical:
                damageAfterResist = baseDamage * (1f - _resistances.MagicalResistance);
                break;

            case DamageType.Pure:
                damageAfterResist = baseDamage;
                break;
        }

        return Mathf.RoundToInt(damageAfterResist);
    }

    private void Die()
    {
        CurrentHealth = 0;

        OnDied?.Invoke();
    }
}
