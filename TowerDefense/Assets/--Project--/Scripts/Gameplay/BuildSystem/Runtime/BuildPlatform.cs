using DG.Tweening;
using UnityEngine;

public class BuildPlatform : MonoBehaviour
{
    [Header("Visual Elements")]
    [SerializeField] private GameObject _highlightVisual;
    [SerializeField] private GameObject _platformView;
    [SerializeField] private Renderer _idleVisual;

    [Header("Hover Animation Settings")]
    [SerializeField] private float _hoverDistance = 0.2f;
    [SerializeField] private float _duration = 1.2f;

    public bool IsOccupied { get; private set; }

    private Tween _idleHoverTween;
    private Vector3 _startLocalPosition;
    private bool _isHovered;

    private void Start()
    {
        SetupIdleHover();
        UpdateVisuals();
    }

    private void SetupIdleHover()
    {
        if (_idleVisual == null) return;

        _startLocalPosition = _idleVisual.transform.localPosition;

        _idleHoverTween = _idleVisual.transform
            .DOLocalMoveY(_startLocalPosition.y + _hoverDistance, _duration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }

    public void Occupy()
    {
        IsOccupied = true;
        UpdateVisuals();
    }

    public void Vacate()
    {
        IsOccupied = false;
        UpdateVisuals();
    }

    public void SetHoverState(bool isHovered)
    {
        if (IsOccupied) return;

        _isHovered = isHovered;
        UpdateVisuals();
    }

    public void ToggleHighlight(bool active)
    {
        SetHoverState(active);
    }

    private void UpdateVisuals()
    {
        if (IsOccupied)
        {
            if (_highlightVisual != null) _highlightVisual.SetActive(false);
            if (_platformView != null) _platformView.SetActive(false);

            ToggleIdleVisual(false);
            return;
        }

        if (_highlightVisual != null) _highlightVisual.SetActive(_isHovered);
        if (_platformView != null) _platformView.SetActive(_isHovered);

        ToggleIdleVisual(!_isHovered);
    }

    private void ToggleIdleVisual(bool show)
    {
        if (_idleVisual == null) return;

        _idleVisual.gameObject.SetActive(show);

        if (_idleHoverTween != null)
        {
            if (show)
                _idleHoverTween.Play();
            else
                _idleHoverTween.Pause();
        }
    }

    private void OnDestroy()
    {
        _idleHoverTween?.Kill();
    }
}