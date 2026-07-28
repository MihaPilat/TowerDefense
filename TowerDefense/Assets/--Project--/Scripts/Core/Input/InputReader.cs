using System;
using UnityEngine;
using Zenject;

public class InputReader : IInitializable, ITickable, IDisposable, IInput
{
    public event Action OnClickPressed;

    public Vector2 Move { get; private set; }
    public float ZoomDelta { get; private set; }

    private PlayerInputAction _input;
    private readonly PauseManager _pauseManager;

    [Inject]
    public InputReader(PauseManager pauseManager)
    {
        _pauseManager = pauseManager;
    }

    public void Initialize()
    {
        _input = new PlayerInputAction();
        _input.Enable();

        _input.Camera.Click.performed += _ =>
        {
            if (!_pauseManager.IsPaused)
                OnClickPressed?.Invoke();
        };
    }

    public void Tick()
    {
        if (_pauseManager.IsPaused)
        {
            Move = Vector2.zero;
            ZoomDelta = 0f;
            return;
        }

        Move = _input.Camera.Move.ReadValue<Vector2>();

        float scroll = _input.Camera.Zoom.ReadValue<float>();
        ZoomDelta = Mathf.Abs(scroll) > 0.01f ? Mathf.Sign(scroll) : 0f;
    }

    public void Dispose()
    {
        _input?.Disable();
        _input?.Dispose();
    }
}