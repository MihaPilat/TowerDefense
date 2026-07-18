using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameplayInstaller : MonoInstaller
{
    [SerializeField] private EnemyPath _enemyPath;
    [SerializeField] private BaseHealth _baseHealth;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private LayerMask _platformLayerMask;
    [SerializeField] private List<Tower> _availableTowersPrefabsList;

    public override void InstallBindings()
    {
        BindTowerFactory();
        BindPoolFactory();
        BindProjectileFactory();
        BindEnemyPath();
        BindMainCamera();
        BindBaseHealth();
        BindBuildMenuUI();
        BindBuildService();
        BindBuildController();
    }

    private void BindBuildMenuUI()
    {
        Container.Bind<BuildMenuUI>()
            .FromComponentInHierarchy()
            .AsSingle()
            .NonLazy();
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
            .WithArguments(_platformLayerMask)
            .NonLazy();
    }

    private void BindBuildService()
    {
        Container.Bind<BuildService>()
            .AsSingle()
            .WithArguments(_availableTowersPrefabsList)
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
