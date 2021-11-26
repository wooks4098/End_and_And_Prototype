using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 테스트용이라 신경안쓰고 몰아넣음!!
/// 카메라 전환 등 종합적으로 실행함
/// </summary>
public class TestForFloorTrigger : MonoBehaviour
{
    private ChessPlayerController chessPlayerController;

    // 껐다 켰다 할 오브젝트들
    [SerializeField] Camera cBasicCamera; // 사람 캐릭터에게 달린 카메라
    [SerializeField] Camera cTopViewCamera; // 큐브 캐릭터를 바라볼 탑뷰 카메라
    [SerializeField] GameObject goPlayer1; // 사람 캐릭터
    [SerializeField] GameObject goChessPlayer1; // 체스에서 움직일 큐브 캐릭터
    [SerializeField] GameObject goArrowUI; 

    // 유니티 이벤트 - 컴포넌트에서 끼워넣음
    public UnityEvent OnEnterStart;
    public UnityEvent OnEnterGoal;


    private void Awake()
    {
        // Chess Player Controller를 찾는다
        // 큐브 캐릭터의 움직임을 제한하기 위해 - isMoveOnChess
        chessPlayerController = FindObjectOfType<ChessPlayerController>();
    }

    private void Start()
    {
        // 체스의 큐브 캐릭터가 켜져 있다면
        if(goChessPlayer1.activeSelf)
        {
            // 끈다 (시작할 때 사용 안 함)
            goChessPlayer1.SetActive(false);
        }

        // 화살표 UI도 켜져있다면
        if(goArrowUI.activeSelf)
        {
            // 끈다 (시작할 때 사용 안 함)
            goArrowUI.SetActive(false);
        }
    }

    /// <summary>
    /// 만약 오브젝트와 충돌했을 때...
    /// </summary>
    private void OnTriggerEnter(Collider _other)
    {
        // Player1과 충돌하면
        // *** Player1 = 사람 캐릭터
        if(_other.CompareTag("Player1"))
        {
            if(OnEnterStart != null) OnEnterStart.Invoke();
        }      
        // Player와 충돌하면
        // *** Player = 체스 큐브 캐릭터
        else if(_other.CompareTag("Player"))
        {
            if (OnEnterGoal != null) OnEnterGoal.Invoke();
        }
    }

    /// <summary>
    /// Start Point에 들어왔을 때 실행...
    /// </summary>
    public void OnChangeWhenEnterStart()
    {
        // 화살표 UI = 켬
        goArrowUI.gameObject.SetActive(true);

        // 일반 카메라 = 끔
        cBasicCamera.gameObject.SetActive(false);
        // 체스용 탑뷰 카메라 = 켬
        cTopViewCamera.gameObject.SetActive(true);
        
        // 사람 캐릭터 = 끔
        goPlayer1.SetActive(false);
        // 체스 큐브 캐릭터 = 켬
        goChessPlayer1.SetActive(true);

        // 체스판에서 큐브가 움직일 수 있게 true로 변경
        chessPlayerController.CanMoveOnChess = true;
    }

    /// <summary>
    /// Goal Point에 들어왔을 때 실행...
    /// </summary>
    public void OnChangeWhenEnterGoal()
    {
        // 화살표 UI = 끔
        goArrowUI.gameObject.SetActive(false);

        // 체스용 탑뷰 카메라 = 끔
        cTopViewCamera.gameObject.SetActive(false);
        // 일반 카메라 = 켬
        cBasicCamera.gameObject.SetActive(true);

        // 좌표 교체 후 (큐브의 x,z 값 + 사람 캐릭터의 y값)
        goPlayer1.transform.position = new Vector3(goChessPlayer1.transform.position.x, goPlayer1.transform.position.y, goChessPlayer1.transform.position.z);
        // 사람 캐릭터 = 켬
        goPlayer1.SetActive(true);
        // 체스 큐브 캐릭터 = 끔 켬
        goChessPlayer1.SetActive(false);

        // 체스판에서 큐브가 움직일 수 없도록 false로 변경
        chessPlayerController.CanMoveOnChess = false;
    }

}
