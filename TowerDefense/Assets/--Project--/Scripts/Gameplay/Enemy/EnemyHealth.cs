using System;

public class EnemyHealth
{
    public event Action<int, int> OnHealthChanged;
    public event Action OnDied;

    public int MaxHealth { get;}
    public int CurrentHealth { get; private set; }

    public EnemyHealth(int maxHealth)
    {
        MaxHealth = maxHealth;
        CurrentHealth = maxHealth;
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

        CurrentHealth -= damageInfo.Damage;

        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);

        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        CurrentHealth = 0;

        OnDied?.Invoke();
    }
}
