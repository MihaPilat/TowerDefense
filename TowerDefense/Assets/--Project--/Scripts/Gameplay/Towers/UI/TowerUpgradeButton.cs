using UnityEngine;
using UnityEngine.UI;
using Zenject;
using TMPro;
using UnityEngine.Events;
using System.Collections;

public class TowerUpgradeButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private TMP_Text _costText;

    private Color _defaultColor;
    private Tower _selectedTower;
    private TowerUpgradeService _upgradeService;
    private Coroutine _activationCoroutine;

    [Inject]
    private void Construct(TowerUpgradeService upgradeService)
    {
        _upgradeService = upgradeService;

        _defaultColor = _costText.color;
    }
    public void DisableButton()
    {
        if (_activationCoroutine != null)
        {
            StopCoroutine(_activationCoroutine);
            _activationCoroutine = null;
        }

        _button.onClick.RemoveAllListeners();
        _button.interactable = false; 
        _selectedTower = null;
    }

    public void Settup(Tower tower, UnityAction onSelected)
    {
        DisableButton();

        _selectedTower = tower;

        if (tower.CanUpgrade)
        {
            _costText.color = _defaultColor;
            _costText.text = tower.NextUpgradeCost.ToString();

            _activationCoroutine = StartCoroutine(EnableButtonWhenMouseReleased(onSelected));
        }
        else
        {
            _costText.color = Color.red;
            _costText.text = "cannot be upgraded";
        }
    }

    private IEnumerator EnableButtonWhenMouseReleased(UnityAction onSelected)
    {
        while (Input.GetMouseButton(0))
        {
            yield return null;
        }

        yield return null;

        _button.onClick.AddListener(() =>
        {
            UpgradeTower();
            onSelected?.Invoke();
        });

        _button.interactable = true;
        _activationCoroutine = null;
    }

    private void UpgradeTower()
    {
        if (_selectedTower == null)
            return;

        _upgradeService.TryUpgrade(_selectedTower);
        DisableButton();
    }
}