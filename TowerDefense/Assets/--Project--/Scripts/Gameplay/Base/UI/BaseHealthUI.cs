using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Zenject;

public class BaseHealthUI : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI _healthText;
    [SerializeField] private Image _heartIcon;

    private BaseHealth _baseHealth;

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
    }

    private void OnDisable()
    {
        if (_baseHealth != null)
        {
            _baseHealth.HealthChanged -= OnHealthChanged;
            _baseHealth.Died -= OnBaseDied;
        }
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

    private void OnBaseDied()
    {
        _healthText.text = "0";
        _healthText.color = Color.red;
    }
}