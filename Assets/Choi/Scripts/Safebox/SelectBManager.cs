using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectBManager : BaseSelectManager
{
    

    public override void CheckAvailable()
    {
        availableList.Clear();

        for (int i = 0; i < safeboxManager.Origin.Count; i++)
        {
            // 일치하는지 검사한 결과가 availableList에 추가된다. (true or false)
            availableList.Add(safeboxManager.IsMatch(i, safeboxManager.SafeboxB));
        }
    }
    protected override void InputActiveKey(PlayerType _playerType, PlayerState _playerState)
    {
        if (objectUiShow.GetCanUse() == true)
        {
            //플레이어 상태 변경
            GameManager.Instance.PlayerStateChange(PlayerType.SecondPlayer, PlayerState.SafeBox);
            isActive = true;
            OpenSafeBox();
            //GameManager.Instance.PlayerCameraOnOFF(PlayerType.SecondPlayer, true);
            GameManager.Instance.PlayerMeshRendererOnOFF(PlayerType.SecondPlayer, false);

        }

        //if (Input.GetKeyDown(KeyCode.Keypad2))
        //{
        //    isActive = true;
        //}
    }

    protected override void InputMoveKey(MoveType _moveType, PlayerState _playerState)
    {
        if (_playerState != PlayerState.SafeBox)
            return;
        switch (_moveType)
        {
            case MoveType.Left:
                MoveOnPrev();
                break;
            case MoveType.Right:
                MoveOnNext();
                break;
        }
        //// 왼쪽
        //if (Input.GetKeyDown(KeyCode.LeftArrow))
        //{
        //    MoveOnPrev();
        //}

        //// 오른쪽
        //if(Input.GetKeyDown(KeyCode.RightArrow))
        //{
        //    MoveOnNext();
        //}
    }


    protected override void InputSelectKey(PlayerType _playerType, PlayerState _playerState)
    {
        if (_playerState != PlayerState.SafeBox)
            return;
        // 현재 금고의 인덱스를 1만큼 증가시킨다.
        // 금고의 인덱스는 출력할 머터리얼과 관련이 있다.
        safeboxManager.SafeboxB[currentIndex] += 1;

        // 범위를 벗어났을 경우를 대비한 예외처리
        if (safeboxManager.SafeboxB[currentIndex] > safeboxManager.Origin.Count - 1)
        {
            safeboxManager.SafeboxB[currentIndex] = 0;
        }

        // SetMaterial() 을 호출
        safeboxManager.SetMaterial();

        safeboxManager.CheckAllCorrect(safeboxManager.SafeboxB);


        //if (Input.GetKeyDown(KeyCode.Keypad2))
        //{
        //    //Debug.Log("safeboxManager.SafeboxB[currentIndex[1]]" + safeboxManager.SafeboxB[currentIndex].ToString());
    
        //    // 현재 금고의 인덱스를 1만큼 증가시킨다.
        //    // 금고의 인덱스는 출력할 머터리얼과 관련이 있다.
        //    safeboxManager.SafeboxB[currentIndex] += 1;

        //    // 범위를 벗어났을 경우를 대비한 예외처리
        //    if (safeboxManager.SafeboxB[currentIndex] > safeboxManager.Origin.Count - 1)
        //    {
        //        safeboxManager.SafeboxB[currentIndex] = 0;
        //    }

        //    // SetMaterial() 을 호출
        //    safeboxManager.SetMaterial();

        //    safeboxManager.CheckAllCorrect(safeboxManager.SafeboxB);
        //}
    }

    //inputManager에 등록
    protected override void SetInputKey()
    {
        InputManager.Instance.OnUsePlayer2 += InputActiveKey;
        InputManager.Instance.OnUsePlayer2 += InputSelectKey;
        InputManager.Instance.OnLeftRightPlayer2 += InputMoveKey;

    }
}