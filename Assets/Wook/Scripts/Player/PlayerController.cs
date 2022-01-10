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
    Walk = 0, //걷는중
    ClimbWall, //벽 오르는중(벽타기)
    ClimbUpWall, //벽 올라가기
    Wait, //대기상태
    ClimbRope,//로프 오르는중
    //ClimbRopeUp,//로프 올라가기
    ClimbWallFall,//벽 타기중 떨어지기
    HoldRope,//로프 잡는중
    Inventory, //인벤토리 오픈
    SafeBox,//금고 사용중
    None, //Null로 사용
    Die,//죽음

    Attack,//공격
}

public class PlayerController : MonoBehaviour
{
    [SerializeField] PlayerType playerType;
    [SerializeField] PlayerState playerState = PlayerState.Walk;

    [SerializeField] float moveSpeed; //이동속도
    [SerializeField] float rotateSpeed; //회전속도
    [SerializeField] float gravity; //중력
    Vector3 moveDirection; //이동방향
    [SerializeField] bool isRun = false;

    CharacterController characterController;
    [SerializeField] Transform CameraTransform;
    [SerializeField] Animator ani;
    //[SerializeField] PlayerInput playerInput;
    //test
    [SerializeField] GameObject playerCamera;
    [SerializeField] SkinnedMeshRenderer meshRenderer;

    //벽타기 변수
    [SerializeField] int FallCount;
    [SerializeField] bool isClimbWallCheck;//벽타기시 클릭 가능한 상태인지
     Vector3 groundPos;
    [SerializeField] float toGroundLength; //땅까지의 거리
    [SerializeField] float fallLength; //벽타기시 떨어질 수 있는 거리

