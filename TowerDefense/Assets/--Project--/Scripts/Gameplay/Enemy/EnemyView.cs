using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyView : MonoBehaviour
{
    private Animator _animator;

    private static readonly int IsDeadHash = Animator.StringToHash("IsDead");
    private static readonly int HitTriggerHash = Animator.StringToHash("Hit");

    private Enemy _enemy;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _enemy = GetComponentInParent<Enemy>();
    }

    private void OnEnable()
    {
        _enemy.OnDamageTaken += HandleDamageTaken;
        _enemy.OnDied += HandleDied;
    }

    private void OnDisable()
    {
        _enemy.OnDamageTaken -= HandleDamageTaken;
        _enemy.OnDied -= HandleDied;
    }

    public void Init()
    {
        if (_animator != null)
        {
            _animator.SetBool(IsDeadHash, false);
            _animator.Rebind();
            _animator.Update(0f);
        }
    }

    private void HandleDamageTaken(int damage, DamageType damageType)
    {
        if (_animator != null)
        {
            _animator.SetTrigger(HitTriggerHash);
        }
    }

    private void HandleDied()
    {
        if (_animator != null)
        {
            _animator.SetBool(IsDeadHash, true);
        }
    }
}
