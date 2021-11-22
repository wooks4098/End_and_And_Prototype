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
    [SerializeField] FadeInOut fader;

    Animator animator;


    // �Է� ������ ������ �÷���
    [SerializeField] bool isMoving = false;
    public bool IsMoving { get { return isMoving; } }

    // �����̴� �ӵ�, �Ÿ�
    private float moveSpeed = 4.0f;
    //[SerializeField] float moveDistance = 5.0f;

    private Vector3 v3CurrentPosition;

    private int currentFloorIndex;
    [SerializeField] private Floor currentFloor;
    //[SerializeField] Floor previousFloor;

    [SerializeField] private Floor startingFloor;
    [SerializeField] private Floor endingFloor;

    // �ڷ�ƾ�� ������ ����
    private Coroutine cRespawnCoroutine;

    // ���
    // ��-�Ʒ��� �� ĭ�� �� �ּ�
    readonly int startingFloorIndex = 39; // �������� �ε��� 
    readonly int endingFloorIndex = -6;   // �������� �ε���

    // ���̵���/�ƿ� �ð�
    readonly float fadeInTime = 1f;
    readonly float fadeOutTime = 2f;

    //==================================================

    // �÷��̾ �߸��� ������ ����� �� ȣ���ϴ� �̺�Ʈ
    public event Action<int> OnEnterWrongFloorEvent; // �߸��� ���ǿ� ������ ��
    public event Action<int> OnStayWrongFloorEvent; // �߸��� ���ǿ� �ӹ����� ���� ��
    public event Action<int> OnExitWrongFloorEvent; // �߸��� ������ �������� ��



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
        // ü���� ���� ���� ��� �������� ����ȴ�.
        if(playerHp.GetPlayerHp() > 0)
        {
            // isMoving = false �̸� (�����̴� ���� �ƴϸ�)
            if (!isMoving)
            {
                // key �Է��� �޴´�
                InputDirectionKey();
            }
            // isMoving == true �̸� (�����̴� ���̸�)
            else if (isMoving)
            {
                // ��� �̵��Ѵ�
                MoveToDirection();
            }
        }

        // ü���� ���� ���� ������
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
        // ���� �ִ� ����� �������� = ���� �ִ� ����� �ڷ�ƾ�� ����
        OnExitWrongFloorEvent(currentFloorIndex);

        // ���̵� ��
        yield return fader.FadeInCoroutine(fadeInTime);

        // Move To Start Floor OnRespawn
        // ���� ���� �� �ε��� ����
        currentFloorIndex = startingFloorIndex;
        currentFloor = startingFloor.GetComponent<Floor>();

        // StartingFloor ��ǥ�� �̵�
        transform.position = new Vector3(currentFloor.transform.position.x, this.transform.position.y, currentFloor.transform.position.z);

        // �÷��̾� ü�� ȸ�� 
        playerHp.ResetPlayerHp();

        // ���̵� �ƿ�
        yield return fader.FadeOutCoroutine(fadeOutTime);

        // �ڷ�ƾ ����
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
    /// ���� Floor�� �ùٸ� Floor���� üũ
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

        // false�̸�...
        else if (!chessManager.GetFloorChecking(_index))
        {
            // Active Thorn Coroutine
            OnStayWrongFloorEvent(_index);
        }
    }

    /// <summary>
    /// �÷��̾ ������ �� ȣ��.
    /// �ִϸ��̼� �̺�Ʈ�� ȣ��ȴ�.
    /// </summary>
    public void OnMoveToDirection()
    {
        // Stop Active Thorn Coroutine
        OnExitWrongFloorEvent(currentFloorIndex);

        SelectFloor(currentFloorIndex);
    }
    /// <summary>
    /// �÷��̾ �������� ���� �� ȣ��.
    /// �ִϸ��̼� �̺�Ʈ�� ȣ��ȴ�.
    /// </summary>
    public void StopMoveToDirection()
    {
        Debug.Log("StopMoveToDirection");

        // Active Thorn
        OnEnterWrongFloorEvent(currentFloorIndex);

        CheckCurrentFloor(currentFloorIndex);
    }
}
