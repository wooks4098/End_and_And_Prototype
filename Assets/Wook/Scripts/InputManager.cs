using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ObjectType
{
    Object1 = 0, //�ݰ�
    Object2, //�ݰ�
}


public class InputManager : MonoBehaviour, IInput
{
    private static InputManager instance;
    public static InputManager Instance { get { return instance; } }

    #region InputAction
    //Input Action �ʿ��� ��ü�� �����Ͽ� ��ǲ�� ����
    public Action<MoveType, PlayerState> OnFrontBackPlayer1 { get; set; }
    public Action<MoveType, PlayerState> OnFrontBackPlayer2 { get; set; }
    public Action<MoveType, PlayerState> OnLeftRightPlayer1 { get; set; }
    public Action<MoveType, PlayerState> OnLeftRightPlayer2 { get; set; }
    public Action<bool> OnRunPlayer1 { get; set; }
    public Action<bool> OnRunPlayer2 { get; set; }
    public Action<PlayerType, PlayerState> OnUsePlayer1 { get; set; }
    public Action<PlayerType, PlayerState> OnUsePlayer2 { get; set; }
    public Action OnInventoryOpenPlayer1 { get; set; }
    public Action OnInventoryOpenPlayer2 { get; set; }
  public Action OnQuickSolt1Player1 { get; set; }
  public Action OnQuickSolt1Player2 { get; set; }
  public Action OnQuickSolt2Player1 { get; set; }
  public Action OnQuickSolt2Player2 { get; set; }
  public Action OnQuickSolt3Player1 { get; set; }
  public Action OnQuickSolt3Player2 { get; set; }
   #endregion

    //�÷��̾� ��Ʈ�ѷ�
    [SerializeField] PlayerController player1;
    [SerializeField] PlayerController player2;
    //�÷��̾� ����
    PlayerState player1State;
    PlayerState player2State;
    private void Awake()
    {
        if (null == instance)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }


    private void Update()
    {
        //�÷��̾� ���� ���
        player1State = player1.GetPlayerState();
        player2State = player2.GetPlayerState();
        //Input
        OnForntBack();
        OnLeftRight();
        OnRun();
        OnUse();
        OnOpenInventory();


    }

    #region Move

    void OnForntBack()
    {
        //�÷��̾�1
        switch (player1State)
        {
            case PlayerState.Walk:
            case PlayerState.ClimbWall:
            //case PlayerState.ClimbUpWall:
            case PlayerState.ClimbRope:
                //GetKey
                if (Input.GetKey(KeyCode.W))
                    OnFrontBackPlayer1?.Invoke(MoveType.Front, player1State);
                else if (Input.GetKey(KeyCode.S))
                    OnFrontBackPlayer1?.Invoke(MoveType.Back, player1State);
                else
                    OnFrontBackPlayer1?.Invoke(MoveType.Stay, player1State);
                break;
            case PlayerState.Inventory:
                //GetKeyDown
                if (Input.GetKeyDown(KeyCode.W))
                    OnFrontBackPlayer1?.Invoke(MoveType.Front, player1State);
                else if (Input.GetKeyDown(KeyCode.S))
                    OnFrontBackPlayer1?.Invoke(MoveType.Back, player1State);
                else
                    OnFrontBackPlayer1?.Invoke(MoveType.Stay, player1State);
                break;
        }


        //�÷��̾�2
        switch (player2State)
        {
            case PlayerState.Walk:
            case PlayerState.ClimbWall:
            case PlayerState.ClimbRope:
                //GetKey
                if (Input.GetKey(KeyCode.UpArrow))
                    OnFrontBackPlayer2?.Invoke(MoveType.Front, player2State);
                else if (Input.GetKey(KeyCode.DownArrow))
                    OnFrontBackPlayer2?.Invoke(MoveType.Back, player2State);
                else
                    OnFrontBackPlayer2?.Invoke(MoveType.Stay, player2State);
                break;
            case PlayerState.Inventory:
                //GetKeyDown
                if (Input.GetKeyDown(KeyCode.UpArrow))
                    OnFrontBackPlayer2?.Invoke(MoveType.Front, player2State);
                else if (Input.GetKeyDown(KeyCode.DownArrow))
                    OnFrontBackPlayer2?.Invoke(MoveType.Back, player2State);
                else
                    OnFrontBackPlayer2?.Invoke(MoveType.Stay, player2State);
                break;
        }
    }

