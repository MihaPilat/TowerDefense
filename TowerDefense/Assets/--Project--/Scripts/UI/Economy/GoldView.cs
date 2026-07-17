using TMPro;
using UnityEngine;
using Zenject;
using DG.Tweening;

public class GoldView : MonoBehaviour
{
    [SerializeField] private TMP_Text _goldText;

    [SerializeField] private float _duration = 0.5f;
    [SerializeField] private float _punchScale = 1.15f;
    [SerializeField] private int _punchVibrato = 5;

    private CurrencyService _currencyService;

    private int _currentDisplayedGold;
    private Tween _textTween;
    private Tween _scaleTween;
    private Vector3 _initialScale;

    [Inject]
    public void Construct(
        CurrencyService currencyService)
    {
        _currencyService = currencyService;
    }

    private void Awake()
    {
        _initialScale = _goldText.transform.localScale;
    }

    private void OnEnable()
    {
        _currencyService.GoldChanged += OnGoldChanged;

        _currentDisplayedGold = _currencyService.Gold;
        _goldText.text = _currentDisplayedGold.ToString();
        _goldText.transform.localScale = _initialScale;
    }

    private void OnDisable()
    {
        _currencyService.GoldChanged -= OnGoldChanged;

        KillTweens();
    }

    private void OnGoldChanged(int targetGold)
    {
        KillTweens();

        _textTween = DOTween.To(() => _currentDisplayedGold, x =>
        {
            _currentDisplayedGold = x;
            _goldText.text = _currentDisplayedGold.ToString();
        }, targetGold, _duration).SetEase(Ease.OutQuad);

        _scaleTween = _goldText.transform
            .DOPunchScale(_initialScale * (_punchScale - 1f), _duration, _punchVibrato)
            .OnComplete(() => _goldText.transform.localScale = _initialScale);
    }

    private void KillTweens()
    {
        _textTween?.Kill();
        _scaleTween?.Kill();
    }
}
