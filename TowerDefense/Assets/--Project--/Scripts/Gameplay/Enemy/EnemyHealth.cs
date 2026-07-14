using System;

public class EnemyHealth
{
    public event Action<float, float> OnHealthChanged;
    public event Action OnDied;

    public float MaxHealth { get; }
    public float CurrentHealth { get; private set; }

    public EnemyHealth(float maxHealth)
    {
        MaxHealth = maxHealth;
        CurrentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (CurrentHealth <= 0)
            return;

        CurrentHealth -= damage;

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
