using DG.Tweening;
using TMPro;
using UnityEngine;
using Zenject;

public class WaveTimerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private CanvasGroup _timerCanvasGroup;

    [SerializeField] private Color _normalColor = Color.white;
    [SerializeField] private Color _warningColor = Color.red;
    [SerializeField] private float _punchStrength = 0.3f;
    [SerializeField] private float _animationDuration = 0.25f;

    private WaveService _waveService;
    private int _lastSecond = -1;
    private bool _isShowing;

    private Tween _punchTween;
    private Tween _fadeTween;

    [Inject]
    private void Construct(WaveService waveService)
    {
        _waveService = waveService;
    }

    private void Awake()
    {
        if (_timerCanvasGroup != null)
        {
            _timerCanvasGroup.alpha = 0f;
            _timerCanvasGroup.transform.localScale = Vector3.zero;
        }
    }

    private void OnEnable()
    {
        _waveService.OnTimerUpdated += UpdateTimerText;
        _waveService.OnWaveStarted += HideTimer;
    }

    private void OnDisable()
    {
        _waveService.OnTimerUpdated -= UpdateTimerText;
        _waveService.OnWaveStarted -= HideTimer;

        _punchTween?.Kill();
        _fadeTween?.Kill();
    }

    private void UpdateTimerText(float timeRemaining)
    {
        int currentSecond = Mathf.CeilToInt(timeRemaining);

        if (!_isShowing)
        {
            ShowTimer();
        }

        if (currentSecond != _lastSecond && currentSecond > 0)
        {
            _lastSecond = currentSecond;
            _timerText.text = $"{currentSecond}";

            _punchTween?.Kill();
            _timerText.transform.localScale = Vector3.one;
            _punchTween = _timerText.transform
                .DOPunchScale(Vector3.one * _punchStrength, _animationDuration, vibrato: 1, elasticity: 0.5f);

            Color targetColor = currentSecond <= 3 ? _warningColor : _normalColor;
            _timerText.DOColor(targetColor, _animationDuration);
        }
    }

    private void ShowTimer()
    {
        _isShowing = true;
        _fadeTween?.Kill();

        _fadeTween = DOTween.Sequence()
            .Join(_timerCanvasGroup.DOFade(1f, _animationDuration))
            .Join(_timerCanvasGroup.transform.DOScale(Vector3.one, _animationDuration).SetEase(Ease.OutBack));
    }

    private void HideTimer()
    {
        _isShowing = false;
        _lastSecond = -1;

        _punchTween?.Kill();
        _fadeTween?.Kill();

        _fadeTween = DOTween.Sequence()
            .Join(_timerCanvasGroup.DOFade(0f, _animationDuration))
            .Join(_timerCanvasGroup.transform.DOScale(Vector3.zero, _animationDuration).SetEase(Ease.InBack));
    }
}
