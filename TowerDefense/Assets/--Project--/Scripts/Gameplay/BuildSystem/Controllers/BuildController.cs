using UnityEngine;
using Zenject;

public class BuildController : ITickable
{
    private readonly BuildService _buildService;
    private readonly Camera _camera;

    public BuildController(
        BuildService buildService, Camera camera)
    {
        _buildService = buildService;
        _camera = camera;
    }

    public void Tick()
    {
        if (!Input.GetMouseButtonDown(0))
            return;

        if (!_buildService.HasSelectedTower)
            return;

        HandleClick();
    }

    private void HandleClick()
    {
        Ray ray =
            _camera.ScreenPointToRay(
                Input.mousePosition);

        if (!Physics.Raycast(ray, out RaycastHit hit))
            return;

        if (!hit.collider.TryGetComponent(
                out BuildPlatform platform))
        {
            return;
        }

        _buildService.TryBuildTower(platform);
    }
}

