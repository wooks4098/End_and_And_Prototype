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
    // 테스트가 끝나면 일부는 감출 것

    [SerializeField] ChessManager chessManager;
    ChessPlayerHp playerHp;
    [SerializeField] FadeInOut fader;

    Animator animator;


    // 입력 중인지 구분할 플래그
    [SerializeField] bool isMoving = false;
    public bool IsMoving { get { return isMoving; } }

    // 움직이는 속도, 거리
    private float moveSpeed = 4.0f;
    //[SerializeField] float moveDistance = 5.0f;

    private Vector3 v3CurrentPosition;

    private int currentFloorIndex;
    [SerializeField] private Floor currentFloor;
    //[SerializeField] Floor previousFloor;

    [SerializeField] private Floor startingFloor;
    [SerializeField] private Floor endingFloor;

    // 코루틴을 저장할 변수
    private Coroutine cRespawnCoroutine;

    // 상수
    // 위-아래로 한 칸씩 더 둬서
    readonly int startingFloorIndex = 39; // 시작지점 인덱스 
    readonly int endingFloorIndex = -6;   // 도착지점 인덱스

    // 페이드인/아웃 시간
    readonly float fadeInTime = 1f;
    readonly float fadeOutTime = 2f;

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
    }

    private void Start()
    {
        currentFloor = startingFloor.GetComponent<Floor>();
        currentFloorIndex = startingFloorIndex;
        //v3CurrentPosition = transform.position;
    }

    private void Update()
    {
        // 체력이 있을 때만 모든 움직임이 실행된다.
        if(playerHp.GetPlayerHp() > 0)
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
            isMoving = true;
            PlayerRespawn();
        }
    }

    private void PlayerRespawn()
    {
        if(cRespawnCoroutine != null)
        {
            return;
        }
        cRespawnCoroutine = StartCoroutine(PlayerRespawnCoroutine());
    }

    private IEnumerator PlayerRespawnCoroutine()
    {
        // 원래 있던 블록을 빠져나감 = 원래 있던 블록의 코루틴을 멈춤
        OnExitWrongFloorEvent(currentFloorIndex);

        // 페이드 인
        yield return fader.FadeInCoroutine(fadeInTime);

        // Move To Start Floor OnRespawn
        // 빠져 나간 후 인덱스 갱신
        currentFloorIndex = startingFloorIndex;
        currentFloor = startingFloor.GetComponent<Floor>();

        // StartingFloor 좌표로 이동
        transform.position = new Vector3(currentFloor.transform.position.x, this.transform.position.y, currentFloor.transform.position.z);

        // 플레이어 체력 회복 
        playerHp.ResetPlayerHp();

        // 페이드 아웃
        yield return fader.FadeOutCoroutine(fadeOutTime);

        // 코루틴 비우기
        cRespawnCoroutine = null;
    }

    public Floor GetCurrentFloor()
    {
        return currentFloor;
    }
    public int GetCurrentFloorIndex()
    {
        return currentFloorIndex;
    }

    private void InputDirectionKey()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (currentFloorIndex > 5 || currentFloorIndex == 0)
            {
                isMoving = true;
                animator.SetTrigger("MoveToDirection");

                currentFloorIndex -= 6;
            }            
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            if(currentFloorIndex < 30 && currentFloorIndex != startingFloorIndex)
            {
                isMoving = true;
                animator.SetTrigger("MoveToDirection");

                currentFloorIndex += 6;
            }            
        }
        else if (Input.GetKeyDown(KeyCode.A) && currentFloorIndex != startingFloorIndex)
        {
            if (currentFloorIndex % 6 != 0) 
            {
                isMoving = true;
                animator.SetTrigger("MoveToDirection");

                currentFloorIndex -= 1;
            }
        }
        else if (Input.GetKeyDown(KeyCode.D) && currentFloorIndex != startingFloorIndex)
        {
            if ((currentFloorIndex % 6) < 5)
            {
                isMoving = true;
                animator.SetTrigger("MoveToDirection");

                currentFloorIndex += 1;
            }            
        }
    }

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
            if(playerHp.GetPlayerHp() > 0)
            {
                isMoving = false;
            }
        }
    }

    void SelectFloor(int _index)
    {
        if (_index == startingFloorIndex)
        {
            currentFloor = startingFloor.GetComponent<Floor>();
        }
        else if (_index == endingFloorIndex)
        {
            currentFloor = endingFloor.GetComponent<Floor>();
        }
        else
        {
            currentFloor = chessManager.GetFloorObjects(_index).GetComponent<Floor>();
        }
    }

    /// <summary>
    /// 현재 Floor가 올바른 Floor인지 체크
    /// </summary>
    private void CheckCurrentFloor(int _index)
    {
        if (currentFloorIndex == startingFloorIndex)
        {
            Debug.Log("Start");
            return;
        }
        else if (currentFloorIndex == endingFloorIndex)
        {
            Debug.Log("End");
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
        Debug.Log("StopMoveToDirection");

        // Active Thorn
        OnEnterWrongFloorEvent(currentFloorIndex);

        CheckCurrentFloor(currentFloorIndex);
    }
}
