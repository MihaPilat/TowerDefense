using System;

public class WaveService
{
    public event Action OnWaveStarted;
    public event Action<float> OnTimerUpdated;
    public event Action<int> WaveChanged;
    public event Action<int> RemainingEnemiesChanged;
    public event Action WaveCompleted;


    public int CurrentWave { get; private set; }

    public int RemainingEnemies { get; private set; }

    public bool IsWaveCompleted => RemainingEnemies <= 0;

    public void StartWave(int waveNumber, int enemiesCount)
    {
        CurrentWave = waveNumber;
        RemainingEnemies = enemiesCount;

        OnWaveStarted?.Invoke();
        WaveChanged?.Invoke(CurrentWave);
        RemainingEnemiesChanged?.Invoke(RemainingEnemies);
    }

    public void UpdateTimer(float timeRemaining)
    {
        OnTimerUpdated?.Invoke(timeRemaining);
    }

    public void EnemyKilled()
    {
        if (RemainingEnemies <= 0)
            return;

        RemainingEnemies--;

        RemainingEnemiesChanged?.Invoke(RemainingEnemies);

        if (RemainingEnemies == 0)
        {
            WaveCompleted?.Invoke();
        }
    }
}
