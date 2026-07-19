using System;
using UnityEngine;

[Serializable]
public struct ResistanceConfig
{
    [Range(0f, 1f)] public float PhysicalResistance;
    [Range(0f, 1f)] public float MagicalResistance;
}
