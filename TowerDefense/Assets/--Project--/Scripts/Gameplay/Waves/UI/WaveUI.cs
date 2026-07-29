using DG.Tweening;
using TMPro;
using UnityEngine;
using Zenject;

public class WaveUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _waveText;
    [SerializeField] private TMP_Text _remainingEnemiesText;

    [Header("Punch Animation Settings")]
    [SerializeField] private float _punchScale = 1.2f;
    [SerializeField] private float _animationDuration = 0.2f;

    private WaveService _waveService;
    private Vector3 _originalScale;
    private Tween _punchTween;

    [Inject]
    private void Construct(WaveService waveService)
    {
        _waveService = waveService;
    }

    private void Awake()
    {
        if (_remainingEnemiesText != null)
        {
            _originalScale = _remainingEnemiesText.transform.localScale;
        }
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

        _punchTween?.Kill();
    }

    private void UpdateWave(int wave) => _waveText.text = $"{wave}";

    private void UpdateEnemies(int enemies)
    {
        _remainingEnemiesText.text = $"{enemies}/{_waveService.TotalEnemies}";

        AnimateEnemyCountPulse();
    }

    private void AnimateEnemyCountPulse()
    {
        if (_remainingEnemiesText == null) return;

        _punchTween?.Kill();
        _remainingEnemiesText.transform.localScale = _originalScale;

        _punchTween = _remainingEnemiesText.transform
            .DOPunchScale(_originalScale * (_punchScale - 1f), _animationDuration, vibrato: 1, elasticity: 0.5f)
            .OnComplete(() => _remainingEnemiesText.transform.localScale = _originalScale);
    }
}
