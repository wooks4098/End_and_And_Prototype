using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerType
{//1P 2P
    FirstPlayer = 0,
    SecondPlayer,
}
//플레이어 행동 상태
public enum PlayerState
{
    Walk = 0,
    ClimbWall,
    Wait, //대기상태

}

public class PlayerController : MonoBehaviour
{
    [SerializeField] PlayerType playerType;
    [SerializeField] PlayerState playerState = PlayerState.Walk;

    [SerializeField] float moveSpeed; //이동속도
    [SerializeField] float rotateSpeed; //회전속도
    Vector3 moveDirection; //이동방향
    bool isRun = false;

    CharacterController characterController;
    [SerializeField] Transform CameraTransform;
    [SerializeField] Animator ani;
    [SerializeField] PlayerInput playerInput;
    //test
    [SerializeField] GameObject playerCamera;


    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        ani = GetComponentInChildren<Animator>();
        playerInput = GetComponent<PlayerInput>();
        playerInput.SetPlayerType(playerType);
        //input event 등록
        playerInput.OnMove += Move;
        playerInput.OnRotation += Rotation;
        playerInput.OnRun += Run;
        //playerInput.OnUse += UseObject;

    }

    private void Update()
    {
        MoveTo(moveDirection);
    }
    void MoveTo(Vector3 direction)
    {
        characterController.Move(direction * (isRun == false ? moveSpeed : moveSpeed * 2.3f) * Time.deltaTime);

    }
    void Move(MoveType moveType)
    {
        switch(playerState)
        {
            case PlayerState.Walk:
                FrontBackWalk(moveType);
                break;
            case PlayerState.ClimbWall:
                Climb(moveType);
                break;
        }
    }

    void FrontBackWalk(MoveType moveType)
    {
        if (moveType == MoveType.Front)
        {
            moveDirection = GetDirection(InputDir.front, PlayerState.Walk);
            ani.SetBool("WalkFront", true);
            ani.SetBool("WalkBack", false);
        }
        else if (moveType == MoveType.Back)
        {
            moveDirection = GetDirection(InputDir.back, PlayerState.Walk);
            ani.SetBool("WalkFront", false);
            ani.SetBool("WalkBack", true);
            isRun = false;
        }
        else
        {
            moveDirection = Vector3.zero;
            ani.SetBool("WalkFront", false);
            ani.SetBool("WalkBack", false);
            isRun = false;
        }
    }

    void Climb(MoveType moveType)
    {
        if (moveType == MoveType.Front)
        {
            moveSpeed = 1;
            moveDirection = -GetDirection(InputDir.front, PlayerState.ClimbWall);
            ani.SetBool("IsClimb", true);

            //var cameraFowardDirection = CameraTransform.forward;
            //Vector3 directionToMoveIn = Vector3.Scale(cameraFowardDirection, (Vector3.right + Vector3.up));
            //moveDirection = directionToMoveIn*-1;
            //moveDirection = GetDirection(InputDir.front);
            //moveDirection = Vector3.Cross(moveDirection, Vector3.);

        }
        else if (moveType == MoveType.Back)
        {
            moveSpeed = 1;
            moveDirection = -GetDirection(InputDir.back, PlayerState.ClimbWall);

        }
        else
        {
            moveDirection = Vector3.zero;
            
        }
        Debug.DrawRay(Camera.main.transform.position, moveDirection * 10, Color.blue);
    }

    void Rotation(MoveType moveType)
    {
        if (playerState == PlayerState.ClimbWall)
            return;
        // ani.SetBool("Run", isRun);
        if (moveType == MoveType.LeftTurn)
        {
            transform.Rotate(new Vector3(0, -rotateSpeed * Time.deltaTime, 0));
        }
        else if (moveType == MoveType.RightTrun)
        {
            transform.Rotate(new Vector3(0, rotateSpeed * Time.deltaTime, 0));
        }
    }

    void Run(bool _isRun)
    {
        if (playerState == PlayerState.ClimbWall)
            return;
        if (_isRun)
            isRun = _isRun;
        else
            isRun = false;
    }


    Vector3 GetDirection(InputDir _inputDir, PlayerState _playerState)
    {
        Vector3 MoveDir = Vector3.zero; //이동방향
        switch (_playerState)
        {
            case PlayerState.Walk:
                MoveDir = Vector3.forward;
                break;
            case PlayerState.ClimbWall:
                MoveDir = Vector3.up;
                break;
        }
        var cameraFowardDirection = CameraTransform.forward;
        Vector3 directionToMoveIn = Vector3.Scale(cameraFowardDirection, (Vector3.right + MoveDir));
        //Debug.DrawRay(Camera.main.transform.position, cameraFowardDirection * 10, Color.red);
        //Debug.DrawRay(Camera.main.transform.position, directionToMoveIn * 10, Color.blue);

        if (_inputDir == InputDir.front)
        {
            return directionToMoveIn;
        }
        else if (_inputDir == InputDir.back)
        {
            return -directionToMoveIn;
        }
        return directionToMoveIn;
    }

    public void PlayerCanMoveChange(bool CanMove)
    {
        playerInput.CanMoveChange(CanMove);


    }

    public void PlayerStateChange(PlayerState _playerState)
    {
        playerState = _playerState;
    }

}
