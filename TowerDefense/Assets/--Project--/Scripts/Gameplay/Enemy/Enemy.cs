using System;
using System.Collections;
using UnityEngine;
using Zenject;

public class Enemy : MonoBehaviour, IDamageable
{
    public event Action<int, DamageType> OnDamageTaken;
    public event Action<int, int> OnHealthChanged;
    public event Action OnDied;

    [SerializeField] private EnemyConfig _config;
    [SerializeField] private EnemyMover _mover;
    [SerializeField] private EnemyView _view;

    private EnemyHealth _health;
    private PoolFactory _originFactory;
    private Enemy _originPrefab;
    private CurrencyService _currencyService;
    private WaveService _waveService;

    private bool _isDie;

    public Transform Transform => transform;
    public bool IsDie => _isDie;
    public EnemyConfig Config => _config;

    [Inject]
    public void Construct(CurrencyService currencyService, WaveService waveService)
    {
        _currencyService = currencyService;
        _waveService = waveService;
    }

    public void Init(Enemy prefab, PoolFactory factory)
    {
        _originPrefab = prefab;
        _originFactory = factory;

        _isDie = false;

        _health.Init();
        _mover.Init();
        _view.Init();
    }
    private void Awake()
    {
        _health = new EnemyHealth(_config.MaxHealth, _config.ResistanceConfig);
    }
    private void OnEnable()
    {
        _health.OnDamageTaken += HandleDamageTaken;
        _health.OnHealthChanged += HealthChanged;
        _health.OnDied += Died;
    }

    private void OnDisable()
    {
        _health.OnDamageTaken -= HandleDamageTaken;
        _health.OnHealthChanged -= HealthChanged;
        _health.OnDied -= Died;
    }

    public void TakeDamage(DamageInfo damageInfo) => _health.TakeDamage(damageInfo);

    public void ReclaimInPool()
    {
        if (_isDie) return;
        _isDie = true;

        _waveService.EnemyKilled();
        ReturnToPool();
    }

    private void HandleDamageTaken(int finalDamage, DamageType damageType) => OnDamageTaken?.Invoke(finalDamage, damageType);

    private void HealthChanged(int current, int max) => OnHealthChanged?.Invoke(current, max);

    private void Died()
    {
        if (_isDie)
            return;
        _isDie = true;

        _currencyService.AddGold(_config.RewardGold);

        OnDied?.Invoke();

        StartCoroutine(DeathCoroutine());
    }

    private IEnumerator DeathCoroutine()
    {
        yield return new WaitForSeconds(_config.DeathDelay);

        _waveService.EnemyKilled();

        ReturnToPool();
    }

    private void ReturnToPool()
    {
        transform.position = new Vector3(-9999f, -9999f, -9999f);
        _originFactory.Reclaim(this, _originPrefab);
    }
}
