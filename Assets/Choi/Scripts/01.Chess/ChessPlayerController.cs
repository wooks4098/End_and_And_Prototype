using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 플레이어 움직임을 컨트롤!!
/// ... 만 하려고 했는데 ChessManager도 좀 섞여버린.
/// </summary>
public class ChessPlayerController : MonoBehaviour
{
    private ChessManager chessManager;
    private ChessPlayerHp playerHp;
    private FadeInOut fader;

    Animator animator;


    // 입력 중인지 구분할 플래그
    [SerializeField] bool isMoving = false;
    public bool IsMoving { get { return isMoving; } }

    // 움직이는 속도, 거리
    private float moveSpeed = 4.0f;
    //[SerializeField] float moveDistance = 5.0f;

    // 현재 체스(바닥)의 인덱스
    private int currentFloorIndex;
    [SerializeField] private Floor currentFloor;
    //[SerializeField] Floor previousFloor;

    [SerializeField] private Floor startFloor;
    [SerializeField] private Floor goalFloor;

    // 코루틴을 저장할 변수
    private Coroutine cRespawnCoroutine;

    // 상수
    // 위-아래로 한 칸씩 더 둬서
    readonly int startFloorIndex = 39; // 시작지점 인덱스 
    readonly int goalFloorIndex = -6;   // 도착지점 인덱스

    // 페이드인/아웃 시간
    readonly float fadeInTime = 1f;
    readonly float fadeOutTime = 2f;

    // MapTest01 용 변수
    // 체스에서 움직일 수 있는가?
    private bool canMoveOnChess;
    public bool CanMoveOnChess { get { return canMoveOnChess; } set { canMoveOnChess = value; } }

    //==================================================

    // 플레이어가 잘못된 발판을 밟았을 때 호출하는 이벤트
    public event Action<int> OnEnterWrongFloorEvent; // 잘못된 발판에 들어왔을 때
    public event Action<int> OnStayWrongFloorEvent; // 잘못된 발판에 머무르고 있을 때
    public event Action<int> OnExitWrongFloorEvent; // 잘못된 발판을 빠져나갈 때



    private void Awake()
    {
        playerHp = GetComponent<ChessPlayerHp>();
        animator = GetComponent<Animator>();
        fader = FindObjectOfType<FadeInOut>();
        chessManager = FindObjectOfType<ChessManager>();
    }

    private void Start()
    {
        currentFloor = startFloor.GetComponent<Floor>();
        currentFloorIndex = startFloorIndex;
        
        //v3CurrentPosition = transform.position;
    }

    private void Update()
    {
        // canMovingInChess가 true일 때만 모든 움직임이 가능
        if (canMoveOnChess)
        {
            // 체력이 있을 때만 모든 움직임이 실행된다.
            if (playerHp.GetPlayerHp() > 0)
            {
                // isMoving = false 이면 (움직이는 중이 아니면)
                if (!isMoving)
                {
                    // key 입력을 받는다
                    InputDirectionKey();
                }
                // isMoving == true 이면 (움직이는 중이면)
                else if (isMoving)
                {
                    // 계속 이동한다
                    MoveToDirection();
                }
            }

            // 체력이 없는 경우는 리스폰
            else if (playerHp.GetPlayerHp() <= 0)
            {
                // isMoving을 true로 전환하여 움직이지 못하게 함
                isMoving = true;
                // 리스폰
                PlayerRespawn();
            }

        }
        
    }

    private void PlayerRespawn()
    {
        // Coroutine cRespawnCoroutine이 비어있지 않으면 리턴
        // 코루틴이 여러번 시작되는 걸 막기 위함
        if (cRespawnCoroutine != null)
        {
            return;
        }
        cRespawnCoroutine = StartCoroutine(PlayerRespawnCoroutine());
    }

    /// <summary>
    /// 플레이어 리스폰 코루틴
    /// 아래 문장들이 순서대로 진행됨
    /// </summary>
    /// <returns></returns>
    private IEnumerator PlayerRespawnCoroutine()
    {
        // 원래 있던 블록을 빠져나감 = 원래 있던 블록의 코루틴을 멈춤
        OnExitWrongFloorEvent(currentFloorIndex);

        // 페이드 인 코루틴이 진행될 때까지 기다린다
        yield return fader.FadeInCoroutine(fadeInTime);

        // Move To Start Floor OnRespawn
        // 빠져 나간 후 인덱스 갱신
        currentFloorIndex = startFloorIndex;
        currentFloor = startFloor.GetComponent<Floor>();

        // StartingFloor 좌표로 이동
        transform.position = new Vector3(currentFloor.transform.position.x, this.transform.position.y, currentFloor.transform.position.z);

        // 플레이어 체력 회복 
        playerHp.ResetPlayerHp();

        // 페이드 아웃 코루틴이 진행될 때까지 기다린다
        yield return fader.FadeOutCoroutine(fadeOutTime);

        // 코루틴 비우기
        cRespawnCoroutine = null;
    }

    // 현재 체스판 바닥을 받아오는 메서드
    // 지금은 안 쓰는듯?
    public Floor GetCurrentFloor()
    {
        return currentFloor;
    }
    // 현재 체스판 바닥 인덱스를 받아오는 메서드
    public int GetCurrentFloorIndex()
    {
        return currentFloorIndex;
    }

