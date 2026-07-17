using UnityEngine;

public class BuildPlatform : MonoBehaviour
{
    [SerializeField] private GameObject _highlightVisual;

    public bool IsOccupied { get; private set; }

    public void Occupy()
    {
        IsOccupied = true;
    }

    public void Vacate()
    {
        IsOccupied = false;
    }

    public void ToggleHighlight(bool active)
    {
        if (_highlightVisual != null)
        {
            _highlightVisual.SetActive(active);
        }
    }
}
