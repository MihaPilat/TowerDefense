using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    public event Action<int, int> OnHealthChanged;
    public event Action OnDied;

    [SerializeField] private EnemyConfig _config;
    [SerializeField] private EnemyMover _mover;
    private EnemyHealth _health;

    private PoolFactory _originFactory;
    private Enemy _originPrefab;

    private float _deathDelay = 1f;

    private bool _isDie;

    public EnemyConfig Config => _config;


    public void Init(Enemy prefab, PoolFactory factory)
    {
        _originPrefab = prefab;
        _originFactory = factory;

        _isDie = false;

        _health.Init();
        _mover.Init();
    }
    private void Awake()
    {
        _health = new EnemyHealth(_config.MaxHealth);
    }
    private void OnEnable()
    {

        _health.OnHealthChanged += HealthChanged;
        _health.OnDied += Died;
    }

    private void OnDisable()
    {

        _health.OnHealthChanged -= HealthChanged;
        _health.OnDied -= Died;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(10);
        }
    }
    public void TakeDamage(int damage)
    {
        Debug.Log($"take {damage} damage");
        _health.TakeDamage(damage);
    }

    public void ReclaimInPool()
    {
        StartCoroutine(DeathCoroutine());
    }

    private void HealthChanged(int current, int max)
    {
        OnHealthChanged?.Invoke(current, max);
    }

    private void Died()
    {
        if (_isDie)
            return;
        _isDie = true;

        OnDied?.Invoke();

        StartCoroutine(DeathCoroutine());
    }

    private IEnumerator DeathCoroutine()
    {
        yield return new WaitForSeconds(_deathDelay);
        _originFactory.Reclaim(this, _originPrefab);
    }

}
