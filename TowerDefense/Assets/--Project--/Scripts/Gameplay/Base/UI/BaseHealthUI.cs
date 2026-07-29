using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Zenject;
using DG.Tweening;

public class BaseHealthUI : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI _healthText;
    [SerializeField] private Image _heartIcon;

    [Header("Continuous Pulse Settings")]
    [SerializeField] private float _pulseScale = 1.1f;
    [SerializeField] private float _pulseDuration = 0.8f;

    private BaseHealth _baseHealth;
    private Tween _continuousPulseTween;

    [Inject]
    private void Construct(BaseHealth baseHealth)
    {
        _baseHealth = baseHealth;
    }

    private void OnEnable()
    {
        if (_baseHealth != null)
        {
            _baseHealth.HealthChanged += OnHealthChanged;
            _baseHealth.Died += OnBaseDied;
        }

        StartContinuousPulse();
    }

    private void OnDisable()
    {
        if (_baseHealth != null)
        {
            _baseHealth.HealthChanged -= OnHealthChanged;
            _baseHealth.Died -= OnBaseDied;
        }

        StopContinuousPulse();
    }

    private void Start()
    {
        if (_baseHealth != null)
        {
            UpdateHealthUI(_baseHealth.CurrentHealth);
        }
    }

    private void OnHealthChanged(int currentHealth, int maxHealth)
    {
        UpdateHealthUI(currentHealth);
    }

    private void UpdateHealthUI(int currentHealth)
    {
        _healthText.text = $"{currentHealth}";
    }

    private void StartContinuousPulse()
    {
        if (_heartIcon == null) return;

        StopContinuousPulse();

        _heartIcon.transform.localScale = Vector3.one;

        _continuousPulseTween = _heartIcon.transform
            .DOScale(_pulseScale, _pulseDuration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }

    private void StopContinuousPulse()
    {
        if (_continuousPulseTween != null)
        {
            _continuousPulseTween.Kill();
            _continuousPulseTween = null;
        }

        if (_heartIcon != null)
        {
            _heartIcon.transform.localScale = Vector3.one;
        }
    }

    private void OnBaseDied()
    {
        _healthText.text = "0";
        _healthText.color = Color.red;
        StopContinuousPulse();
    }
}