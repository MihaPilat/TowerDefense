using System;
using UnityEngine;
using Zenject;

public class Tower : MonoBehaviour
{
    public event Action OnLvlUp;

    [SerializeField] private TowerConfig _config;
    [SerializeField] private LayerMask _enemyLayerMask;
    [SerializeField] private Transform _firePoint;

    private Enemy _currentTargetEnemy;
    private IDamageable _currentTargetDamageable;
    private float _attackCooldownTimer;

    private ProjectileFactory _projectileFactory;

    private readonly Collider[] _targetsBuffer = new Collider[30];

    private int _level;

    public int FinalDamage => CurrentLevel.Damage;
    public TowerConfig Config => _config;

    public int Level => _level;

    public TowerLevelData CurrentLevel => _config.Levels[_level];

    public bool CanUpgrade => _level < _config.Levels.Count - 1;

    public int NextUpgradeCost => CanUpgrade ? _config.Levels[_level + 1].UpgradeCost : 0;

    [Inject]
    private void Construct(ProjectileFactory projectileFactory)
    {
        _projectileFactory = projectileFactory;
    }

    private void Update()
    {
        UpdateCooldown();

        if (!IsTargetValid(_currentTargetEnemy))
        {
            ResetTarget();
        }

        if (_currentTargetEnemy == null)
        {
            FindTarget();
        }

        if (_currentTargetEnemy != null && _attackCooldownTimer <= 0f)
        {
            Attack(_currentTargetDamageable);
        }
    }

    public void LevelUp()
    {
        if (!CanUpgrade)
            return;

        _level++;

        OnLvlUp?.Invoke();
    }

    private void ResetTarget()
    {
        _currentTargetEnemy = null;
        _currentTargetDamageable = null;
    }

    private void UpdateCooldown()
    {
        if (_attackCooldownTimer > 0f)
        {
            _attackCooldownTimer -= Time.deltaTime;
        }
    }

    private bool IsTargetValid(Enemy enemy)
    {
        if(enemy == null || !enemy.gameObject.activeInHierarchy)
            return false;

        if (enemy.IsDie)
            return false;

        float sqrDistance = (transform.position - enemy.transform.position).sqrMagnitude;
        float sqrRange = CurrentLevel.AttackRange * CurrentLevel.AttackRange;

        return sqrDistance <= sqrRange;
    }

    private void FindTarget()
    {
        int count = Physics.OverlapSphereNonAlloc(
            transform.position,
            CurrentLevel.AttackRange,
            _targetsBuffer,
            _enemyLayerMask
        );

        for (int i = 0; i < count; i++)
        {
            var enemyCollider = _targetsBuffer[i];

            if (enemyCollider == null || !enemyCollider.gameObject.activeInHierarchy)
                continue;

            if (enemyCollider.TryGetComponent<Enemy>(out var enemy))
            {
                if (IsTargetValid(enemy))
                {
                    _currentTargetEnemy = enemy;
                    _currentTargetDamageable = enemy;
                    break;
                }
            }
        }
    }

    private void Attack(IDamageable target)
    {
        DamageInfo damageInfo = new DamageInfo(FinalDamage, _config.DamageType);

        _projectileFactory.Spawn(
        _config.ProjectilePrefab,
        _firePoint.position,
        _currentTargetEnemy, damageInfo);

        _attackCooldownTimer = CurrentLevel.AttackCooldown;
    }

    private void OnDrawGizmosSelected()
    {
        if (_config == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, CurrentLevel.AttackRange);
    }
}
