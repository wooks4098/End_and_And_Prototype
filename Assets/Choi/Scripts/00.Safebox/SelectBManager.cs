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
            // ��ġ�ϴ��� �˻��� ����� availableList�� �߰��ȴ�. (true or false)
            availableList.Add(safeboxManager.IsMatch(i, safeboxManager.SafeboxB));
        }
    }
    protected override void InputActiveKey(PlayerType _playerType, PlayerState _playerState)
    {
        if (objectUiShow.GetCanUse() == true)
        {
            //�÷��̾� ���� ����
            GameManager.Instance.PlayerStateChange(PlayerType.SecondPlayer, PlayerState.SafeBox);
                        
            // ī�޶� Ȱ���Ǿ����� ���� ����  ����
            if (isZoomInCameraActive != true)
            {
                // ���� ī�޶� ����
                OpenSafeBox();
                // �ݰ� ��� ���� �̺�Ʈ ����
                OnTriggerEnterSafeBox.Invoke();
            }
            // isActive = true -> ī�޶� Ȱ��ȭ �Ǿ��ִٴ� ��
            // ���� �� �޼��带 �����Ų �� bool ������ true�� �����Ѵ�.
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
        // ���� �ݰ��� �ε����� 1��ŭ ������Ų��.
        // �ݰ��� �ε����� ����� ���͸���� ������ �ִ�.
        safeboxManager.SafeboxB[currentIndex] += 1;

        // ������ ����� ��츦 ����� ����ó��
        if (safeboxManager.SafeboxB[currentIndex] > safeboxManager.Origin.Count - 1)
        {
            safeboxManager.SafeboxB[currentIndex] = 0;
        }

        // SetMaterial() �� ȣ��
        safeboxManager.SetMaterial();

        safeboxManager.CheckAllCorrect(false, safeboxManager.SafeboxB);
    }

    //inputManager�� ���
    protected override void SetInputKey()
    {
        InputManager.Instance.OnUsePlayer2 += InputActiveKey;
        InputManager.Instance.OnUsePlayer2 += InputSelectKey;
        InputManager.Instance.OnLeftRightPlayer2 += InputMoveKey;
    }

    //inputManager�� ����

    public void OutInputKey()
    {
        InputManager.Instance.OnUsePlayer1 -= InputActiveKey;
        InputManager.Instance.OnUsePlayer1 -= InputSelectKey;
        InputManager.Instance.OnLeftRightPlayer1 -= InputMoveKey;
    }
}