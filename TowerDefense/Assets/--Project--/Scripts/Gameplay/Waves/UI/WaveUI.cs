using TMPro;
using UnityEngine;
using Zenject;

public class WaveUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _waveText;
    [SerializeField] private TMP_Text _remainingEnemiesText;

    private WaveService _waveService;

    [Inject]
    private void Construct(WaveService waveService)
    {
        _waveService = waveService;
    }

    private void OnEnable()
    {
        _waveService.WaveChanged += UpdateWave;
        _waveService.RemainingEnemiesChanged += UpdateEnemies;

        UpdateWave(_waveService.CurrentWave);
        UpdateEnemies(_waveService.RemainingEnemies);
    }

    private void OnDisable()
    {
        _waveService.WaveChanged -= UpdateWave;
        _waveService.RemainingEnemiesChanged -= UpdateEnemies;
    }

    private void UpdateWave(int wave) => _waveText.text = $"{wave}";

    private void UpdateEnemies(int enemies) => _remainingEnemiesText.text = $"{enemies}";
    
}
