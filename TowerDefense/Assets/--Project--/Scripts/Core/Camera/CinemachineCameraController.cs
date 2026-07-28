using Unity.Cinemachine;
using UnityEngine;
using Zenject;

public class CinemachineCameraController : MonoBehaviour
{
    [Header("Cinemachine References")]
    [SerializeField] private CinemachineCamera _virtualCamera;

    [Header("Movement Settings")]
    [SerializeField] private float _moveSpeed = 20f;

    [Header("Zoom Settings (Height)")]
    [SerializeField] private float _zoomSpeed = 10f;
    [SerializeField] private float _minHeightY = 10f;
    [SerializeField] private float _maxHeightY = 35f;

    [Header("Bounds")]
    [SerializeField] private Vector2 _minBounds = new Vector2(-30f, -30f);
    [SerializeField] private Vector2 _maxBounds = new Vector2(30f, 30f);

    private IInput _input;

    [Inject]
    public void Construct(IInput input)
    {
        _input = input;
    }

    private void Update()
    {
        if (_input == null) return;

        HandleMovement();
        HandleZoom();
        ClampPosition();
    }

    private void HandleMovement()
    {
        Vector2 dir = _input.Move;
        if (dir == Vector2.zero) return;

        Transform mainCamTransform = Camera.main.transform;

        Vector3 forward = mainCamTransform.forward;
        Vector3 right = mainCamTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = (forward * dir.y + right * dir.x).normalized;

        transform.Translate(moveDirection * (_moveSpeed * Time.deltaTime), Space.World);
    }

    private void HandleZoom()
    {
        float zoomDelta = _input.ZoomDelta;
        if (Mathf.Abs(zoomDelta) < 0.01f) return;

        Vector3 pos = transform.position;

        pos.y = Mathf.Clamp(pos.y - zoomDelta * _zoomSpeed, _minHeightY, _maxHeightY);

        transform.position = pos;
    }

    private void ClampPosition()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, _minBounds.x, _maxBounds.x);
        pos.z = Mathf.Clamp(pos.z, _minBounds.y, _maxBounds.y);
        transform.position = pos;
    }
}