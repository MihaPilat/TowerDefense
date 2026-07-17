using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class TowerSelectionButton : MonoBehaviour
{
    [SerializeField] private Tower _towerPrefab;

    [SerializeField] private Button _button;
    [SerializeField] private TMP_Text _costText;
    [SerializeField] private Image _icon;

    private BuildService _buildService;

    [Inject]
    public void Construct(
        BuildService buildService)
    {
        _buildService = buildService;
    }

    private void OnEnable()
    {
        if (_button == null)
            return;
        _button.onClick.AddListener(SelectTower);

        if (_costText != null)
            _costText.text = _towerPrefab.Config.Cost.ToString();

        if (_icon != null)
            _icon.sprite = _towerPrefab.Config.Icon;
    }

    private void OnDisable()
    {
        if (_button == null)
            return;
        _button.onClick.RemoveListener(SelectTower);
    }

    public void SelectTower()
    {
        _buildService.SelectTower(
            _towerPrefab);
    }
}
