using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour, IInput
{
    public Action<MoveType> OnMove { get; set; }
    public Action<MoveType> OnRotation { get; set; }
    public Action<bool> OnRun { get; set; }
    public Action OnUse { get; set; }

    PlayerType playerType;

    public bool CanMove = true;

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
        if (!CanMove)
        {
            if (playerType == PlayerType.FirstPlayer)
            {
                OnMove?.Invoke(MoveType.Stay);
            }
            else if (playerType == PlayerType.SecondPlayer)
            {
                OnMove?.Invoke(MoveType.Stay);
            }
            return;
        }
        if (playerType == PlayerType.FirstPlayer)
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
        if (!CanMove)
        {
            if (playerType == PlayerType.FirstPlayer)
            {
                return;
            }
            else if (playerType == PlayerType.SecondPlayer)
            {
                return;
            }
            return;
        }
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
        if(!CanMove)
        {
            OnRun?.Invoke(false);
            return;

        }
        if (playerType == PlayerType.FirstPlayer)
        {
            OnRun?.Invoke(Input.GetKey(KeyCode.LeftShift));
        }
        else if (playerType == PlayerType.SecondPlayer)
        {
            if (Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.RightShift))
                OnRun?.Invoke(true);
            else if(!(Input.GetKey(KeyCode.RightControl)) && !(Input.GetKey(KeyCode.RightShift)))
                OnRun?.Invoke(false);
        }
    }

    void UseInput()
    {
        if (playerType == PlayerType.FirstPlayer)
        {
            OnUse.Invoke();

        }
        else if (playerType == PlayerType.SecondPlayer)
        {
            OnUse.Invoke();
        }
    }

    public void CanMoveChange(bool Change)
    {
        CanMove = !CanMove;
    }
}
