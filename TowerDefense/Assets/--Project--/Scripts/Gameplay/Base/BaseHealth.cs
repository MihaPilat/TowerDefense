using System;
using UnityEngine;

public class BaseHealth : MonoBehaviour
{
    public event Action<int, int> HealthChanged;
    public event Action Died;

    [SerializeField] private int _maxHealth = 100;

    public int CurrentHealth { get; private set; }

    private void Awake()
    {
        CurrentHealth = _maxHealth;
    }

    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;

        HealthChanged?.Invoke(CurrentHealth, _maxHealth);

        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            Died?.Invoke();
        }
    }
}
