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
    //�� �� ������
    Action<MoveType, PlayerState> OnFrontBackPlayer1 { get; set; }//w s
    Action<MoveType, PlayerState> OnFrontBackPlayer2 { get; set; }//���

    //�� �� ������
    Action<MoveType, PlayerState> OnLeftRightPlayer1 { get; set; }//a d
    Action<MoveType, PlayerState> OnLeftRightPlayer2 { get; set; }//�� ��

    //�޸���
    Action<bool> OnRunPlayer1 { get; set; }
    Action<bool> OnRunPlayer2 { get; set; }

    //���Ű
    Action<PlayerType, PlayerState> OnUsePlayer1 { get; set; } 
    Action<PlayerType, PlayerState> OnUsePlayer2 { get; set; }

    //�κ��丮 ����
    Action OnInventoryOpenPlayer1 { get; set; } 
    Action OnInventoryOpenPlayer2 { get; set; } 

}