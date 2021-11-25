using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSelectManager : MonoBehaviour
{
    [SerializeField] protected int currentIndex;
    [SerializeField] protected GameObject currentPiece = null;

    [SerializeField] protected SafeboxManager safeboxManager;

    // ������ �� �ִ����� üũ�ϴ� list
    // true = ���� ����][false = ���� �Ұ���  
    [SerializeField] protected List<bool> availableList;


    // ���� ī�޶�
    [SerializeField] Camera zoomInCamera;
    // Ȱ��ȭ üũ
    [SerializeField] protected bool isActive = false;
    [SerializeField] protected ObjectUIShow objectUiShow;

    protected void Awake()
    {
        availableList = new List<bool>();
    }

    protected void Start()
    {
        // ���� ���õ� ���� �Ҵ�
        currentPiece = transform.GetChild(currentIndex).gameObject;

        // ��ó�� 1.Ȱ��ȭ 2.�ε����Ҵ�
        CheckAvailable();
        SetStartIndex();

        // ���� ī�޶� ����
        if (zoomInCamera.gameObject.activeSelf == true)
        {
            zoomInCamera.gameObject.SetActive(false);
        }

        //inputManger ���
        SetInputKey();
    }

    private void Update()
    {
        // isActive == false�̸� 
        //if (!isActive)
        //{
        //    // Ȱ��ȭŰ (�ݰ� ���� �� Ȯ��Ű) �Է��� �޴� �޼���
        //    //InputActiveKey();

        //    if (zoomInCamera.gameObject.activeSelf == true)
        //    {
        //        zoomInCamera.gameObject.SetActive(false);
        //    }                
        //    // �Է� ���� �ʰ� ��������
        //    return;
        //}
        //else if (isActive)
        //{
        //    if (zoomInCamera.gameObject.activeSelf == false)
        //    {
        //        zoomInCamera.gameObject.SetActive(true);
        //    }
        //}

        //InputMoveKey();
        //InputSelectKey();        
    }

    protected void OpenSafeBox()
    {
        if (zoomInCamera.gameObject.activeSelf == false)
        {
            zoomInCamera.gameObject.SetActive(true);
        }
    }

    protected void CloseSafeBox()
    {
        if (zoomInCamera.gameObject.activeSelf == true)
        {
            zoomInCamera.gameObject.SetActive(false);
        }
    }

    public void SetStartIndex()
    {
        MoveOnNext();
    }

    public abstract void CheckAvailable();
    protected abstract void InputMoveKey(MoveType _moveType, PlayerState _playerState);

    protected void MoveOnPrev()
    {
        if (currentPiece == null) return;

        // ���� piece (= �ݰ� cube �� 1��)���� <Outline> ������Ʈ�� ������ ��Ȱ��ȭ
        currentPiece.GetComponent<Outline>().enabled = false;

        // ���� �ε����� ���ҽ�Ų��
        // �������� �̵� = �ε��� 1��ŭ ����
        //currentIndex--;
        CalculateMoveOnPrev();


        // ���� piece�� ������ �ε����� ������ ��ü�Ѵ�.
        currentPiece = transform.GetChild(currentIndex).gameObject;

        // ���ο� piece���� <Outline> ������Ʈ�� ������ Ȱ��ȭ
        currentPiece.GetComponent<Outline>().enabled = true;
    }

    protected void MoveOnNext()
    {
        if (currentPiece == null) return;

        // ���� piece (= �ݰ� cube �� 1��)���� <Outline> ������Ʈ�� ������ ��Ȱ��ȭ
        currentPiece.GetComponent<Outline>().enabled = false;

        // ���� �ε����� ������Ų��
        // ���������� �̵� = �ε��� 1��ŭ ����
        //currentIndex++;
        CalculateMoveOnNext();


        // ���� piece�� ������ �ε����� ������ ��ü�Ѵ�.
        currentPiece = transform.GetChild(currentIndex).gameObject;

        // ���ο� piece���� <Outline> ������Ʈ�� ������ Ȱ��ȭ
        currentPiece.GetComponent<Outline>().enabled = true;
    }

    protected abstract void InputSelectKey(PlayerType _playerType, PlayerState _playerState);

    protected void CalculateMoveOnPrev()
    {
        currentIndex = currentIndex - 1;

        // ������ ����� ��츦 ����� ����ó��
        if (currentIndex < 0)
        {
            currentIndex = safeboxManager.Origin.Count - 1;
        }

        // availableList == false�̸� ������ �� ������
        if (availableList[currentIndex] == false)
        {
            // ��͸� ���� �ε����� �ٽ� �� �� ���ҽ�Ų��.
            CalculateMoveOnPrev();
        }
    }

    protected void CalculateMoveOnNext()
    {
        currentIndex = currentIndex + 1;

        // ������ ����� ��츦 ����� ����ó��
        if (currentIndex > safeboxManager.Origin.Count - 1)
        {
            currentIndex = 0;
        }

        // availableList == false�̸� ������ �� ������
        if (availableList[currentIndex] == false)
        {
            // ��͸� ���� �ε����� �ٽ� �� �� ������Ų��.
            CalculateMoveOnNext();
        }
    }


    protected abstract void InputActiveKey(PlayerType _playerType, PlayerState _playerState);

    //inputManager�� ���
    protected abstract void SetInputKey();
}
