using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private TowerConfig _towerConfig;
    [SerializeField] private LayerMask _enemyLayerMask;

    private IDamageable _currentTarget;
    private Transform _currentTargetTransform;
    private float _attackCooldownTimer;

    private readonly Collider[] _targetsBuffer = new Collider[30];

    public TowerConfig Config => _towerConfig;
    private void Update()
    {
        UpdateCooldown();

        if (_attackCooldownTimer > 0f)
        {
            return;
        }

        if (!IsTargetValid(_currentTarget))
        {
            FindTarget();
        }

        if (_currentTarget != null)
        {
            Attack(_currentTarget);
        }
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
        if (_currentTargetTransform == null || !_currentTargetTransform.gameObject.activeSelf)
            return false;

        float sqrDistance = (transform.position - _currentTargetTransform.position).sqrMagnitude;
        float sqrRange = _towerConfig.AttackRange * _towerConfig.AttackRange;

        return sqrDistance <= sqrRange;
    }

    private void FindTarget()
    {
        _currentTarget = null;
        _currentTargetTransform = null;

        int count = Physics.OverlapSphereNonAlloc(
            transform.position,
            _towerConfig.AttackRange,
            _targetsBuffer,
            _enemyLayerMask
        );

        Debug.Log($"enemys in radius: {count}");
        for (int i = 0; i < count; i++)
        {
            var enemyCollider = _targetsBuffer[i];
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
        target.TakeDamage(_towerConfig.Damage);
        _attackCooldownTimer = _towerConfig.AttackCooldown;
        Debug.Log($"Attacked enemy for {_towerConfig.Damage} damage!");
    }

    private void OnDrawGizmosSelected()
    {
        if (_towerConfig == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _towerConfig.AttackRange);
    }
}
