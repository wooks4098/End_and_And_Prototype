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
    private ChessManager chessManager;
    private ChessPlayerHp playerHp;
    private FadeInOut fader;

    Animator animator;


    // �Է� ������ ������ �÷���
    [SerializeField] bool isMoving = false;
    public bool IsMoving { get { return isMoving; } }

    // �����̴� �ӵ�, �Ÿ�
    private float moveSpeed = 4.0f;
    //[SerializeField] float moveDistance = 5.0f;

    // ���� ü��(�ٴ�)�� �ε���
    private int currentFloorIndex;
    [SerializeField] private Floor currentFloor;
    //[SerializeField] Floor previousFloor;

    [SerializeField] private Floor startFloor;
    [SerializeField] private Floor goalFloor;

    // �ڷ�ƾ�� ������ ����
    private Coroutine cRespawnCoroutine;

    // ���
    // ��-�Ʒ��� �� ĭ�� �� �ּ�
    readonly int startFloorIndex = 39; // �������� �ε��� 
    readonly int goalFloorIndex = -6;   // �������� �ε���

    // ���̵���/�ƿ� �ð�
    readonly float fadeInTime = 1f;
    readonly float fadeOutTime = 2f;

    // MapTest01 �� ����
    // ü������ ������ �� �ִ°�?
    private bool canMoveOnChess;
    public bool CanMoveOnChess { get { return canMoveOnChess; } set { canMoveOnChess = value; } }

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
        // canMovingInChess�� true�� ���� ��� �������� ����
        if (canMoveOnChess)
        {
            // ü���� ���� ���� ��� �������� ����ȴ�.
            if (playerHp.GetPlayerHp() > 0)
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
                // isMoving�� true�� ��ȯ�Ͽ� �������� ���ϰ� ��
                isMoving = true;
                // ������
                PlayerRespawn();
            }

        }
        
    }

    private void PlayerRespawn()
    {
        // Coroutine cRespawnCoroutine�� ������� ������ ����
        // �ڷ�ƾ�� ������ ���۵Ǵ� �� ���� ����
        if (cRespawnCoroutine != null)
        {
            return;
        }
        cRespawnCoroutine = StartCoroutine(PlayerRespawnCoroutine());
    }

    /// <summary>
    /// �÷��̾� ������ �ڷ�ƾ
    /// �Ʒ� ������� ������� �����
    /// </summary>
    /// <returns></returns>
    private IEnumerator PlayerRespawnCoroutine()
    {
        // ���� �ִ� ����� �������� = ���� �ִ� ����� �ڷ�ƾ�� ����
        OnExitWrongFloorEvent(currentFloorIndex);

        // ���̵� �� �ڷ�ƾ�� ����� ������ ��ٸ���
        yield return fader.FadeInCoroutine(fadeInTime);

        // Move To Start Floor OnRespawn
        // ���� ���� �� �ε��� ����
        currentFloorIndex = startFloorIndex;
        currentFloor = startFloor.GetComponent<Floor>();

        // StartingFloor ��ǥ�� �̵�
        transform.position = new Vector3(currentFloor.transform.position.x, this.transform.position.y, currentFloor.transform.position.z);

        // �÷��̾� ü�� ȸ�� 
        playerHp.ResetPlayerHp();

        // ���̵� �ƿ� �ڷ�ƾ�� ����� ������ ��ٸ���
        yield return fader.FadeOutCoroutine(fadeOutTime);

        // �ڷ�ƾ ����
        cRespawnCoroutine = null;
    }

    // ���� ü���� �ٴ��� �޾ƿ��� �޼���
    // ������ �� ���µ�?
    public Floor GetCurrentFloor()
    {
        return currentFloor;
    }
    // ���� ü���� �ٴ� �ε����� �޾ƿ��� �޼���
    public int GetCurrentFloorIndex()
    {
        return currentFloorIndex;
    }

    /// <summary>
    /// ����Ű�� �޴� �޼���
    /// </summary>
    private void InputDirectionKey()
    {
        // ��
        if (Input.GetKeyDown(KeyCode.W))
        {
            // (���� ��� �񱳹��� ����� ������ ��)
            // 1. ���� �ε����� ��. ���� ���ϴ� ���̱� ������ ���� ���� �̵��� ����.
            // ������ 0�� �ٴ��� ���� ������ ���� �����ϱ� ������? �̵��� ���� ����.
            if (currentFloorIndex > 5 || currentFloorIndex == 0)
            {
                // �Է��� ������ isMoving = true�� �����Ͽ� �Է��� ����
                isMoving = true;
                // �ִϸ��̼� �̺�Ʈ�� �����ϱ� ���� �ִϸ��̼��� �������
                // �� �ִϸ��̼��� �����ϱ� ���� �̺�Ʈ Ʈ����
                animator.SetTrigger("MoveToDirection");

                // �ε����� �����Ѵ�
                // �ε����� �����ϸ� -> ���ʰ� ����,
                // �ε����� �����ϸ� -> ���ʰ� ������
                currentFloorIndex -= 6;
            }            
        }
        // ��
        else if (Input.GetKeyDown(KeyCode.S))
        {
            if(currentFloorIndex < 30 && currentFloorIndex != startFloorIndex)
            {
                isMoving = true;
                animator.SetTrigger("MoveToDirection");

                currentFloorIndex += 6;
            }            
        }
        // ��
        else if (Input.GetKeyDown(KeyCode.A) && currentFloorIndex != startFloorIndex)
        {
            if (currentFloorIndex % 6 != 0) 
            {
                isMoving = true;
                animator.SetTrigger("MoveToDirection");

                currentFloorIndex -= 1;
            }
        }
        // ��
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
    /// �Է� ���� �������� ���ư����� �ϴ� �޼���
    /// </summary>
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
            // �÷��̾� ü���� 0���� Ŭ ���� �������� Ǭ��.
            // ������ �� �������� �ԷµǴ� ���� ������ ���Ƶ�
            if(playerHp.GetPlayerHp() > 0)
            {
                isMoving = false;
            }
        }
    }

    /// <summary>
    /// currentFloor�� �Ҵ�� ü���� �ٴ��� �����ϴ� �޼���
    /// </summary>
    /// <param name="_index">������ �� ����� �ٴ��� index�� �ʿ��ϴ�.</param>
    void SelectFloor(int _index)
    {
        // ���������� ��
        if (_index == startFloorIndex)
        {
            currentFloor = startFloor.GetComponent<Floor>();
        }
        // ���������� ��
        else if (_index == goalFloorIndex)
        {
            currentFloor = goalFloor.GetComponent<Floor>();
        }
        // ����������, ���������� �ƴϸ� ���� index������ ������� currentFloor�� �����Ѵ�.
        else
        {
            // ���� �ٴ������� �����´�.
            currentFloor = chessManager.GetFloorObjects(_index).GetComponent<Floor>();
        }
    }

    /// <summary>
    /// ���� Floor�� �ùٸ� Floor���� üũ
    /// </summary>
    private void CheckCurrentFloor(int _index)
    {
        // ���������� ���������� ü���� �ٴ� ������ ������ ����Ʈ�� ���ԵǾ����� �ʱ� ������
        // �ε����� ���� ����Ʈ ���� ��� ���̸� ��ȯ�Ѵ�.
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
        //Debug.Log("StopMoveToDirection");

        // Active Thorn
        OnEnterWrongFloorEvent(currentFloorIndex);

        CheckCurrentFloor(currentFloorIndex);
    }
}
