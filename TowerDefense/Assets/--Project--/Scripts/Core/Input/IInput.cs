using System;
using System;
using UnityEngine;

public interface IInput
{
    Vector2 Move { get; }
    float ZoomDelta { get; }

    event Action OnClickPressed;
}