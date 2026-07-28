using System;
using UnityEngine;
using Zenject;

public class CursorService : ICursorService, IInitializable
{
    private readonly CursorSettings _settings;

    [Inject]
    public CursorService(CursorSettings settings)
    {
        _settings = settings;
    }

    public void Initialize()
    {
        SetDefaultCursor();
    }

    public void SetDefaultCursor()
    {
        ApplyCursor(_settings.DefaultCursor);
    }

    private void ApplyCursor(CursorSettings.CursorData data)
    {
        if (data == null || data.Texture == null)
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            return;
        }

        Cursor.SetCursor(data.Texture, data.Hotspot, CursorMode.Auto);
    }

}
