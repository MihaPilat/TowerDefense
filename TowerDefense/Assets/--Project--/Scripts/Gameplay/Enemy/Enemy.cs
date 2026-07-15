using System;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    public event Action<float, float> OnHealthChanged;
    public event Action OnDied;

    [SerializeField] private EnemyConfig _config;

    private EnemyHealth _health;

    public EnemyConfig Config => _config;

    private void Awake()
    {
        _health = new EnemyHealth(_config.MaxHealth);

        _health.OnHealthChanged += HealthChanged;
        _health.OnDied += Died;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(10);
        }
    }
    public void TakeDamage(float damage)
    {
        _health.TakeDamage(damage);
    }

    private void HealthChanged(float current, float max)
    {
        OnHealthChanged?.Invoke(current, max);
    }

    private void Died()
    {
        OnDied?.Invoke();

        Destroy(gameObject,2f);
    }
}
