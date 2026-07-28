using System;
using UnityEngine;

[CreateAssetMenu(fileName = "CursorSettings", menuName = "Configs/CursorSettings")]
public class CursorSettings : ScriptableObject
{
    [SerializeField] private CursorData _defaultCursor;

    public CursorData DefaultCursor => _defaultCursor;

    [Serializable]
    public class CursorData
    {
        public Texture2D Texture;
        public Vector2 Hotspot = Vector2.zero;
    }
}