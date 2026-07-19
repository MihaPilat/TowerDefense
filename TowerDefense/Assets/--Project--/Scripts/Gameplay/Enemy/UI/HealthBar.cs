using UnityEngine;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(Image))]
public class HealthBar : MonoBehaviour
{
    [SerializeField] private UIDamagePopup _popupPrefab;
    [SerializeField] private Transform _popupContainer;

    private Image _fill;
    private Enemy _enemy;
    private PoolFactory _poolFactory;

    [Inject]
    public void Construct(PoolFactory poolFactory)
    {
        _poolFactory = poolFactory;
    }

    private void Awake()
    {
        _fill = GetComponent<Image>();
        _enemy = GetComponentInParent<Enemy>();

        if (_popupContainer == null) _popupContainer = transform;
    }

    private void OnEnable()
    {
        _enemy.OnHealthChanged += SetValue;
        _enemy.OnDamageTaken += HandleDamageTaken;
    }

    private void OnDisable()
    {
        _enemy.OnHealthChanged -= SetValue;
        _enemy.OnDamageTaken-= HandleDamageTaken;
    }

    private void HandleDamageTaken(int finalDamage, DamageType damageType)
    {
        Color damageColor = damageType switch
        {
            DamageType.Physical => Color.red,
            DamageType.Magical => Color.cyan,
            DamageType.Pure => Color.white,
            _ => new Color(1f, 0.6f, 0f)
        };

        UIDamagePopup popup = _poolFactory.Get(_popupPrefab, _popupContainer);

        popup.Setup(finalDamage, damageColor);

        popup.OnAnimationFinished += ReturnPopupToPool;
    }

    private void ReturnPopupToPool(UIDamagePopup popup)
    {
        popup.OnAnimationFinished -= ReturnPopupToPool;

        _poolFactory.Reclaim(popup, _popupPrefab);
    }

    private void SetValue(int current, int max)
    {
        _fill.fillAmount = (float)current / (float)max;
    }
}
