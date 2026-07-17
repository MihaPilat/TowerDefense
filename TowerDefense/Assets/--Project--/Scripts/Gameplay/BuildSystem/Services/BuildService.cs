using UnityEngine;
using Zenject;

public class BuildService
{
    private readonly CurrencyService _currencyService;
    private readonly TowerFactory _towerFactory;

    private Tower _selectedTower;

    public BuildService(CurrencyService currencyService, TowerFactory towerFactory)
    {
        _currencyService = currencyService;
        _towerFactory = towerFactory;
    }

    public bool HasSelectedTower =>
        _selectedTower != null;

    public void SelectTower(Tower towerPrefab)
    {
        _selectedTower = towerPrefab;
    }

    public void ClearSelection()
    {
        _selectedTower = null;
    }

    public bool TryBuildTower(BuildPlatform buildPlatform)
    {
        if (_selectedTower == null)
            return false;

        if (buildPlatform.IsOccupied)
            return false;

        int cost = _selectedTower.Config.Cost;

        if (!_currencyService.TrySpendGold(cost))
            return false;

        _towerFactory.Create(_selectedTower, buildPlatform.transform.position);

        buildPlatform.Occupy();

        return true;
    }
}
