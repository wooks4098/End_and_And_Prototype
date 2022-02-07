using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 플레이어 인풋 관련 클래스
/// 지금은 각각의 플레이어가 가지고 있지만 
/// InputManager로 변경후 하나의 InputManager가 인풋을 받아 플레이어나 오브젝트에 인풋을 주는걸로 수정
/// </summary>


public class PlayerInput : MonoBehaviour//, IInput
{
    //public Action<MoveType> OnFrontBackPlayer1 { get; set; }
    //public Action<MoveType> OnLeftRightPlayer1 { get; set; }
    //public Action<bool> OnRunPlayer1 { get; set; }
    //public Action OnUsePlayer1 { get; set; }

    //PlayerType playerType;

    //public bool CanMove = true;

    //private void Update()
    //{
    //    RunInput();
    //    MoveInput();
    //    RotationInput();
    //    //UseInput();
    //}

    //public void SetPlayerType(PlayerType _playertype)
    //{
    //    playerType = _playertype;
    //}

    //void MoveInput()
    //{
    //    if (!CanMove)
    //    {
    //        OnFrontBackPlayer1?.Invoke(MoveType.Stay);

    //        return;
    //    }

    //    switch (playerType)
    //    {
    //        case PlayerType.FirstPlayer:
    //            if (Input.GetKey(KeyCode.W))
    //                OnFrontBackPlayer1?.Invoke(MoveType.Front);
    //            else if (Input.GetKey(KeyCode.S))
    //                OnFrontBackPlayer1?.Invoke(MoveType.Back);
    //            else
    //                OnFrontBackPlayer1?.Invoke(MoveType.Stay);
    //            break;
    //        case PlayerType.SecondPlayer:
    //            if (Input.GetKey(KeyCode.UpArrow))
    //                OnFrontBackPlayer1?.Invoke(MoveType.Front);
    //            else if (Input.GetKey(KeyCode.DownArrow))
    //                OnFrontBackPlayer1?.Invoke(MoveType.Back);
    //            else
    //                OnFrontBackPlayer1?.Invoke(MoveType.Stay);
    //            break;
    //    }
        
    //}

    //void RotationInput()
    //{
    //    if (!CanMove)
    //        return;

    //    switch (playerType)
    //    {
    //        case PlayerType.FirstPlayer:
    //            if (Input.GetKey(KeyCode.A))
    //                OnLeftRightPlayer1?.Invoke(MoveType.LeftTurn);
    //            else if (Input.GetKey(KeyCode.D))
    //                OnLeftRightPlayer1?.Invoke(MoveType.RightTrun);
    //            break;
    //        case PlayerType.SecondPlayer:
    //            if (Input.GetKey(KeyCode.LeftArrow))
    //                OnLeftRightPlayer1?.Invoke(MoveType.LeftTurn);
    //            else if (Input.GetKey(KeyCode.RightArrow))
    //                OnLeftRightPlayer1?.Invoke(MoveType.RightTrun);
    //            break;
    //    }
    //}

    //void RunInput()
    //{
    //    if(!CanMove)
    //    {
    //        OnRunPlayer1?.Invoke(false);
    //        return;
    //    }

    //    switch(playerType)
    //    {
    //        case PlayerType.FirstPlayer:
    //            OnRunPlayer1?.Invoke(Input.GetKey(KeyCode.LeftShift));
    //            break;
    //        case PlayerType.SecondPlayer:
    //            if (Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.RightShift))
    //                OnRunPlayer1?.Invoke(true);
    //            else if (!(Input.GetKey(KeyCode.RightControl)) && !(Input.GetKey(KeyCode.RightShift)))
    //                OnRunPlayer1?.Invoke(false);
    //            break;
    //    }
       
    //}

    //void UseInput()
    //{
    //    switch(playerType)
    //    {
    //        case PlayerType.FirstPlayer:
    //            Debug.Log("d");
    //            if (Input.GetKey(KeyCode.E))
    //            {
    //                if (CanMove)
    //                    CanMove = false;
    //                else
    //                    CanMove = true;
    //            }
    //            break;

    //        case PlayerType.SecondPlayer:
    //            if (Input.GetKey(KeyCode.P))
    //            {
    //                if (CanMove)
    //                    CanMove = false;
    //                else
    //                    CanMove = true;
    //            }
    //            break;
    //    }
    //    //if (playerType == PlayerType.FirstPlayer)
    //    //{
    //    //    OnUse.Invoke();

    //    //}
    //    //else if (playerType == PlayerType.SecondPlayer)
    //    //{
    //    //    OnUse.Invoke();
    //    //}
    //}

    //public void CanMoveChange(bool Change)
    //{
    //    CanMove = Change;
    //}
}
