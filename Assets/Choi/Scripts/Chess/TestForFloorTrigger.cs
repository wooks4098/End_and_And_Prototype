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
    [SerializeField] ChessPlayerController chessPlayerController;

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
        chessPlayerController = FindObjectOfType<ChessPlayerController>();
    }

    private void Start()
    {
        if(goChessPlayer1.activeSelf)
        {
            goChessPlayer1.SetActive(false);
        }

        if(goArrowUI.activeSelf)
        {
            goArrowUI.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider _other)
    {
        // Player1 = 사람 캐릭터
        if(_other.CompareTag("Player1"))
        {
            if(OnEnterStart != null) OnEnterStart.Invoke();
        }        
        // Player = 체스 큐브 캐릭터
        else if(_other.CompareTag("Player"))
        {
            if (OnEnterGoal != null) OnEnterGoal.Invoke();
        }
    }

    public void OnChangeWhenEnterStart()
    {
        goArrowUI.gameObject.SetActive(true);

        cBasicCamera.gameObject.SetActive(false);
        cTopViewCamera.gameObject.SetActive(true);
        
        goPlayer1.SetActive(false);
        goChessPlayer1.SetActive(true);

        chessPlayerController.CanMovingInChess = true;
    }

    public void OnChangeWhenEnterGoal()
    {
        goArrowUI.gameObject.SetActive(false);

        cTopViewCamera.gameObject.SetActive(false);
        cBasicCamera.gameObject.SetActive(true);

        goPlayer1.SetActive(true);
        goChessPlayer1.SetActive(false);

        chessPlayerController.CanMovingInChess = false;
    }

}
