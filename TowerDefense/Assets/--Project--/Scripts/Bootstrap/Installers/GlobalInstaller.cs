using System;
using UnityEngine;
using Zenject;
public class GlobalInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        BindInputReader();
    }

    private void BindInputReader()
    {
        Container.BindInterfacesAndSelfTo<InputReader>()
            .AsSingle()
            .NonLazy();
    }
}
