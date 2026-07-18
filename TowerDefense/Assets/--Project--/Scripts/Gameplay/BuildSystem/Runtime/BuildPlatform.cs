using UnityEngine;

public class BuildPlatform : MonoBehaviour
{
    [SerializeField] private GameObject _highlightVisual;
    [SerializeField] private GameObject _platformView;

    public bool IsOccupied { get; private set; }

    private void Start()
    {
        SetHoverState(false);
    }
    public void Occupy()
    {
        IsOccupied = true;
        SetHoverState(false);
    }

    public void Vacate()
    {
        IsOccupied = false;
    }

    public void SetHoverState(bool isHovered)
    {
        if (IsOccupied) return;

        if (_highlightVisual != null)
        {
            _highlightVisual.SetActive(isHovered);
        }

        if (_platformView != null)
        {
            _platformView.SetActive(isHovered);
        }
    }

    public void ToggleHighlight(bool active)
    {
        SetHoverState(active);
    }
}
