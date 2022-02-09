using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushObject : MonoBehaviour
{
    [SerializeField] ObjectUIShow Player1Collider;
    [SerializeField] ObjectUIShow Player2Collider;
    [SerializeField] bool isPushPlayer1;
    [SerializeField] bool isPushPlayer2;
    [SerializeField] bool isPushStartPlayer1;
    [SerializeField] bool isPushStartPlayer2;
    [SerializeField] bool EndPush = false;

    CharacterController character;
    GameObject gPlayer1;
    GameObject gPlayer2;
    Animator aniPlayer1;
    Animator aniPlayer2;

    Vector3 moveDirection; //이동방향
    [SerializeField] Transform MovePoint;//이동해야할 지점
    [SerializeField] float moveSpeed;//이동속도
    private void Start()
    {
        character = GetComponent<CharacterController>();
        moveDirection = MovePoint.position - transform.position;

        SetInput();
    }

    void SetInput()
    {
        InputManager.Instance.OnUsePlayer1 += UseObject;
        InputManager.Instance.OnUsePlayer2 += UseObject;
    }

    void UseObject(PlayerType _playerType, PlayerState _playerState)
    {
        switch (_playerType)
        {
            case PlayerType.FirstPlayer:
                if (Player1Collider.GetCanUse())
                {
                    isPushStartPlayer1 = true;
                    StartCoroutine(StartPushObject(PlayerType.FirstPlayer));
                }
                break;
            case PlayerType.SecondPlayer:
                if (Player2Collider.GetCanUse())
                {
                    isPushStartPlayer2 = true;
                    StartCoroutine(StartPushObject(PlayerType.SecondPlayer));
                }
                break;
        }
    }

    IEnumerator StartPushObject(PlayerType _playerType)
    {
        //플레이어 상태 변경
        GameManager.Instance.PlayerStateChange(_playerType, PlayerState.PushObject);

        //플레이어가 칼 들고있는지 확인
        if(GameManager.Instance.GetUseSword(_playerType) == true)
        {
            GameManager.Instance.GetPlayerController(_playerType).StartSheathSword();
            yield return new WaitForSeconds(0.8f);
        }

        switch (_playerType)
        {
            case PlayerType.FirstPlayer:
                //상호작용 UI숨기기
                Player1Collider.UIHide(_playerType);
                Player1Collider.HideObjectUIShow();
                //플레이어 오브젝트 저장
                gPlayer1 = GameManager.Instance.GetPlayerTrans(_playerType).gameObject;
                //플레이어 애니메이터 저장
                aniPlayer1 = GameManager.Instance.GetPlayerModelTrans(_playerType).GetComponent<Animator>();
                //플레이어 부모 미는 오브젝트로 변경
                gPlayer1.transform.parent = this.transform;
                //미는 애니 실행
                aniPlayer1.SetTrigger("StartPush");
                break;
            case PlayerType.SecondPlayer:
                //상호작용 UI숨기기
                Player2Collider.UIHide(_playerType);
                Player2Collider.HideObjectUIShow();
                //플레이어 오브젝트 저장
                gPlayer2 = GameManager.Instance.GetPlayerTrans(_playerType).gameObject;
                //플레이어 애니메이터 저장
                aniPlayer2 = GameManager.Instance.GetPlayerModelTrans(_playerType).GetComponent<Animator>();
                //플레이어 부모 미는 오브젝트로 변경
                gPlayer2.transform.parent = this.transform;
                //미는 애니 실행
                aniPlayer2.SetTrigger("StartPush");
                break;
        }
        if (isPushStartPlayer1 && isPushStartPlayer2)
        {
            //Player1
            InputManager.Instance.OnUsePlayer1 += PushCheckPlayer1;
            UIManager.Instance.StartClimbWall(PlayerType.FirstPlayer);
            //Player2
            InputManager.Instance.OnUsePlayer2 += PushCheckPlayer2;
            UIManager.Instance.StartClimbWall(PlayerType.SecondPlayer);
        }

    }

    void PushCheckPlayer1(PlayerType _playerType, PlayerState _playerState)
    {
        if (_playerState != PlayerState.PushObject)
            return;
        if (UIManager.Instance.isSliderTriggerCheck(_playerType))
        {
            //해당 플레이어 밀기 성공
            isPushPlayer1 = true;
            ObjectMoveCheck();
        }

    }

    void PushCheckPlayer2(PlayerType _playerType, PlayerState _playerState)
    {
        if (_playerState != PlayerState.PushObject)
            return;
        if (UIManager.Instance.isSliderTriggerCheck(_playerType))
        {
            //해당 플레이어 밀기 성공
            isPushPlayer2 = true;
            ObjectMoveCheck();
        }
    }

    //오브젝트를 밀 수 있는지 확인하고 미는 함수
    void ObjectMoveCheck()
    {

        if (isPushPlayer1 && isPushPlayer2)
        {
            StartCoroutine(ObjectMove());
            isPushPlayer1 = false;
            isPushPlayer2 = false;
        }
    }

    IEnumerator ObjectMove()
    {
        float timeCheck = 0;

        while(timeCheck<1f)
        {
            if (EndPush)
                break;
            timeCheck += Time.deltaTime;
            character.Move(moveDirection * moveSpeed * Time.deltaTime);
            aniPlayer1.SetFloat("PushSpeed", 0.4f);
            aniPlayer2.SetFloat("PushSpeed", 0.4f);
            yield return null;
        }
        aniPlayer1.SetFloat("PushSpeed", 0f);
        aniPlayer2.SetFloat("PushSpeed", 0f);
    }

    void EndPushObject()
    {
        aniPlayer1.SetFloat("PushSpeed", 0f);
        aniPlayer2.SetFloat("PushSpeed", 0f);
        //애니메이션 변경
        aniPlayer1.SetTrigger("EndPush");
        aniPlayer2.SetTrigger("EndPush");
        //플레이어 부모변경
        gPlayer1.transform.parent = null;
        gPlayer2.transform.parent = null;
        //플레이어 상태 변경
        GameManager.Instance.PlayerStateChange(PlayerType.FirstPlayer, PlayerState.Walk);
        GameManager.Instance.PlayerStateChange(PlayerType.SecondPlayer, PlayerState.Walk);
        EndPush = true;
        UIManager.Instance.EndClimbWall(PlayerType.FirstPlayer);
        UIManager.Instance.EndClimbWall(PlayerType.SecondPlayer);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "PushObjectEndPos")
        {
            EndPushObject();
        }
    }

}


