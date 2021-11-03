using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour, IInput
{
    public Action<MoveType> OnMove { get; set; }
    public Action<MoveType> OnRotation { get; set; }
    public Action<bool> OnRun { get; set; }

    PlayerType playerType;

    private void Update()
    {
        RunInput();
        MoveInput();
        RotationInput();

    }

    public void SetPlayerType(PlayerType _playertype)
    {
        playerType = _playertype;
    }

    void MoveInput()
    {
        if(playerType == PlayerType.FirstPlayer)
        {
            if (Input.GetKey(KeyCode.W))
                OnMove?.Invoke(MoveType.Front);
            else if (Input.GetKey(KeyCode.S))
                OnMove?.Invoke(MoveType.Back);
            else
                OnMove?.Invoke(MoveType.Stay);
        }
        else if (playerType == PlayerType.SecondPlayer)
        {
            if (Input.GetKey(KeyCode.UpArrow))
                OnMove?.Invoke(MoveType.Front);
            else if (Input.GetKey(KeyCode.DownArrow))
                OnMove?.Invoke(MoveType.Back);
            else
                OnMove?.Invoke(MoveType.Stay);
        }
    }

    void RotationInput()
    {
        if (playerType == PlayerType.FirstPlayer)
        {
            if (Input.GetKey(KeyCode.A))
                OnRotation?.Invoke(MoveType.LeftTurn);
            else if (Input.GetKey(KeyCode.D))
                OnRotation?.Invoke(MoveType.RightTrun);
        }
        else if (playerType == PlayerType.SecondPlayer)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
                OnRotation?.Invoke(MoveType.LeftTurn);
            else if (Input.GetKey(KeyCode.RightArrow))
                OnRotation?.Invoke(MoveType.RightTrun);
        }

    }

    void RunInput()
    {
        if (playerType == PlayerType.FirstPlayer)
        {
            if (Input.GetKey(KeyCode.LeftShift))
                OnRun?.Invoke(true);
            else
                OnRun?.Invoke(false);
        }
        else if (playerType == PlayerType.SecondPlayer)
        {
            if (Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.RightShift))
                OnRun?.Invoke(true);
            else if(!(Input.GetKey(KeyCode.RightControl)) && !(Input.GetKey(KeyCode.RightShift)))
                OnRun?.Invoke(false);
        }
    }
}
