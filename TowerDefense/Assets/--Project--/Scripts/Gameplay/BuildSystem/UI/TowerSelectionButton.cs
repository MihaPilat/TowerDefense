using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TowerSelectionButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private TMP_Text _costText;
    [SerializeField] private Image _icon;

    private Tower _towerPrefab;
    private BuildPlatform _targetPlatform;
    private BuildService _buildService;

    public void Init(Tower towerPrefab, BuildPlatform targetPlatform, BuildService buildService, UnityAction onSelected)
    {
        _towerPrefab = towerPrefab;
        _targetPlatform = targetPlatform;
        _buildService = buildService;

        if (_costText != null)
            _costText.text = _towerPrefab.Config.Cost.ToString();

        if (_icon != null)
            _icon.sprite = _towerPrefab.Config.Icon;

        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(() =>
        {
            SelectAndBuild();
            onSelected?.Invoke();
        });
    }

    private void SelectAndBuild()
    {
        _buildService.BuildTowerDirectly(_towerPrefab, _targetPlatform);
    }
}
