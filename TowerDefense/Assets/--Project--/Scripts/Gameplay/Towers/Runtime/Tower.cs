using System;
using UnityEngine;
using Zenject;

public class Tower : MonoBehaviour
{
    public event Action OnLvlUp;

    [SerializeField] private TowerConfig _config;
    [SerializeField] private LayerMask _enemyLayerMask;
    [SerializeField] private Transform _firePoint;

    private IDamageable _currentTarget;
    private Transform _currentTargetTransform;
    private float _attackCooldownTimer;
    private int _finalDamage;

    private ProjectileFactory _projectileFactory;

    private readonly Collider[] _targetsBuffer = new Collider[30];

    private int _level;

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

        if (_currentTarget != null)
        {
            if (_currentTargetTransform == null ||
                !_currentTargetTransform.gameObject.activeInHierarchy ||
                !IsTargetValid(_currentTarget))
            {
                ResetTarget();
            }
        }

        if (_attackCooldownTimer > 0f)
        {
            return;
        }

        if (_currentTarget == null)
        {
            FindTarget();
        }

        if (_currentTarget != null && _currentTargetTransform != null && _currentTargetTransform.gameObject.activeInHierarchy)
        {
            Attack(_currentTarget);
        }
    }

    public void LevelUp()
    {
        OnLvlUp?.Invoke();

        if (!CanUpgrade)
            return;

        _level++;
    }

    private void ResetTarget()
    {
        _currentTarget = null;
        _currentTargetTransform = null;
    }

    private void UpdateCooldown()
    {
        if (_attackCooldownTimer > 0f)
        {
            _attackCooldownTimer -= Time.deltaTime;
        }
    }

    private bool IsTargetValid(IDamageable target)
    {
        if (target == null) return false;

        if (_currentTargetTransform == null || !_currentTargetTransform.gameObject.activeInHierarchy)
            return false;

        float sqrDistance = (transform.position - _currentTargetTransform.position).sqrMagnitude;
        float sqrRange = CurrentLevel.AttackRange * CurrentLevel.AttackRange;

        return sqrDistance <= sqrRange;
    }

    private void FindTarget()
    {
        ResetTarget();

        int count = Physics.OverlapSphereNonAlloc(
            transform.position,
            CurrentLevel.AttackRange,
            _targetsBuffer,
            _enemyLayerMask
        );

        Debug.Log($"enemys in radius: {count}");
        for (int i = 0; i < count; i++)
        {
            var enemyCollider = _targetsBuffer[i];

            if (!enemyCollider.gameObject.activeInHierarchy)
                continue;

            if (enemyCollider.TryGetComponent<IDamageable>(out var damageable))
            {
                _currentTarget = damageable;
                _currentTargetTransform = enemyCollider.transform;
                break;
            }
        }
    }

    private void Attack(IDamageable target)
    {
        _finalDamage = CurrentLevel.Damage;
        DamageInfo damageInfo = new DamageInfo(_finalDamage, _config.DamageType);

        _projectileFactory.Spawn(
        _config.ProjectilePrefab,
        _firePoint.position,
        _currentTarget, damageInfo);

        _attackCooldownTimer = CurrentLevel.AttackCooldown;
    }

    private void OnDrawGizmosSelected()
    {
        if (_config == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, CurrentLevel.AttackRange);
    }
}
