using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }
    PlayerController player1;
    PlayerController player2;

    GameObject gPlayer1;
    GameObject gPlayer2;

    private void Awake()
    {

        if (null == instance)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        player1 = GameObject.FindWithTag("Player1").gameObject.GetComponent<PlayerController>();
        player2 = GameObject.FindWithTag("Player2").gameObject.GetComponent<PlayerController>();
        gPlayer1 = player1.GetComponent<GameObject>();
        gPlayer2 = player2.GetComponent<GameObject>();
    }

    //플레이어 움직임 Move관련 input제어
    //public void PlayerCanMoveChange(PlayerType playerType, bool CanMove)
    //{
    //    switch (playerType)
    //    {
    //        case PlayerType.FirstPlayer:
    //            player1.PlayerCanMoveChange(CanMove);
    //            break;
    //        case PlayerType.SecondPlayer:
    //            player2.PlayerCanMoveChange(CanMove);
    //            break;
    //    }
    //}

    public void PlayerStateChange(PlayerType _playerType, PlayerState _playerState)
    {
        switch (_playerType)
        {
            case PlayerType.FirstPlayer:
                player1.PlayerStateChange(_playerState);
                break;
            case PlayerType.SecondPlayer:
                player2.PlayerStateChange(_playerState);
                break;
        }
    }

    /// <summary>
    /// 플레이어가 오브젝트 근처에 간 경우
    /// UI를 보여주고, 어떤 플레이어(1P, 2P)인지 오브젝트에 전달
    /// </summary>
    public void PlayerObjectHitin(PlayerType _playerType)
    {
        UIManager.Instance.ObjectUIShow(_playerType);

    }

    public void ObjectUiMove(PlayerType _playerType, Vector3 _UiPos)
    {
        UIManager.Instance.ObjectUIMove(_playerType, _UiPos);
    }

    public void PlayerObjectHitout(PlayerType _playerType)
    {
        UIManager.Instance.ObjectUIHide(_playerType);

    }


    public Transform GetPlayerTrans(PlayerType _playerType)
    {
        switch (_playerType)
        {
            case PlayerType.FirstPlayer:
                return player1.transform;
            case PlayerType.SecondPlayer:
                return player2.transform;

        }
        return null;
    }

    public Transform GetPlayerModelTrans(PlayerType _playerType)
    {
        switch (_playerType)
        {
            case PlayerType.FirstPlayer:
                return player1.GetComponentInChildren<Animator>().transform;
            case PlayerType.SecondPlayer:
                return player2.GetComponentInChildren<Animator>().transform;

        }
        return null;
    }
}
