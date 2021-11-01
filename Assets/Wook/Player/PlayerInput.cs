using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour, IInput
{
    public Action<MoveType> OnMove { get; set; }
    public Action<MoveType> OnRotation { get; set; }
    public Action<bool> OnRun { get; set; }

    private void Update()
    {
        
    }

    void MoveInput()
    {
        if(Input.GetKey(KeyCode.W))
            OnMove?.Invoke(MoveType.Front);
        else if(Input.GetKey(KeyCode.D))
            OnMove?.Invoke(MoveType.Back);

    }

    void RotationInput()
    {
        if (Input.GetKey(KeyCode.A))
            OnRotation?.Invoke(MoveType.LeftTurn);
        else if (Input.GetKey(KeyCode.D))
            OnRotation?.Invoke(MoveType.RightTrun);
    }

    void RunInput()
    {
        if (Input.GetKey(KeyCode.LeftShift))
            OnRun?.Invoke(true);
        else
            OnRun?.Invoke(false);

    }
}
