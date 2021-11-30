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

    [SerializeField] ClimbingUpWall climbingUpWall;

    

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
            GameManager.Instance.PlayerStateChange(_playerType, PlayerState.HoldRope);
            //Transform PlayerTrans = GameManager.Instance.GetPlayerTrans(_playerType);
            ////�÷��̾� ȸ�� ������ �ٶ󺸵���
            //float angle = Vector3.Angle(PlayerTrans.position, gRope.transform.position);
            //PlayerTrans.LookAt(gRope.transform.position);
            //PlayerTrans.rotation = Quaternion.Euler(0, PlayerTrans.eulerAngles.y, 0);
            GameManager.Instance.GetPlayerController(_playerType).StartHoldRope(gRope.transform.position);
            return;
        }

        //��Ÿ�� or ����Ÿ��
        switch(_playerType)
        {
            case PlayerType.FirstPlayer:
                if (isRope == false)
                {
                    if (ClimbObjectShowP1.GetCanUse())
                        Climbwall(_playerType);
                }                
                else
                {
                    if (RopeObjectShowP1.GetCanUse())
                        RopeClimb(_playerType);
                }                    
                break;


            case PlayerType.SecondPlayer:
                if (isRope == false)
                {
                    if (ClimbObjectShowP2.GetCanUse())
                        Climbwall(_playerType);
                }

                else
                {
                    if (RopeObjectShowP2.GetCanUse())
                        RopeClimb(_playerType);
                }
                break;
        }
       


    }

    void RopeClimb(PlayerType _playerType)
    {

        switch(_playerType)
        {
            case PlayerType.FirstPlayer:
                if (isPlayer1ClimbRope)
                    return;

                isPlayer1ClimbRope = true;
                PlayerStateChange(RopeObjectShowP1, PlayerState.ClimbRope, _playerType);

                break;
            case PlayerType.SecondPlayer:
                if (isPlayer2ClimbRope)
                    return;

                isPlayer2ClimbRope = true;
                PlayerStateChange(RopeObjectShowP2, PlayerState.ClimbRope, _playerType);
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
                PlayerStateChange(ClimbObjectShowP1, PlayerState.ClimbWall, _playerType);
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
                PlayerStateChange(ClimbObjectShowP2, PlayerState.ClimbWall, _playerType);
                break;
        }
    }

    //�÷��̾� ���� ���� -> Climb��
    void PlayerStateChange(ObjectUIShow _objectUIShow, PlayerState _playerState,PlayerType _playerType)
    {
        PlayerType playerType = _objectUIShow.GetPlayerType();
        Transform playerTrans = GameManager.Instance.GetPlayerTrans(playerType);
        Transform playerModelTrans = GameManager.Instance.GetPlayerModelTrans(playerType);

        //�÷��̾� rotation����
        //playerTrans.rotation = Quaternion.Euler();
        float angle = Vector3.Angle(playerTrans.position, transform.position);
        playerTrans.LookAt(transform.position);
        playerTrans.rotation = Quaternion.Euler(0, playerTrans.eulerAngles.y, 0);
        //���� rotation�� ���� ���� ����
        playerModelTrans.localRotation = Quaternion.Euler(8.2f, 0, 0);
        playerModelTrans.localPosition = new Vector3(0, 0, -0.146f);
        //�÷��̾� ���� ����
        GameManager.Instance.PlayerStateChange(playerType, _playerState);

        //�÷��̾ ��Ÿ�⸦ �����Ƿ� player1ObjectShow,player2ObjectShow gameobject �����
        ClimbObjectShowP1.gameObject.SetActive(false);
        ClimbObjectShowP2.gameObject.SetActive(false);

        //ClimbWallUp End Pos����

        climbingUpWall.ChangeEndPos(_playerType, playerTrans.position);
    }
    #endregion
}
