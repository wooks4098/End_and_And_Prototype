using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }
    //�÷��̾� ��Ʈ�ѷ�
    PlayerController player1;
    PlayerController player2;
    //�κ��丮
    [SerializeField] Inventory inventoryP1;
    [SerializeField] Inventory inventoryP2;
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

    #region �÷��̾� ����

    //�÷��̾� ���� ����
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

    public void PlayerCameraOnOFF(PlayerType _playerType, bool _state)
    {
        switch (_playerType)
        {
            case PlayerType.FirstPlayer:
                player1.PlayerCameraOnOFF(_state);
                break;
            case PlayerType.SecondPlayer:
                player2.PlayerCameraOnOFF(_state);
                break;
        }
    }

    public void PlayerMeshRendererOnOFF(PlayerType _playerType, bool _state)
    {
        switch (_playerType)
        {
            case PlayerType.FirstPlayer:
                player1.PlayerMeshRendererOnOFF(_state);
                break;
            case PlayerType.SecondPlayer:
                player2.PlayerMeshRendererOnOFF(_state);
                break;
        }
    }



    //�÷��̾� ����
    public PlayerState GetPlayerState(PlayerType _playerType)
    {
        switch (_playerType)
        {
            case PlayerType.FirstPlayer:
                return player1.GetPlayerState();
                
            case PlayerType.SecondPlayer:
                return player2.GetPlayerState();
                
        }
        return PlayerState.None;
    }
    public PlayerController GetPlayerController(PlayerType _playerType)
    {
        switch (_playerType)
        {
            case PlayerType.FirstPlayer:
                return player1;
            case PlayerType.SecondPlayer:
                return player2;

        }
        return null;
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
    //�÷��̾� �� transform����
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

    #endregion

    #region �κ��丮 ������ ����

    //�÷��̾ �������� ������ �ִ��� Ȯ���ϴ� �Լ�
    public bool IsHaveItem(PlayerType _playertype, string _ItemName)
    {
        switch (_playertype)
        {
            case PlayerType.FirstPlayer:
                return inventoryP1.IsHaveItem(_ItemName);
            case PlayerType.SecondPlayer:
                return inventoryP2.IsHaveItem(_ItemName);
        }
        return false;
    }

    public void GetItem(PlayerType _playertype,Item _item)
    {
        switch (_playertype)
        {
            case PlayerType.FirstPlayer:
                inventoryP1.AcquireItem(_item);
                break;
            case PlayerType.SecondPlayer:
                inventoryP2.AcquireItem(_item);
                break;
        }
    }

    #endregion

    #region UI����
    /// <summary>
    /// �÷��̾ ������Ʈ ��ó�� �� ���
    /// UI�� �����ְ�, � �÷��̾�(1P, 2P)���� ������Ʈ�� ����
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

    #endregion
}
