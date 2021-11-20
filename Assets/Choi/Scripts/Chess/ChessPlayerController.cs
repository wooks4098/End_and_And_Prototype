using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// �÷��̾� �������� ��Ʈ��!!
/// ... �� �Ϸ��� �ߴµ� ChessManager�� �� ��������.
/// </summary>
public class ChessPlayerController : MonoBehaviour
{
    // �׽�Ʈ�� ������ �Ϻδ� ���� ��

    [SerializeField] ChessManager chessManager;
    ChessPlayerHp playerHp;


    // �Է� ������ ������ �÷���
    [SerializeField] bool isMoving = false;

    // �����̴� �ӵ�, �Ÿ�
    [SerializeField] float moveSpeed = 4.0f;
    //[SerializeField] float moveDistance = 5.0f;

    Vector3 v3CurrentPosition;

    int currentFloorIndex;
    [SerializeField] Floor currentFloor;
    //[SerializeField] Floor previousFloor;

    [SerializeField] Floor startingFloor;

    // ���
    // ��-�Ʒ��� �� ĭ�� �� �ּ�
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
        // isMoving == false �̸� (�����̴� ���� �ƴϸ�)
        if (!isMoving)
        {
            // key �Է��� �޴´�
            InputDirectionKey();

            SelectFloor(currentFloorIndex);
            CheckCurrentFloor(currentFloorIndex);
        }
        // isMoving == true �̸� (�����̴� ���̸�)
        else if (isMoving)
        {

            // ��� �̵��Ѵ�
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

        // �ε巯�� �̵��� ���� Mathf.MoveTowrads�� ��� 
        // �̰��� ������ �հ� �ڷ� �̵��� �� ����ǹǷ� z���� ����Ѵ�.
        // ť�갡 currentFloor (= ���� ������ �ٴ�)�� Ÿ������ �̵��Ѵ�. - �ӵ��� moveSpeed * time.deltaTime��ŭ
        float newPositonX = Mathf.MoveTowards(transform.position.x, currentFloor.transform.position.x, moveSpeed * Time.deltaTime);
        float newPositonZ = Mathf.MoveTowards(transform.position.z, currentFloor.transform.position.z, moveSpeed * Time.deltaTime);

        // Vector3�� ����Ͽ� ���ο� ��ǥ�� ������Ʈ.
        transform.position = new Vector3(newPositonX, transform.position.y, newPositonZ);

        //Debug.Log(Vector3.Distance(transform.position, currentFloor.transform.position));

        // �����ϸ� �÷��׸� false�� ����
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
    /// ���� Floor�� �ùٸ� Floor���� üũ
    /// </summary>
    private void CheckCurrentFloor(int _index)
    {
        if (currentFloorIndex > 35) return;

        // false�̸�...
        if (!chessManager.GetFloorChecking(_index))
        {
            OnWrongFloorEvent(_index);
        }
    }    
}
