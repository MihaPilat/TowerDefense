using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class BuildController : ITickable
{
    private readonly BuildService _buildService;
    private readonly Camera _camera;
    private readonly BuildMenuUI _buildMenuUI;
    private readonly LayerMask _platformLayerMask;

    private Collider _lastHitCollider;

    private BuildPlatform _lastHoveredPlatform;

    public BuildController(BuildService buildService, Camera camera, BuildMenuUI buildMenuUI, LayerMask layerMask)
    {
        _buildService = buildService;
        _camera = camera;
        _buildMenuUI = buildMenuUI;
        _platformLayerMask = layerMask;
    }

    public void Tick()
    {
        HandleHover();

        if (Input.GetMouseButtonDown(0))
        {
            HandleClick();
        }
    }

    private void HandleHover()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _platformLayerMask))
        {
            if (_lastHitCollider == hit.collider)
            {
                return;
            }

            _lastHitCollider = hit.collider;

            if (hit.collider.TryGetComponent(out BuildPlatform platform) && !platform.IsOccupied)
            {
                if (_lastHoveredPlatform != platform)
                {
                    ResetLastHover();
                    _lastHoveredPlatform = platform;
                    _lastHoveredPlatform.ToggleHighlight(true);
                }
                return;
            }
        }

        if (_lastHitCollider != null)
        {
            _lastHitCollider = null;
            ResetLastHover();
        }

    }
    private void HandleClick()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.TryGetComponent(out BuildPlatform platform) && !platform.IsOccupied)
            {
                Vector3 screenPos = _camera.WorldToScreenPoint(platform.transform.position);
                _buildMenuUI.Show(platform, _buildService.AvailableTowers, screenPos);
                return;
            }
        }

        _buildMenuUI.Hide();
    }

    private void ResetLastHover()
    {
        if (_lastHoveredPlatform != null)
        {
            _lastHoveredPlatform.ToggleHighlight(false);
            _lastHoveredPlatform = null;
        }
    }
}

