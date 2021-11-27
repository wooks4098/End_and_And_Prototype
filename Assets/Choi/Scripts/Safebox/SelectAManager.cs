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
            // ��ġ�ϴ��� �˻��� ����� availableList�� �߰��ȴ�. (true or false)
            availableList.Add(safeboxManager.IsMatch(i, safeboxManager.SafeboxA));
        }
    }
    protected override void InputActiveKey(PlayerType _playerType, PlayerState _playerState)
    {
        if(objectUiShow.GetCanUse() == true)
        {
            //�÷��̾� ���� ����
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
        // ���� �ݰ��� �ε����� 1��ŭ ������Ų��.
        // �ݰ��� �ε����� ����� ���͸���� ������ �ִ�.
            safeboxManager.SafeboxA[currentIndex] += 1;

        // ������ ����� ��츦 ����� ����ó��
        if (safeboxManager.SafeboxA[currentIndex] > safeboxManager.Origin.Count - 1)
        {
            safeboxManager.SafeboxA[currentIndex] = 0;
        }
        // SetMaterial() �� ȣ��
        safeboxManager.SetMaterial();

        safeboxManager.CheckAllCorrect(true,safeboxManager.SafeboxA);
    }

    //inputManager�� ���
    protected override void SetInputKey()
    {
        InputManager.Instance.OnUsePlayer1 += InputActiveKey;
        InputManager.Instance.OnUsePlayer1 += InputSelectKey;
        InputManager.Instance.OnLeftRightPlayer1 += InputMoveKey;
    }
}