    /// <summary>
    /// 방향키를 받는 메서드
    /// </summary>
    private void InputDirectionKey()
    {
        // 앞
        if (Input.GetKeyDown(KeyCode.W))
        {
            // (이하 모든 비교문이 비슷한 역할을 함)
            // 1. 현재 인덱스를 비교. 앞을 향하는 것이기 때문에 가장 윗줄 이동을 막음.
            // 하지만 0번 바닥일 때는 골인을 향해 가야하기 때문에? 이동을 막지 않음.
            if (currentFloorIndex > 5 || currentFloorIndex == 0)
            {
                // 입력을 받으면 isMoving = true로 설정하여 입력을 막음
                isMoving = true;
                // 애니메이션 이벤트를 실행하기 위한 애니메이션을 만들었고
                // 그 애니메이션을 실행하기 위한 이벤트 트리거
                animator.SetTrigger("MoveToDirection");

                // 인덱스를 변경한다
                // 인덱스가 감소하면 -> 앞쪽과 왼쪽,
                // 인덱스가 증가하면 -> 뒤쪽과 오른쪽
                currentFloorIndex -= 6;
            }            
        }
        // 뒤
        else if (Input.GetKeyDown(KeyCode.S))
        {
            if(currentFloorIndex < 30 && currentFloorIndex != startFloorIndex)
            {
                isMoving = true;
                animator.SetTrigger("MoveToDirection");

                currentFloorIndex += 6;
            }            
        }
        // 왼
        else if (Input.GetKeyDown(KeyCode.A) && currentFloorIndex != startFloorIndex)
        {
            if (currentFloorIndex % 6 != 0) 
            {
                isMoving = true;
                animator.SetTrigger("MoveToDirection");

                currentFloorIndex -= 1;
            }
        }
        // 오
        else if (Input.GetKeyDown(KeyCode.D) && currentFloorIndex != startFloorIndex)
        {
            if ((currentFloorIndex % 6) < 5)
            {
                isMoving = true;
                animator.SetTrigger("MoveToDirection");

                currentFloorIndex += 1;
            }            
        }
    }

    /// <summary>
    /// 입력 받은 방향으로 나아가도록 하는 메서드
    /// </summary>
    void MoveToDirection()
    {
        // 부드러운 이동을 위해 Mathf.MoveTowrads를 사용 
        // 이것은 지금은 앞과 뒤로 이동할 때 실행되므로 z값을 계산한다.
        // 큐브가 currentFloor (= 현재 지정된 바닥)을 타겟으로 이동한다. - 속도는 moveSpeed * time.deltaTime만큼
        float newPositonX = Mathf.MoveTowards(transform.position.x, currentFloor.transform.position.x, moveSpeed * Time.deltaTime);
        float newPositonZ = Mathf.MoveTowards(transform.position.z, currentFloor.transform.position.z, moveSpeed * Time.deltaTime);

        // Vector3를 사용하여 새로운 좌표로 업데이트.
        transform.position = new Vector3(newPositonX, transform.position.y, newPositonZ);

        //Debug.Log(Vector3.Distance(transform.position, currentFloor.transform.position));

        // 도착하면 플래그를 false로 변경
        if (Vector3.Distance(transform.position, currentFloor.transform.position) <= 1.0f)
        {
            // 플레이어 체력이 0보다 클 때만 움직임을 푼다.
            // 리스폰 시 움직임이 입력되는 문제 때문에 막아둠
            if(playerHp.GetPlayerHp() > 0)
            {
                isMoving = false;
            }
        }
    }

    /// <summary>
    /// currentFloor에 할당될 체스판 바닥을 선택하는 메서드
    /// </summary>
    /// <param name="_index">선택할 때 사용할 바닥의 index가 필요하다.</param>
    void SelectFloor(int _index)
    {
        // 시작지점일 때
        if (_index == startFloorIndex)
        {
            currentFloor = startFloor.GetComponent<Floor>();
        }
        // 골인지점일 때
        else if (_index == goalFloorIndex)
        {
            currentFloor = goalFloor.GetComponent<Floor>();
        }
        // 시작지점도, 골인지점도 아니면 받은 index정보를 기반으로 currentFloor를 선택한다.
        else
        {
            // 현재 바닥정보를 가져온다.
            currentFloor = chessManager.GetFloorObjects(_index).GetComponent<Floor>();
        }
    }

    /// <summary>
    /// 현재 Floor가 올바른 Floor인지 체크
    /// </summary>
    private void CheckCurrentFloor(int _index)
    {
        // 시작지점과 골인지점은 체스판 바닥 정보를 저장한 리스트에 포함되어있지 않기 때문에
        // 인덱스를 비교해 리스트 밖을 벗어난 값이면 반환한다.
        if (currentFloorIndex == startFloorIndex)
        {
            Debug.Log("Start");
            return;
        }
        else if (currentFloorIndex == goalFloorIndex)
        {
            Debug.Log("Goal");
            return;
        }

        // false이면...
        else if (!chessManager.GetFloorChecking(_index))
        {
            // Active Thorn Coroutine
            OnStayWrongFloorEvent(_index);
        }
    }

    /// <summary>
    /// 플레이어가 움직일 때 호출.
    /// 애니메이션 이벤트로 호출된다.
    /// </summary>
    public void OnMoveToDirection()
    {
        // Stop Active Thorn Coroutine
        OnExitWrongFloorEvent(currentFloorIndex);

        SelectFloor(currentFloorIndex);
    }
    /// <summary>
    /// 플레이어가 움직임을 멈출 때 호출.
    /// 애니메이션 이벤트로 호출된다.
    /// </summary>
    public void StopMoveToDirection()
    {
        //Debug.Log("StopMoveToDirection");

        // Active Thorn
        OnEnterWrongFloorEvent(currentFloorIndex);

        CheckCurrentFloor(currentFloorIndex);
    }
}
