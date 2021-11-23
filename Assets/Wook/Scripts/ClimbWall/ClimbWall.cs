using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ��������� ������ ��
/// </summary>
public class ClimbWall : MonoBehaviour
{

    //�÷��̾ ���� �ö󰬴���(�ѹ��� ������ ����)
    [SerializeField] bool isPlayer1Climb = false;
    [SerializeField] bool isPlayer2Climb = false;

    //�÷��̾ ������ �ö󰬴���(�ѹ��� ������ ����)
    [SerializeField] bool isPlayer1ClimbRope = false;
    [SerializeField] bool isPlayer2ClimbRope = false;

    //������ ����������
    [SerializeField] bool isRope;

    //��Ÿ�� ��ȣ�ۿ� ���� ����
    [SerializeField] ObjectUIShow ClimbObjectShowP1;
    [SerializeField] ObjectUIShow ClimbObjectShowP2;
    //���� Ÿ�� ��ȣ�ۿ� ���� ����
    [SerializeField] ObjectUIShow RopeObjectShowP1;
    [SerializeField] ObjectUIShow RopeObjectShowP2;
    //���� ������ ��ȣ�ۿ� ���� ����
    [SerializeField] ObjectUIShow RopeDownObjectShowP1;
    [SerializeField] ObjectUIShow RopeDownObjectShowP2;

    //����
    [SerializeField] GameObject gRope;
    private void Start()
    {
        SetInput();

    }

    void SetInput()
    {
        InputManager.Instance.OnUsePlayer1 += UseObject;
        InputManager.Instance.OnUsePlayer2 += UseObject;
    }
    //�÷��̾� Climb�� ��ȣ�ۿ� üũ
    void UseObject(PlayerType _playerType, PlayerState _playerState)
    {
        if(isRope)
        {
            switch(_playerType)
            {
                case PlayerType.FirstPlayer:
                     if (isPlayer1Climb)
                        return;
                    break;
                case PlayerType.SecondPlayer:
                    if (isPlayer2Climb)
                        return;
                    break;
            }
        }
        //����������
        if (isRope == false &&(RopeDownObjectShowP1.GetCanUse() || RopeDownObjectShowP2.GetCanUse()))
        {
            gRope.SetActive(true);
            isRope = true;
            return;
        }

        //��Ÿ�� or ����Ÿ��
        if (isRope == false)
            Climbwall(_playerType);
        else
            RopeClimb(_playerType);


    }

    void RopeClimb(PlayerType _playerType)
    {

        switch(_playerType)
        {
            case PlayerType.FirstPlayer:
                if (isPlayer1ClimbRope)
                    return;

                isPlayer1ClimbRope = true;
                PlayerStateChange(RopeObjectShowP1, PlayerState.ClimbRope);

                break;
            case PlayerType.SecondPlayer:
                if (isPlayer2ClimbRope)
                    return;

                isPlayer2ClimbRope = true;
                PlayerStateChange(RopeObjectShowP2, PlayerState.ClimbRope);
                break;
        }
    }

    #region ��Ÿ��
    //��Ÿ�� ����
    void Climbwall(PlayerType _playerType)
    {
        Transform playerTrans;
        Transform playerModelTrans;
        switch (_playerType)
        {
            case PlayerType.FirstPlayer:
                //�÷��̾ �ش� ������Ʈ�� ��밡���� ��������
                if (!ClimbObjectShowP1.GetCanUse())
                    return;
                if (isPlayer1Climb == true)
                    return;
                //�÷��̾ �尩 �������� �������ִ��� Ȯ��
                if (!GameManager.Instance.IsHaveItem(PlayerType.FirstPlayer, "Glove"))
                    return;

                isPlayer1Climb = true;
                //�÷��̾� ���� ����
                PlayerStateChange(ClimbObjectShowP1, PlayerState.ClimbWall);
                break;

            case PlayerType.SecondPlayer:
                //�÷��̾ �ش� ������Ʈ�� ��밡���� ��������
                if (!ClimbObjectShowP2.GetCanUse())
                    return;
                if (isPlayer2Climb == true)
                    return;
                //�÷��̾ �尩 �������� �������ִ��� Ȯ��
                if (!GameManager.Instance.IsHaveItem(PlayerType.SecondPlayer, "Glove"))
                    return;

                isPlayer2Climb = true;
                //�÷��̾� ���� ����
                PlayerStateChange(ClimbObjectShowP2, PlayerState.ClimbWall);
                break;
        }
    }

    //�÷��̾� ���� ���� -> Climb��
    void PlayerStateChange(ObjectUIShow _objectUIShow, PlayerState _playerState)
    {
        PlayerType playerType = _objectUIShow.GetPlayerType();
        Transform playerTrans = GameManager.Instance.GetPlayerTrans(playerType);
        Transform playerModelTrans = GameManager.Instance.GetPlayerModelTrans(playerType);
        //�÷��̾� rotation����
        playerTrans.rotation = Quaternion.Euler(0, 0, 0);
        //���� rotation�� ���� ���� ����
        playerModelTrans.rotation = new Quaternion(0, transform.rotation.y, 0, transform.rotation.w);
        //�÷��̾� ���� ����
        GameManager.Instance.PlayerStateChange(playerType, _playerState);

        //�÷��̾ ��Ÿ�⸦ �����Ƿ� player1ObjectShow,player2ObjectShow gameobject �����
        ClimbObjectShowP1.gameObject.SetActive(false);
        ClimbObjectShowP2.gameObject.SetActive(false);
    }
    #endregion
}
