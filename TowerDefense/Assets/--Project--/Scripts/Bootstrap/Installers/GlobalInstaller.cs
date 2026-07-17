using System;
using UnityEngine;
using Zenject;
public class GlobalInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        BindCurrencyService();
    }

    private void BindCurrencyService()
    {
        Container.Bind<CurrencyService>()
            .AsSingle()
            .NonLazy();
    }
}