    //공격 변수
    [SerializeField] int AttackComboCount = 0;//콤보 번호 초기 0 | 공격종류 1~3
    [SerializeField] bool CanAttack = true; // 공격 가능한지
   // [SerializeField] bool CanNextCombo = true;//다음 공격으로 이어갈 수 있는지
    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        ani = GetComponentInChildren<Animator>();

    }
    private void Start()
    {
        //input event 등록
        SetInput();
    }

    private void Update()
    {
        //캐릭터 중력 적용
        SetGravity();
        //캐릭터 움직임
        MoveTo(moveDirection);
    }

    // 캐릭터 중력 적용
    void SetGravity()
    {
        switch (playerState)
        {
            case PlayerState.Walk:
                moveDirection.y -= gravity * Time.deltaTime;
                break;

        }


    }

    //input event 등록
    void SetInput()
    {
        switch (playerType)
        {
            case PlayerType.FirstPlayer:
                InputManager.Instance.OnFrontBackPlayer1 += Move;
                InputManager.Instance.OnLeftRightPlayer1 += Rotation;
                InputManager.Instance.OnRunPlayer1 += Run;
                InputManager.Instance.OnAttackPlayer1 += Attack;
                break;

            case PlayerType.SecondPlayer:
                InputManager.Instance.OnFrontBackPlayer2 += Move;
                InputManager.Instance.OnLeftRightPlayer2 += Rotation;
                InputManager.Instance.OnRunPlayer2 += Run;
                InputManager.Instance.OnAttackPlayer2 += Attack;

                break;
        }
    }

    #region 움직임 관련

    void MoveTo(Vector3 direction)
    {
        switch (playerState)
        {
            case PlayerState.ClimbUpWall:
            case PlayerState.SafeBox:
            case PlayerState.Inventory:
            case PlayerState.Attack:
                moveDirection = Vector3.zero;
                return;
        }

        characterController.Move(direction * (isRun == false ? moveSpeed : moveSpeed * 2.3f) * Time.deltaTime);
    }
    void Move(MoveType moveType, PlayerState _playerState)
    {
        switch(playerState)
        {
            case PlayerState.Attack:
                return;
        }
        switch (playerState)
        {
            case PlayerState.Walk:
                FrontBackWalk(moveType);
                break;
            case PlayerState.ClimbWall:
            case PlayerState.ClimbRope:
                //Climb(moveType);
                break;
            case PlayerState.ClimbUpWall:
                moveDirection = Vector3.zero;
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
        ani.SetBool("Run", isRun);
    }

    #region ClimbWall

    void ClimbWallStart()
    {
        FallCount = 3;
        isClimbWallCheck = true;
        //바닥 위치 등록
        groundPos = transform.position;
        switch (playerType)
        {
            case PlayerType.FirstPlayer:
                InputManager.Instance.OnUsePlayer1 += ClimbWallCheck;
                UIManager.Instance.StartClimbWall(playerType);
                break;
            case PlayerType.SecondPlayer:
                InputManager.Instance.OnUsePlayer2 += ClimbWallCheck;
                UIManager.Instance.StartClimbWall(playerType);
                break;
        }
    }

    public void ClimbWallEnd()
    {
        switch (playerType)
        {
            case PlayerType.FirstPlayer:
                InputManager.Instance.OnUsePlayer1 -= ClimbWallCheck;
                //UIManager.Instance.StartClimbWall(playerType);
                break;
            case PlayerType.SecondPlayer:
                InputManager.Instance.OnUsePlayer2 -= ClimbWallCheck;
                //UIManager.Instance.StartClimbWall(playerType);
                break;
        }
    }

    void ClimbWallCheck(PlayerType _playerType, PlayerState _playerState)
    {
        if (!isClimbWallCheck)
            return;
        if (UIManager.Instance.isSliderTriggerCheck(_playerType))
        {
            StartCoroutine(ClimbWallUp());
        }
        else
        {
            FallCount--;
            if (FallCount <= 0)
            {
                toGroundLength = Vector3.Distance(groundPos, transform.position);
                if(toGroundLength >= fallLength)
                {
                    //떨어지기
                    Debug.Log("Fall down");
                  
                    StartClimbWallFall();
                    return;
                }
                
            }
            //미끄러지기
            StartCoroutine(ClimbWallDown());

        }
    }

    IEnumerator ClimbWallUp()
    {
        float timeCheck = 1f;
        moveSpeed = 2;
        isClimbWallCheck = false;
        while (timeCheck >= 0)
        {
            timeCheck -= Time.deltaTime;
            moveDirection = Vector3.up;
            ani.SetFloat("ClimbSpeed", 1f);
            yield return null;
        }
        moveDirection = Vector3.zero;
        ani.SetFloat("ClimbSpeed", 0);
        isClimbWallCheck = true;
        yield return null;
    }

    //미끄러지기
    IEnumerator ClimbWallDown()
    {
        float timeCheck = 0.5f;
        moveSpeed = 5;
        isClimbWallCheck = false;
        while (timeCheck >= 0)
        {
            timeCheck -= Time.deltaTime;
            moveDirection = Vector3.down;
            ani.SetFloat("ClimbSpeed", -2.5f);
            yield return null;
        }
        moveDirection = Vector3.zero;
        ani.SetFloat("ClimbSpeed", 0);
        isClimbWallCheck = true;
        yield return null;
    }

    public void StartClimbWallFall(bool isRopeClimbUp = false)//isRopeClimbUp 로프를 타고 올라가 벽을 올라갔는지
    {
        if (playerState != PlayerState.ClimbRope && playerState != PlayerState.ClimbWall && playerState != PlayerState.HoldRope)
            return;
        PlayerStateChange(PlayerState.ClimbWallFall);
        ani.SetTrigger("ClimbupFall");
        StartCoroutine(ClimbWallFall(isRopeClimbUp));
    }

    public IEnumerator ClimbWallFall(bool isRopeClimbUp)
    {
 
        float FalltimeCheck = 0;
        float Falltime = 2f;
        float MoveY;
        UIManager.Instance.EndClimbWall(playerType);
        yield return new WaitForSeconds(0.3f);
        while(FalltimeCheck <= 2)
        {
            FalltimeCheck += Time.deltaTime / Falltime;
            MoveY = Mathf.Lerp(transform.position.y, groundPos.y, FalltimeCheck);
            MoveY = transform.position.y - MoveY;
            transform.position -= new Vector3(0, MoveY, 0);
            yield return null;
        }

        //플레이어 idle상태로 변경
        if(isRopeClimbUp)
        {
            yield return new WaitForSeconds(0.3f);
            ani.SetTrigger("ClimbupFallExit");
        }
    }

    #endregion
    void Climb(MoveType moveType)
    {
        if (moveType == MoveType.Front)
        {
            moveSpeed = 3;
            //moveDirection = -GetDirection(InputDir.front, PlayerState.ClimbWall);
            moveDirection = Vector3.up;
            ani.SetFloat("ClimbSpeed", 1f);

        }
        else if (moveType == MoveType.Back)
        {
            moveSpeed = 3;
            //moveDirection = -GetDirection(InputDir.back, PlayerState.ClimbWall);
            moveDirection = Vector3.down;
            ani.SetFloat("ClimbSpeed", -1f);
        }
        else
        {
            moveDirection = Vector3.zero;
            ani.SetFloat("ClimbSpeed", 0);
        }
    }

    void Rotation(MoveType moveType, PlayerState _playerState)
    {
        if (playerState != PlayerState.Walk)
            return;
        // ani.SetBool("Run", isRun);
        if (moveType == MoveType.Left)
        {
            transform.Rotate(new Vector3(0, -rotateSpeed * Time.deltaTime, 0));
        }
        else if (moveType == MoveType.Right)
        {
            transform.Rotate(new Vector3(0, rotateSpeed * Time.deltaTime, 0));
        }
    }

    void Run(bool _isRun)
    {
        if (playerState != PlayerState.Walk)
            return;
        isRun = _isRun;

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
    #endregion

    #region 기믹-로프
    //로프잡기 시작
    public void StartHoldRope(Vector3 _ropePos)
    {
        //로프를 바라보도록 회전
        float angle = Vector3.Angle(transform.position, _ropePos);
        transform.LookAt(_ropePos);
        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        ani.SetBool("HoldingRope", true);
        switch (playerType)
        {
            case PlayerType.FirstPlayer:
                InputManager.Instance.OnUsePlayer1 += AddHoldRopeValue;
                UIManager.Instance.StartHoldRope(playerType);
                break;
            case PlayerType.SecondPlayer:
                InputManager.Instance.OnUsePlayer1 += AddHoldRopeValue;
                UIManager.Instance.StartHoldRope(playerType);
                break;
        }    
    }

    public void EndHoldRope()
    {

        switch (playerType)
        {
            case PlayerType.FirstPlayer:
                InputManager.Instance.OnUsePlayer1 -= AddHoldRopeValue;
                UIManager.Instance.EndHoldRope(playerType);
                ani.SetTrigger("ClimbupFall");
                ani.SetTrigger("ClimbupFallExit");
                ani.SetBool("HoldingRope", false);
                PlayerStateChange(PlayerState.Walk);
                break;
            case PlayerType.SecondPlayer:
                InputManager.Instance.OnUsePlayer1 -= AddHoldRopeValue;
                UIManager.Instance.EndHoldRope(playerType);
                ani.SetTrigger("ClimbupFall");
                ani.SetTrigger("ClimbupFallExit");
                ani.SetBool("HoldingRope", false);
                PlayerStateChange(PlayerState.Walk);
                break;
        }
    }



    void AddHoldRopeValue(PlayerType _playertype, PlayerState _playerState)
    {
        switch(playerType)
        {
            case PlayerType.FirstPlayer:
                UIManager.Instance.AddHoldRopeValue(playerType);
                break;
            case PlayerType.SecondPlayer:
                UIManager.Instance.AddHoldRopeValue(playerType);
                break;
        }
    }
    //로프 잡다가 넘어지기
    public void HoldRopeFall()
    {
        ani.SetTrigger("ClimbupFall");
    }

    #endregion


    void Attack(PlayerState playerstate)
    {
        if (CanAttack == false)
            return;
        switch(playerstate)
        {
            case PlayerState.ClimbWall:
            case PlayerState.ClimbUpWall:
            case PlayerState.ClimbWallFall:
            case PlayerState.HoldRope:
            case PlayerState.SafeBox:
            case PlayerState.Die:
                return;
        }
        if(AttackComboCount<3)
        {
            PlayerStateChange(PlayerState.Attack);
            AttackComboCount++;
            ani.SetTrigger("Attack" + AttackComboCount.ToString());
            if(AttackComboCount <3)
            {
                StartCoroutine(ComboEndCheck(AttackComboCount));
                StartCoroutine(AniNextCombo());

            }
            else
                StartCoroutine(AttackCoolTime());
            CanAttack = false;
        }
    }

    //콤보를 이어가지 않아 끝났는지 확인하는 함수
    IEnumerator ComboEndCheck(int lastComboNumber)
    {
        yield return new WaitForSeconds(1f);
        if( lastComboNumber == AttackComboCount)
        {
            CanAttack = false;
            PlayerStateChange(PlayerState.Walk);
            yield return new WaitForSeconds(0.2f);
            CanAttack = true;
            AttackComboCount = 0;
        }
    }

    IEnumerator AniNextCombo()
    {
        yield return new WaitForSeconds(0.7f);
        CanAttack = true;
        PlayerStateChange(PlayerState.Walk);

    }

    IEnumerator AttackCoolTime()
    {
        CanAttack = false;
        yield return new WaitForSeconds(0.7f);
        PlayerStateChange(PlayerState.Walk);
        yield return new WaitForSeconds(0.3f);
        CanAttack = true;
        AttackComboCount = 0;
    }


    public void PlayerStateChange(PlayerState _playerState)
    {
        ani.SetBool("WalkFront", false);
        ani.SetBool("WalkBack", false);
        ani.SetBool("Run", false);
        ani.SetBool("IsClimbinUpWall", false);
        ani.SetFloat("ClimbSpeed", 0);
        ani.SetBool("HoldingRope", false);
        playerState = _playerState;
        switch (_playerState)
        {
            case PlayerState.Walk:
                moveSpeed = 5;
                break;
            case PlayerState.ClimbWall:
                ClimbWallStart();
                ani.SetTrigger("ClimbStart");
                moveDirection = Vector3.zero;
                break;
            case PlayerState.ClimbRope:
                ClimbWallStart();
                ani.SetTrigger("RopeClimbStart");
                moveDirection = Vector3.zero;
                break;
            case PlayerState.HoldRope:
                moveDirection = Vector3.zero;
                break;

        }

    }

    public void PlayerCameraOnOFF(bool _State)
    {
        playerCamera.SetActive(_State);
    }

    public void PlayerMeshRendererOnOFF(bool _State)
    {
        meshRenderer.enabled = _State;
    }


    #region Get
    public PlayerState GetPlayerState()
    {
        return playerState;
    }
    #endregion
}