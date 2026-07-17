using System;
using UnityEngine;

public class CurrencyService
{
    public event Action<int> GoldChanged;

    public int Gold { get; private set; }

    public void AddGold(int amount)
    {
        if (amount <= 0)
            return;

        Gold += amount;
        GoldChanged?.Invoke(Gold);
    }

    public bool TrySpendGold(int amount)
    {
        if (Gold < amount)
            return false;

        Gold -= amount;
        GoldChanged?.Invoke(Gold);

        return true;
    }
}
