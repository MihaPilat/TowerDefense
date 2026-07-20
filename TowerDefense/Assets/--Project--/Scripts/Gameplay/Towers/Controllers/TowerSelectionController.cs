using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class TowerSelectionController : ITickable
{
    private readonly TowerMenuUI _towerPanel;
    private readonly Camera _camera;

    public TowerSelectionController(TowerMenuUI towerPanel, Camera camera)
    {
        _towerPanel = towerPanel;
        _camera = camera;
    }

    public void Tick()
    {
        if (!Input.GetMouseButtonDown(0))
            return;

        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.TryGetComponent(out Tower tower))
            {
                Vector3 screenPos = _camera.WorldToScreenPoint(tower.transform.position);
                _towerPanel.Show(tower, screenPos);
            }
            else
            {
                _towerPanel.Hide();
            }
        }
        else
        {
            _towerPanel.Hide();
        }
    }
}
