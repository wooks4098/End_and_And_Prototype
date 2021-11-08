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
        UseInput();
    }

    public void SetPlayerType(PlayerType _playertype)
    {
        playerType = _playertype;
    }

    void MoveInput()
    {
        if (!CanMove)
        {
            OnMove?.Invoke(MoveType.Stay);

            return;
        }

        switch (playerType)
        {
            case PlayerType.FirstPlayer:
                if (Input.GetKey(KeyCode.W))
                    OnMove?.Invoke(MoveType.Front);
                else if (Input.GetKey(KeyCode.S))
                    OnMove?.Invoke(MoveType.Back);
                else
                    OnMove?.Invoke(MoveType.Stay);
                break;
            case PlayerType.SecondPlayer:
                if (Input.GetKey(KeyCode.UpArrow))
                    OnMove?.Invoke(MoveType.Front);
                else if (Input.GetKey(KeyCode.DownArrow))
                    OnMove?.Invoke(MoveType.Back);
                else
                    OnMove?.Invoke(MoveType.Stay);
                break;
        }
        
    }

    void RotationInput()
    {
        if (!CanMove)
            return;

        switch (playerType)
        {
            case PlayerType.FirstPlayer:
                if (Input.GetKey(KeyCode.A))
                    OnRotation?.Invoke(MoveType.LeftTurn);
                else if (Input.GetKey(KeyCode.D))
                    OnRotation?.Invoke(MoveType.RightTrun);
                break;
            case PlayerType.SecondPlayer:
                if (Input.GetKey(KeyCode.LeftArrow))
                    OnRotation?.Invoke(MoveType.LeftTurn);
                else if (Input.GetKey(KeyCode.RightArrow))
                    OnRotation?.Invoke(MoveType.RightTrun);
                break;
        }
    }

    void RunInput()
    {
        if(!CanMove)
        {
            OnRun?.Invoke(false);
            return;
        }

        switch(playerType)
        {
            case PlayerType.FirstPlayer:
                OnRun?.Invoke(Input.GetKey(KeyCode.LeftShift));
                break;
            case PlayerType.SecondPlayer:
                if (Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.RightShift))
                    OnRun?.Invoke(true);
                else if (!(Input.GetKey(KeyCode.RightControl)) && !(Input.GetKey(KeyCode.RightShift)))
                    OnRun?.Invoke(false);
                break;
        }
       
    }

    void UseInput()
    {
        switch(playerType)
        {
            case PlayerType.FirstPlayer:
                Debug.Log("d");
                if (Input.GetKey(KeyCode.E))
                {
                    if (CanMove)
                        CanMove = false;
                    else
                        CanMove = true;
                }
                break;

            case PlayerType.SecondPlayer:
                if (Input.GetKey(KeyCode.P))
                {
                    if (CanMove)
                        CanMove = false;
                    else
                        CanMove = true;
                }
                break;
        }
        //if (playerType == PlayerType.FirstPlayer)
        //{
        //    OnUse.Invoke();

        //}
        //else if (playerType == PlayerType.SecondPlayer)
        //{
        //    OnUse.Invoke();
        //}
    }

    public void CanMoveChange(bool Change)
    {
        CanMove = Change;
    }
}
