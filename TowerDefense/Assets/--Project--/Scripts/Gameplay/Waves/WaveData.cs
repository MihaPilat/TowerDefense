using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WaveData
{
    public List<WaveEnemyData> Enemies;

    [Min(0.1f)]
    public float SpawnDelay = 0.5f;

    [Min(0)]
    public float DelayBeforeNextWave = 10f;
}