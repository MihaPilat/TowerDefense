using System;
using UnityEngine;
using Zenject;

public class GameplayInstaller : MonoInstaller
{
    [SerializeField] private EnemyPath _enemyPath;
    [SerializeField] private Camera _mainCamera;
    public override void InstallBindings()
    {
        BindEnemyPath();
        BindMainCamera();

    }

    private void BindMainCamera()
    {
        Container.Bind<Camera>()
            .FromInstance(_mainCamera)
            .AsSingle()
            .NonLazy();
    }

    private void BindEnemyPath()
    {
        Container
            .Bind<EnemyPath>()
            .FromInstance(_enemyPath)
            .AsSingle()
            .NonLazy();
    }
}
