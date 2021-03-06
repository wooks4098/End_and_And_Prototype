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
                        
            // 카메라가 활성되어있지 않을 때만  실행
            if (isZoomInCameraActive != true)
            {
                // 줌인 카메라 실행
                OpenSafeBox();
                // 금고 기믹 입장 이벤트 실행
                OnTriggerEnterSafeBox.Invoke();
            }
            // isActive = true -> 카메라가 활성화 되어있다는 뜻
            // 앞의 두 메서드를 실행시킨 후 bool 변수를 true로 변경한다.
            isZoomInCameraActive = true;

            //GameManager.Instance.PlayerCameraOnOFF(PlayerType.SecondPlayer, true);
            GameManager.Instance.PlayerMeshRendererOnOFF(PlayerType.SecondPlayer, false);
        }
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

        safeboxManager.CheckAllCorrect(false, safeboxManager.SafeboxB);
    }

    //inputManager에 등록
    protected override void SetInputKey()
    {
        InputManager.Instance.OnUsePlayer2 += InputActiveKey;
        InputManager.Instance.OnUsePlayer2 += InputSelectKey;
        InputManager.Instance.OnLeftRightPlayer2 += InputMoveKey;
    }

    //inputManager에 해제

    public void OutInputKey()
    {
        InputManager.Instance.OnUsePlayer1 -= InputActiveKey;
        InputManager.Instance.OnUsePlayer1 -= InputSelectKey;
        InputManager.Instance.OnLeftRightPlayer1 -= InputMoveKey;
    }
}