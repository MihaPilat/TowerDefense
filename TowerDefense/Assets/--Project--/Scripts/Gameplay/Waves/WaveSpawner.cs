using System.Collections;
using UnityEngine;
using Zenject;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private WavesConfig _wavesConfig;
    [SerializeField] private Transform[] _spawnPoints;

    private PoolFactory _poolFactory;
    [Inject]
    private void Construct(PoolFactory poolFactory)
    {
        _poolFactory = poolFactory;
    }

    private void Start()
    {
        StartCoroutine(WavesRoutine());
    }

    private IEnumerator WavesRoutine()
    {
        foreach (WaveData wave in _wavesConfig.Waves)
        {
            yield return SpawnWave(wave);

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