    void OnLeftRight()
    {
        //�÷��̾�1
        switch (player1State)
        {
            case PlayerState.Walk:
            //case PlayerState.ClimbWall:
                //GetKey
                if (Input.GetKey(KeyCode.A))
                    OnLeftRightPlayer1?.Invoke(MoveType.Left, player1State);
                else if (Input.GetKey(KeyCode.D))
                    OnLeftRightPlayer1?.Invoke(MoveType.Right, player1State);
                break;
            case PlayerState.Inventory:
            case PlayerState.SafeBox:
                //GetKeyDown
                if (Input.GetKeyDown(KeyCode.A))
                    OnLeftRightPlayer1?.Invoke(MoveType.Left, player1State);
                else if (Input.GetKeyDown(KeyCode.D))
                    OnLeftRightPlayer1?.Invoke(MoveType.Right, player1State);
                break; 

        }
        //�÷��̾�2
        switch (player2State)
        {   //GetKey
            case PlayerState.Walk:
                if (Input.GetKey(KeyCode.LeftArrow))
                    OnLeftRightPlayer2?.Invoke(MoveType.Left, player2State);
                else if (Input.GetKey(KeyCode.RightArrow))
                    OnLeftRightPlayer2?.Invoke(MoveType.Right, player2State);
                break;
            //GetKeyDown
            case PlayerState.Inventory:                
            case PlayerState.SafeBox:                
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                    OnLeftRightPlayer2?.Invoke(MoveType.Left, player2State);
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                    OnLeftRightPlayer2?.Invoke(MoveType.Right, player2State);
                break;
        }
    }

    void OnRun()
    {
        //�÷��̾�1
        OnRunPlayer1?.Invoke(Input.GetKey(KeyCode.LeftShift));
        //�÷��̾�2
        if (Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.RightShift))
            OnRunPlayer2?.Invoke(true);
        else if (!(Input.GetKey(KeyCode.RightControl)) && !(Input.GetKey(KeyCode.RightShift)))
            OnRunPlayer2?.Invoke(false);
    }

    #endregion


    #region Use
    void OnUse()
    {
        //�÷��̾�1
        if (Input.GetKeyDown(KeyCode.K))
            OnUsePlayer1?.Invoke(PlayerType.FirstPlayer, player1State);

        //�÷��̾�2
        if (Input.GetKeyDown(KeyCode.Keypad2))
            OnUsePlayer2?.Invoke(PlayerType.SecondPlayer, player2State);

    }

    #endregion 

    #region Inventory

    void OnOpenInventory()
    {
        //�÷��̾�1
        if (Input.GetKeyDown(KeyCode.I))
            OnInventoryOpenPlayer1?.Invoke();

        //�÷��̾�2
        if (Input.GetKeyDown(KeyCode.Keypad4))
            OnInventoryOpenPlayer2?.Invoke();
    }

    void OnQuickSolt1()
    {
        //�÷��̾�1
        if (Input.GetKeyDown(KeyCode.Alpha1))
            OnQuickSolt1Player1!.Invoke();

        //�÷��̾�2
        if (Input.GetKeyDown(KeyCode.Keypad7))
            OnQuickSolt1Player2!.Invoke();
    }

    void OnQuickSolt2()
    {        //�÷��̾�1
        if (Input.GetKeyDown(KeyCode.Alpha2))
            OnQuickSolt2Player1!.Invoke();

        //�÷��̾�2
        if (Input.GetKeyDown(KeyCode.Keypad8))
            OnQuickSolt2Player2!.Invoke();
    }

    void OnQuickSolt3()
    {
        //�÷��̾�1
        if (Input.GetKeyDown(KeyCode.Alpha3))
            OnQuickSolt3Player1!.Invoke();

        //�÷��̾�2
        if (Input.GetKeyDown(KeyCode.Keypad9))
            OnQuickSolt3Player2!.Invoke();
    }

    #endregion
}