#region 동시에 앞으로 가는 키를 눌러 이동시키는 코드

//void StartPushObject(PlayerType _playerType)
//{
//    //플레이어 상태 변경
//    GameManager.Instance.PlayerStateChange(_playerType, PlayerState.PushObject);

//    switch (_playerType)
//    {
//        case PlayerType.FirstPlayer:
//            //상호작용 UI숨기기
//            Player1Collider.UIHide(_playerType);
//            Player1Collider.HideObjectUIShow();
//            //플레이어 오브젝트 저장
//            gPlayer1 = GameManager.Instance.GetPlayerTrans(_playerType).gameObject;
//            //플레이어 애니메이터 저장
//            aniPlayer1 = GameManager.Instance.GetPlayerModelTrans(_playerType).GetComponent<Animator>();
//            //플레이어 부모 미는 오브젝트로 변경
//            gPlayer1.transform.parent = this.transform;
//            //미는 애니 실행
//            aniPlayer1.SetTrigger("StartPush");
//            break;
//        case PlayerType.SecondPlayer:
//            //상호작용 UI숨기기
//            Player2Collider.UIHide(_playerType);
//            Player2Collider.HideObjectUIShow();
//            //플레이어 오브젝트 저장
//            gPlayer2 = GameManager.Instance.GetPlayerTrans(_playerType).gameObject;
//            //플레이어 애니메이터 저장
//            aniPlayer2 = GameManager.Instance.GetPlayerModelTrans(_playerType).GetComponent<Animator>();
//            //플레이어 부모 미는 오브젝트로 변경
//            gPlayer2.transform.parent = this.transform;
//            //미는 애니 실행
//            aniPlayer2.SetTrigger("StartPush");
//            break;
//    }
//    if (isPushStartPlayer1 && isPushStartPlayer2)
//        StartCoroutine(Move());
//}

//IEnumerator Move()
//{
//    while(EndPush != true)
//    {
//        if(isPushPlayer1 && isPushPlayer2)
//        {
//            character.Move(moveDirection * moveSpeed * Time.deltaTime);
//            aniPlayer1.SetFloat("PushSpeed", 1f);
//            aniPlayer2.SetFloat("PushSpeed", 1f);
//        }
//        yield return null;
//    }
//    aniPlayer1.SetFloat("PushSpeed", 0f);
//    aniPlayer2.SetFloat("PushSpeed", 0f);
//}

//void MovePlayer1(MoveType moveType, PlayerState _playerState)
//{
//    if (_playerState != PlayerState.PushObject)
//    {
//        isPushPlayer1 = false;
//        return;
//    }
//    if(moveType == MoveType.Front)
//        isPushPlayer1 = true;
//    else
//    {
//        aniPlayer1.SetFloat("PushSpeed", 0f);
//        isPushPlayer1 = false;
//    }

//}
//void MovePlayer2(MoveType moveType, PlayerState _playerState)
//{
//    if (_playerState != PlayerState.PushObject)
//    {
//        isPushPlayer2 = false;
//        return;
//    }
//    if (moveType == MoveType.Front)
//        isPushPlayer2 = true;
//    else
//    {
//        aniPlayer2.SetFloat("PushSpeed", 0f);
//        isPushPlayer2 = false;
//    }
//}
#endregion