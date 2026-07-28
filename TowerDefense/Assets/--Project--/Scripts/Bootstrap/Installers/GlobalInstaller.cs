using System;
using UnityEngine;
using Zenject;
public class GlobalInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        BindInputReader();
        BindPauseManager();
    }

    private void BindInputReader()
    {
        Container.BindInterfacesAndSelfTo<InputReader>()
            .AsSingle()
            .NonLazy();
    }
    private void BindPauseManager()
    {
        Container.BindInterfacesAndSelfTo<PauseManager>()
            .AsSingle()
            .NonLazy();
    }

}
