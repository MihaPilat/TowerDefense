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
        BindTowerFactory();
        BindPoolFactory();
        BindProjectileFactory();
        BindEnemyPath();
        BindMainCamera();
        BindBaseHealth();
        BindBuildService();
        BindBuildController();
    }

    private void BindProjectileFactory()
    {
        Container.BindInterfacesAndSelfTo<ProjectileFactory>()
            .AsSingle()
            .NonLazy();
    }

    private void BindBuildController()
    {
        Container.BindInterfacesTo<BuildController>()
            .AsSingle()
            .NonLazy();
    }

    private void BindBuildService()
    {
        Container.Bind<BuildService>()
            .AsSingle()
            .NonLazy();
    }

    private void BindTowerFactory()
    {
        Container.Bind<TowerFactory>()
            .AsSingle()
            .NonLazy();
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
