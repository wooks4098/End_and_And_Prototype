using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum CameraState
{
    Division = 0, //����
    All, //��ü
}

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }
    //�÷��̾� ��Ʈ�ѷ�
    [SerializeField] PlayerController player1;
    [SerializeField] PlayerController player2;
    //�κ��丮
    [SerializeField] Inventory inventoryP1;
    [SerializeField] Inventory inventoryP2;
    GameObject gPlayer1;
    GameObject gPlayer2;

    [Header("Camera")]
    [SerializeField] CameraState cameraState; //���һ�������
    [SerializeField] GameObject[] DivisionCamera = new GameObject[2]; //����ī�޶�
    [SerializeField] GameObject AllCamera; //��üī�޶�
    private void Awake()
    {

        if (null == instance)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
        gPlayer1 = player1.GetComponent<GameObject>();
        gPlayer2 = player2.GetComponent<GameObject>();
    }

    public CameraState GetCameraState()
    {
        return cameraState;
    }

    public void ChangeCameraState(CameraState _camreaState)
    {
        cameraState = _camreaState;
        switch(cameraState)
        {
            case CameraState.Division:
                ChangeCameraDivision();
                break;
            case CameraState.All:
                ChangeCameraAll();
                break;
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            switch(cameraState)
            {
                case CameraState.All:
                    ChangeCameraState(CameraState.Division);
                    break;
                case CameraState.Division:
                    ChangeCameraState(CameraState.All);
                    break;
            }
        }
    }

    public void ChangeCameraDivision()
    {
        for (int i = 0; i < DivisionCamera.Length; i++)
            DivisionCamera[i].SetActive(true);
        AllCamera.SetActive(false);
        UIManager.Instance.ShowHideAllUI(false);
        UIManager.Instance.ShowHideDivisionUI(true);

    }

    //��ü ī�޶�� ����
    void ChangeCameraAll()
    {
        for (int i = 0; i < DivisionCamera.Length; i++)
            DivisionCamera[i].SetActive(false);
        AllCamera.SetActive(true);
        UIManager.Instance.ShowHideAllUI(true);
        UIManager.Instance.ShowHideDivisionUI(false);
    }


    #region �÷��̾� ����

    //�÷��̾� ������
    public void PlayerDamage(PlayerType _playerType,float _Damage)
    {
        switch (_playerType)
        {
            case PlayerType.FirstPlayer:
                player1.GetComponent<PlayerStatus>().ChangeHp(_Damage);
                break;
            case PlayerType.SecondPlayer:
                player2.GetComponent<PlayerStatus>().ChangeHp(_Damage);
                break;
        }
    }


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

    public void SheathSword(PlayerType _playerType)
    {
        switch (_playerType)
        {
            case PlayerType.FirstPlayer:
                player1.StartSheathSword();
                break;
            case PlayerType.SecondPlayer:
                player2.StartSheathSword();
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
    public bool GetUseSword(PlayerType _playerType)
    {
        switch (_playerType)
        {
            case PlayerType.FirstPlayer:
                return player1.GetUseSword();
            case PlayerType.SecondPlayer:
                return player2.GetUseSword();
        }
        return false;
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
    public void PlayerObjectHitin(PlayerType _playerType,string _text)
    {
        UIManager.Instance.ObjectUIShow(_playerType, _text);

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
