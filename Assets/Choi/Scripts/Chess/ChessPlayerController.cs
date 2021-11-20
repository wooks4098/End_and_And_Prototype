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


    // 입력 중인지 구분할 플래그
    [SerializeField] bool isMoving = false;

    // 움직이는 속도, 거리
    [SerializeField] float moveSpeed = 4.0f;
    //[SerializeField] float moveDistance = 5.0f;

    Vector3 v3CurrentPosition;

    int currentFloorIndex;
    [SerializeField] Floor currentFloor;
    //[SerializeField] Floor previousFloor;

    [SerializeField] Floor startingFloor;

    // 상수
    // 위-아래로 한 칸씩 더 둬서
    readonly int startingFloorIndex = 39;
    readonly int endingFloorIndex = -6;

    //==================================================

    // [Access] delegate [Type] [Function Name]([Parameters])
    //public delegate void EventHandler(int _index);
    // [Access] [Delegate Function Name] [Function Name]
    //public static event EventHandler OnWrongFloorEvent;

    public event Action<int> OnWrongFloorEvent;

    public event Action OnMoveToDirectionEvent;
    public event Action StopMoveToDirectionEvent;


    private void Awake()
    {
        playerHp = GetComponent<ChessPlayerHp>();
    }

    private void Start()
    {
        currentFloor = startingFloor.GetComponent<Floor>();
        currentFloorIndex = startingFloorIndex;
        //v3CurrentPosition = transform.position;
    }

    private void Update()
    {
        // isMoving == false 이면 (움직이는 중이 아니면)
        if (!isMoving)
        {
            // key 입력을 받는다
            InputDirectionKey();

            SelectFloor(currentFloorIndex);
            CheckCurrentFloor(currentFloorIndex);
        }
        // isMoving == true 이면 (움직이는 중이면)
        else if (isMoving)
        {

            // 계속 이동한다
            MoveToDirection();

            //ExitPreviousFloor(currentFloorIndex);
        }

        if(playerHp.GetPlayerHp() <= 0)
        {
            PlayerRespawn();
        }
    }

    private void PlayerRespawn()
    {
        currentFloor = startingFloor.GetComponent<Floor>();
        currentFloorIndex = startingFloorIndex;
        playerHp.ResetPlayerHp();

        isMoving = true;
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
            if (currentFloorIndex > 5)
            {
                isMoving = true;

                currentFloorIndex -= 6;
            }            
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            if(currentFloorIndex < 30 && currentFloorIndex != startingFloorIndex)
            {
                isMoving = true;

                currentFloorIndex += 6;
            }            
        }
        else if (Input.GetKeyDown(KeyCode.A) && currentFloorIndex != startingFloorIndex)
        {
            if (currentFloorIndex % 6 != 0) 
            {
                isMoving = true;

                currentFloorIndex -= 1;
            }
        }
        else if (Input.GetKeyDown(KeyCode.D) && currentFloorIndex != startingFloorIndex)
        {
            if ((currentFloorIndex % 6) < 5)
            {
                isMoving = true;

                currentFloorIndex += 1;
            }            
        }

    }

    void MoveToDirection()
    {
        OnMoveToDirectionEvent();

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
            isMoving = false;
            StopMoveToDirectionEvent();
        }
    }

    void SelectFloor(int _index)
    {
        if (_index > 35) return;

        currentFloor = chessManager.GetFloorObjects(_index).GetComponent<Floor>();
    }

    /// <summary>
    /// 현재 Floor가 올바른 Floor인지 체크
    /// </summary>
    private void CheckCurrentFloor(int _index)
    {
        if (currentFloorIndex > 35) return;

        // false이면...
        if (!chessManager.GetFloorChecking(_index))
        {
            OnWrongFloorEvent(_index);
        }
    }    
}
