using System;
using UnityEngine;
using Zenject;

public class GameplayInstaller : MonoInstaller
{
    [SerializeField] private EnemyPath _enemyPath;

    public override void InstallBindings()
    {
        BindEnemyPath();
    }

    private void BindEnemyPath()
    {
        Container
            .Bind<EnemyPath>()
            .FromInstance(_enemyPath)
            .AsSingle().NonLazy();
    }
}
