using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 협동기믹중 오르는 벽
/// </summary>
public class ClimbWall : MonoBehaviour
{
    bool isPlayergoup;//플레이어가 올라갔는지
    [SerializeField] bool isClimb = false;//오르는중인지
    [SerializeField] ObjectUIShow player1ObjectShow;
    [SerializeField] ObjectUIShow player2ObjectShow;
    private void Start()
    {
        SetInput();

    }
    //private void Update()
    //{
    //    //if (Input.GetKeyDown(KeyCode.E) && objectUIShow.GetCanUse() == true && isClimb == false)
    //    //{
    //    //    PlayerPanertChange();
    //    //}
    //}
    void SetInput()
    {
        InputManager.Instance.OnUsePlayer1 += PlayerPanertChange;
        InputManager.Instance.OnUsePlayer2 += PlayerPanertChange;
        //switch (playerType)
        //{
        //    case PlayerType.FirstPlayer:
        //        InputManager.Instance.OnUsePlayer1 += PlayerPanertChange;
        //        break;

        //    case PlayerType.SecondPlayer:
        //        InputManager.Instance.OnUsePlayer2 += PlayerPanertChange;

        //        break;
        //}
    }
    //플레이어 Climb시작
    void PlayerPanertChange(PlayerType _playerType, PlayerState _playerState)
    {
        Transform playerTrans;
        Transform playerModelTrans;
        switch (_playerType)
        {
            case PlayerType.FirstPlayer:
                playerTrans = GameManager.Instance.GetPlayerTrans(player1ObjectShow.GetPlayerType());
                playerModelTrans = GameManager.Instance.GetPlayerModelTrans(player1ObjectShow.GetPlayerType());
                playerTrans.rotation = Quaternion.Euler(0, 0, 0);
                playerModelTrans.rotation = new Quaternion(0, transform.rotation.y, 0, transform.rotation.w);
                isClimb = true;
                //플레이어 상태 변경
                GameManager.Instance.PlayerStateChange(player1ObjectShow.GetPlayerType(), PlayerState.ClimbWall);
                break;

            case PlayerType.SecondPlayer:
                playerTrans = GameManager.Instance.GetPlayerTrans(player2ObjectShow.GetPlayerType());
                playerModelTrans = GameManager.Instance.GetPlayerModelTrans(player2ObjectShow.GetPlayerType());
                playerTrans.rotation = Quaternion.Euler(0, 0, 0);
                playerModelTrans.rotation = new Quaternion(0, transform.rotation.y, 0, transform.rotation.w);
                isClimb = true;
                //플레이어 상태 변경
                GameManager.Instance.PlayerStateChange(player2ObjectShow.GetPlayerType(), PlayerState.ClimbWall);
                break;
        }
        
    }
}
