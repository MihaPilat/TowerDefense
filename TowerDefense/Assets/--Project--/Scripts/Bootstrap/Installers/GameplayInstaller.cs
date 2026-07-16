using System;
using UnityEngine;
using Zenject;

public class GameplayInstaller : MonoInstaller
{
    [SerializeField] private EnemyPath _enemyPath;
    [SerializeField] private BaseHealth _baseHealth;
    [SerializeField] private Camera _mainCamera;
    public override void InstallBindings()
    {
        BindPoolFactory();
        BindEnemyPath();
        BindMainCamera();
        BindBaseHealth();
    }

    private void BindPoolFactory()
    {
        Container
            .Bind<PoolFactory>()
            .AsSingle()
            .NonLazy();
    }

    private void BindBaseHealth()
    {
        Container.Bind<BaseHealth>()
            .FromInstance(_baseHealth)
            .AsSingle()
            .NonLazy();
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
