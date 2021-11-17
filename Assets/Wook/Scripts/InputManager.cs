using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ObjectType
{
    Object1 = 0, //금고
    Object2, //금고
}


public class InputManager : MonoBehaviour, IInput
{
    private static InputManager instance;
    public static InputManager Instance { get { return instance; } }
    public Action<MoveType, PlayerState> OnFrontBackPlayer1 { get; set; }
    public Action<MoveType, PlayerState> OnFrontBackPlayer2 { get; set; }
    public Action<MoveType, PlayerState> OnLeftRightPlayer1 { get; set; }
    public Action<MoveType, PlayerState> OnLeftRightPlayer2 { get; set; }
    public Action<bool> OnRunPlayer1 { get; set; }
    public Action<bool> OnRunPlayer2 { get; set; }
    public Action<PlayerState> OnUsePlayer1 { get; set; }
    public Action<PlayerState> OnUsePlayer2 { get; set; }

    [SerializeField] PlayerController player1;
    [SerializeField] PlayerController player2;
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
        player1State = player1.GetPlayerState();
        player2State = player2.GetPlayerState();
        OnForntBack();
        OnLeftRight();
        OnRun();
        OnUse();
    }

    void OnForntBack()
    {
        //플레이어1
        switch (player1State)
        {
            case PlayerState.Walk:
            case PlayerState.ClimbWall:
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


        //플레이어2
        switch (player2State)
        {
            case PlayerState.Walk:
            case PlayerState.ClimbWall:
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
        //플레이어1
        switch (player1State)
        {
            case PlayerState.Walk:
            case PlayerState.ClimbWall:
                //GetKey
                if (Input.GetKey(KeyCode.A))
                    OnLeftRightPlayer1?.Invoke(MoveType.Left, player1.GetPlayerState());
                else if (Input.GetKey(KeyCode.D))
                    OnLeftRightPlayer1?.Invoke(MoveType.Right, player1.GetPlayerState());
                break;
            case PlayerState.Inventory:
                //GetKeyDown
                if (Input.GetKeyDown(KeyCode.A))
                    OnLeftRightPlayer1?.Invoke(MoveType.Left, player1.GetPlayerState());
                else if (Input.GetKeyDown(KeyCode.D))
                    OnLeftRightPlayer1?.Invoke(MoveType.Right, player1.GetPlayerState());
                break; 

        }
        //플레이어2
        switch (player1State)
        {
            case PlayerState.Walk:
            case PlayerState.ClimbWall:
                //GetKey
                if (Input.GetKey(KeyCode.LeftArrow))
                    OnLeftRightPlayer2?.Invoke(MoveType.Left, player2.GetPlayerState());
                else if (Input.GetKey(KeyCode.RightArrow))
                    OnLeftRightPlayer2?.Invoke(MoveType.Right, player2.GetPlayerState());
                break;
            case PlayerState.Inventory:
                //GetKeyDown
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                    OnLeftRightPlayer2?.Invoke(MoveType.Left, player1.GetPlayerState());
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                    OnLeftRightPlayer2?.Invoke(MoveType.Right, player1.GetPlayerState());
                break;
        }
    }

    void OnRun()
    {
        //플레이어1
        OnRunPlayer1?.Invoke(Input.GetKey(KeyCode.LeftShift));
        Debug.Log(Input.GetKey(KeyCode.LeftShift));
        //플레이어2
        if (Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.RightShift))
            OnRunPlayer2?.Invoke(true);
        else if (!(Input.GetKey(KeyCode.RightControl)) && !(Input.GetKey(KeyCode.RightShift)))
            OnRunPlayer2?.Invoke(false);
    }

    void OnUse()
    {
        //플레이어1
        if (Input.GetKey(KeyCode.E))
            OnUsePlayer1?.Invoke(player1.GetPlayerState());

        //플레이어2
        if (Input.GetKey(KeyCode.Alpha2))
            OnUsePlayer1?.Invoke(player1.GetPlayerState());
    }
}
