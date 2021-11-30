using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 협동기믹중 오르는 벽
/// </summary>
public class ClimbWall : MonoBehaviour
{

    //플레이어가 벽을 올라갔는지(한번만 오르기 가능)
    [SerializeField] bool isPlayer1Climb = false;
    [SerializeField] bool isPlayer2Climb = false;

    //플레이어가 로프를 올라갔는지(한번만 오르기 가능)
    [SerializeField] bool isPlayer1ClimbRope = false;
    [SerializeField] bool isPlayer2ClimbRope = false;

    //로프가 내려갔는지
    [SerializeField] bool isRope;

    //벽타기 상호작용 가능 여부
    [SerializeField] ObjectUIShow ClimbObjectShowP1;
    [SerializeField] ObjectUIShow ClimbObjectShowP2;
    //로프 타기 상호작용 가능 여부
    [SerializeField] ObjectUIShow RopeObjectShowP1;
    [SerializeField] ObjectUIShow RopeObjectShowP2;
    //로프 내리기 상호작용 가능 여부
    [SerializeField] ObjectUIShow RopeDownObjectShowP1;
    [SerializeField] ObjectUIShow RopeDownObjectShowP2;

    //로프
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
    //플레이어 Climb와 상호작용 체크
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
        //로프내리기
        if (isRope == false &&(RopeDownObjectShowP1.GetCanUse() || RopeDownObjectShowP2.GetCanUse()))
        {
            gRope.SetActive(true);
            isRope = true;
            GameManager.Instance.PlayerStateChange(_playerType, PlayerState.HoldRope);
            //Transform PlayerTrans = GameManager.Instance.GetPlayerTrans(_playerType);
            ////플레이어 회전 로프를 바라보도록
            //float angle = Vector3.Angle(PlayerTrans.position, gRope.transform.position);
            //PlayerTrans.LookAt(gRope.transform.position);
            //PlayerTrans.rotation = Quaternion.Euler(0, PlayerTrans.eulerAngles.y, 0);
            GameManager.Instance.GetPlayerController(_playerType).StartHoldRope(gRope.transform.position);
            return;
        }

        //벽타기 or 로프타기
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

    #region 벽타기
    //벽타기 시작
    void Climbwall(PlayerType _playerType)
    {
        Transform playerTrans;
        Transform playerModelTrans;
        switch (_playerType)
        {
            case PlayerType.FirstPlayer:
                //플레이어가 해당 오브젝트를 사용가능한 상태인지
                if (!ClimbObjectShowP1.GetCanUse())
                    return;
                if (isPlayer1Climb == true)
                    return;
                //플레이어가 장갑 아이템을 가지고있는지 확인
                if (!GameManager.Instance.IsHaveItem(PlayerType.FirstPlayer, "Glove"))
                    return;

                isPlayer1Climb = true;
                //플레이어 상태 변경
                PlayerStateChange(ClimbObjectShowP1, PlayerState.ClimbWall, _playerType);
                break;

            case PlayerType.SecondPlayer:
                //플레이어가 해당 오브젝트를 사용가능한 상태인지
                if (!ClimbObjectShowP2.GetCanUse())
                    return;
                if (isPlayer2Climb == true)
                    return;
                //플레이어가 장갑 아이템을 가지고있는지 확인
                if (!GameManager.Instance.IsHaveItem(PlayerType.SecondPlayer, "Glove"))
                    return;

                isPlayer2Climb = true;
                //플레이어 상태 변경
                PlayerStateChange(ClimbObjectShowP2, PlayerState.ClimbWall, _playerType);
                break;
        }
    }

    //플레이어 상태 변경 -> Climb로
    void PlayerStateChange(ObjectUIShow _objectUIShow, PlayerState _playerState,PlayerType _playerType)
    {
        PlayerType playerType = _objectUIShow.GetPlayerType();
        Transform playerTrans = GameManager.Instance.GetPlayerTrans(playerType);
        Transform playerModelTrans = GameManager.Instance.GetPlayerModelTrans(playerType);

        //플레이어 rotation변경
        //playerTrans.rotation = Quaternion.Euler();
        float angle = Vector3.Angle(playerTrans.position, transform.position);
        playerTrans.LookAt(transform.position);
        playerTrans.rotation = Quaternion.Euler(0, playerTrans.eulerAngles.y, 0);
        //모델의 rotation을 벽과 같게 변경
        playerModelTrans.localRotation = Quaternion.Euler(8.2f, 0, 0);
        playerModelTrans.localPosition = new Vector3(0, 0, -0.146f);
        //플레이어 상태 변경
        GameManager.Instance.PlayerStateChange(playerType, _playerState);

        //플레이어가 벽타기를 했으므로 player1ObjectShow,player2ObjectShow gameobject 숨기기
        ClimbObjectShowP1.gameObject.SetActive(false);
        ClimbObjectShowP2.gameObject.SetActive(false);

        //ClimbWallUp End Pos변경

        climbingUpWall.ChangeEndPos(_playerType, playerTrans.position);
    }
    #endregion
}
