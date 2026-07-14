using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class HealthBar : MonoBehaviour
{
    private Image _fill;
    private Enemy _enemy;

    private void Awake()
    {
        _fill = GetComponent<Image>();
        _enemy = GetComponentInParent<Enemy>();
    }
    private void OnEnable()
    {
        _enemy.OnHealthChanged += SetValue;
    }
    private void OnDisable()
    {
        _enemy.OnHealthChanged -= SetValue;
    }
    private void SetValue(float current, float max)
    {
        _fill.fillAmount = current / max;
    }
}
