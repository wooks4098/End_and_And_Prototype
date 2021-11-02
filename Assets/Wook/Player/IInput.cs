using System;
using UnityEngine;

public enum MoveType
{
    Front = 0,
    Back,
    Stay,
    LeftTurn,
    RightTrun,
    Run,
}

public interface IInput
{
    Action<MoveType> OnMove { get; set; }
    Action<MoveType> OnRotation { get; set; }
    Action<bool> OnRun { get; set; }
}