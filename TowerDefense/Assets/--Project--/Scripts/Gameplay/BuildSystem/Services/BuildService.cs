using System.Collections.Generic;
using Zenject;

public class BuildService
{
    private readonly CurrencyService _currencyService;
    private readonly TowerFactory _towerFactory;
    private readonly List<Tower> _availableTowers;

    public List<Tower> AvailableTowers => _availableTowers;

    public BuildService(CurrencyService currencyService, TowerFactory towerFactory, List<Tower> levelAvailableTowers)
    {
        _currencyService = currencyService;
        _towerFactory = towerFactory;
        _availableTowers = levelAvailableTowers;
    }

    public bool BuildTowerDirectly(Tower towerPrefab, BuildPlatform buildPlatform)
    {
        if (buildPlatform.IsOccupied) return false;

        int cost = towerPrefab.Config.Cost;

        if (!_currencyService.TrySpendGold(cost)) return false;

        _towerFactory.Create(towerPrefab, buildPlatform.transform.position);
        buildPlatform.Occupy();
        buildPlatform.ToggleHighlight(false);

        return true;
    }
}
