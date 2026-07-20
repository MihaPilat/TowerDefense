using UnityEngine;
using Zenject;

public class TowerUpgradeService
{
    private readonly CurrencyService _currencyService;

    public TowerUpgradeService(CurrencyService currencyService)
    {
        _currencyService = currencyService;
    }

    public bool TryUpgrade(Tower tower)
    {
        Debug.Log("TryUpgrade");
        if (!tower.CanUpgrade)
            return false;

        int cost = tower.NextUpgradeCost;

        if (!_currencyService.TrySpendGold(cost))
            return false;

        Debug.Log("Upgrade");
        tower.LevelUp();

        return true;
    }
}
