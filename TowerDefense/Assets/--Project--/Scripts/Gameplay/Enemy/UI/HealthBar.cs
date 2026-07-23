using UnityEngine;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(Image))]
public class HealthBar : MonoBehaviour
{
    [SerializeField] private UIDamagePopup _popupPrefab;
    [SerializeField] private Transform _popupContainer;
    [SerializeField] private Image _background;

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
        _enemy.OnDied += Hide;
    }

    private void OnDisable()
    {
        _enemy.OnHealthChanged -= SetValue;
        _enemy.OnDamageTaken-= HandleDamageTaken;
        _enemy.OnDied -= Hide;
    }

    private void Hide()
    {
        _fill.enabled = false;
        _background.enabled = false;
    }

    private void Show()
    {
        _fill.enabled = true;
        _background.enabled = true;
    }

    private void HandleDamageTaken(int finalDamage, DamageType damageType)
    {
        if (_enemy.IsDie) return;

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
        if (!_enemy.IsDie && !_fill.enabled)
        {
            Show();
        }

        _fill.fillAmount = (float)current / (float)max;
    }
}
