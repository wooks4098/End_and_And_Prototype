using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectAManager : BaseSelectManager
{
    public override void CheckAvailable()
    {
        availableList.Clear();

        for (int i = 0; i < safeboxManager.Origin.Count; i++)
        {
            // 일치하는지 검사한 결과가 availableList에 추가된다. (true or false)
            availableList.Add(safeboxManager.IsMatch(i, safeboxManager.SafeboxA));
        }
    }
    protected override void InputActiveKey(PlayerType _playerType, PlayerState _playerState)
    {
        if(objectUiShow.GetCanUse() == true)
        {
            //플레이어 상태 변경
            GameManager.Instance.PlayerStateChange(PlayerType.FirstPlayer, PlayerState.SafeBox);
            //isActive = true;

            OpenSafeBox();
            //GameManager.Instance.PlayerCameraOnOFF(PlayerType.FirstPlayer, false);
            GameManager.Instance.PlayerMeshRendererOnOFF(PlayerType.FirstPlayer, false);
        }
    }

    protected override void InputMoveKey(MoveType _moveType, PlayerState _playerState)
    {
        if (_playerState != PlayerState.SafeBox)
            return;
        switch(_moveType)
        {
            case MoveType.Left:
                MoveOnPrev();
                break;
            case MoveType.Right:
                MoveOnNext();
                break;
        }
    }


    protected override void InputSelectKey(PlayerType _playerType, PlayerState _playerState)
    {
        if (_playerState != PlayerState.SafeBox)
            return;
        // 현재 금고의 인덱스를 1만큼 증가시킨다.
        // 금고의 인덱스는 출력할 머터리얼과 관련이 있다.
            safeboxManager.SafeboxA[currentIndex] += 1;

        // 범위를 벗어났을 경우를 대비한 예외처리
        if (safeboxManager.SafeboxA[currentIndex] > safeboxManager.Origin.Count - 1)
        {
            safeboxManager.SafeboxA[currentIndex] = 0;
        }
        // SetMaterial() 을 호출
        safeboxManager.SetMaterial();

        safeboxManager.CheckAllCorrect(true,safeboxManager.SafeboxA);
    }

    //inputManager에 등록
    protected override void SetInputKey()
    {
        InputManager.Instance.OnUsePlayer1 += InputActiveKey;
        InputManager.Instance.OnUsePlayer1 += InputSelectKey;
        InputManager.Instance.OnLeftRightPlayer1 += InputMoveKey;
    }
}