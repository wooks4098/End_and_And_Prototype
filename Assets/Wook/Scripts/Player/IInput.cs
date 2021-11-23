using System;
using UnityEngine;

public enum MoveType
{
    Front = 0,
    Back,
    Stay,
    Left,
    Right,
    Run,
}



public interface IInput
{
    //앞 뒤 움직임
    Action<MoveType, PlayerState> OnFrontBackPlayer1 { get; set; }//w s
    Action<MoveType, PlayerState> OnFrontBackPlayer2 { get; set; }//↑↓

    //좌 우 움직임
    Action<MoveType, PlayerState> OnLeftRightPlayer1 { get; set; }//a d
    Action<MoveType, PlayerState> OnLeftRightPlayer2 { get; set; }//← →

    //달리기
    Action<bool> OnRunPlayer1 { get; set; }
    Action<bool> OnRunPlayer2 { get; set; }

    //사용키
    Action<PlayerType, PlayerState> OnUsePlayer1 { get; set; } 
    Action<PlayerType, PlayerState> OnUsePlayer2 { get; set; }

    //인벤토리 오픈
    Action OnInventoryOpenPlayer1 { get; set; } 
    Action OnInventoryOpenPlayer2 { get; set; } 

}