using System.Collections;
using UnityEngine;
using Zenject;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private WavesConfig _wavesConfig;
    [SerializeField] private Transform[] _spawnPoints;

    private PoolFactory _poolFactory;
    private WaveService _waveService;

    [Inject]
    private void Construct(PoolFactory poolFactory, WaveService waveService)
    {
        _poolFactory = poolFactory;
        _waveService = waveService;
    }

    private void Start()
    {
        StartCoroutine(WavesRoutine());
    }

    private IEnumerator WavesRoutine()
    {
        for (int i = 0; i < _wavesConfig.Waves.Count; i++)
        {
            WaveData wave = _wavesConfig.Waves[i];

            _waveService.StartWave(
                i + 1,
                GetEnemiesCount(wave));

            yield return SpawnWave(wave);

            yield return new WaitUntil(() =>
                _waveService.IsWaveCompleted);

            yield return new WaitForSeconds(
                wave.DelayBeforeNextWave);
        }


        Debug.Log("All waves completed");
    }

    private IEnumerator SpawnWave(WaveData wave)
    {
        foreach (WaveEnemyData enemyData in wave.Enemies)
        {
            for (int i = 0; i < enemyData.Count; i++)
            {
                SpawnEnemy(enemyData.Prefab);

                yield return new WaitForSeconds(
                    wave.SpawnDelay);
            }
        }
    }

    private int GetEnemiesCount(WaveData wave)
    {
        int count = 0;

        foreach (WaveEnemyData enemy in wave.Enemies)
        {
            count += enemy.Count;
        }

        return count;
    }

    private void SpawnEnemy(Enemy enemyPrefab)
    {
        Transform spawnPoint =
            _spawnPoints[Random.Range(0, _spawnPoints.Length)];

        var enemy = _poolFactory.Get(enemyPrefab);

        enemy.transform.position = spawnPoint.position;
        enemy.transform.rotation = Quaternion.identity;

        enemy.Init(enemyPrefab, _poolFactory);
    }

}
