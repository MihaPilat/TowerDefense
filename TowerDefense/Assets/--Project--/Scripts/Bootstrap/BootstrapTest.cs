using UnityEngine;
using Zenject;

public class BootstrapTest : MonoBehaviour
{
    [SerializeField] private int _startGold=30;
    private CurrencyService _currencyService;
    [Inject]
    private void Construct(CurrencyService currencyService)
    {
        _currencyService = currencyService;
    }
    private void Start()
    {
        _currencyService.AddGold(_startGold);
    }
}
