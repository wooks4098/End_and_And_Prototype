using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSelectManager : MonoBehaviour
{
    [SerializeField] protected int currentIndex;
    [SerializeField] protected GameObject currentPiece = null;

    [SerializeField] protected SafeboxManager safeboxManager;

    // ������ �� �ִ����� üũ�ϴ� list
    // true = ���� ����, false = ���� �Ұ���  
    [SerializeField] protected List<bool> availableList;


    protected void Awake()
    {
        availableList = new List<bool>();
    }

    private void Update()
    {
        //CheckAvailable();

        InputMoveKey();
        InputSelectKey();
    }

    public abstract void CheckAvailable();
    protected abstract void InputMoveKey();

    protected void MoveOnPrev(int index)
    {
        if (currentPiece == null) return;

        // ���� piece (= �ݰ� cube �� 1��)���� <Outline> ������Ʈ�� ������ ��Ȱ��ȭ
        currentPiece.GetComponent<Outline>().enabled = false;

        // ���� �ε����� ���ҽ�Ų��
        // �������� �̵� = �ε��� 1��ŭ ����
        currentIndex--;

        // ������ ����� ��츦 ����� ����ó��
        if (currentIndex < 0)
        {
            currentIndex = safeboxManager.Origin.Count - 1;
        }

        // ���� piece�� ������ �ε����� ������ ��ü�Ѵ�.
        currentPiece = transform.GetChild(currentIndex).gameObject;

        // ���ο� piece���� <Outline> ������Ʈ�� ������ Ȱ��ȭ
        currentPiece.GetComponent<Outline>().enabled = true;
    }

    protected void MoveOnNext(int index)
    {
        if (currentPiece == null) return;

        // ���� piece (= �ݰ� cube �� 1��)���� <Outline> ������Ʈ�� ������ ��Ȱ��ȭ
        currentPiece.GetComponent<Outline>().enabled = false;

        // ���� �ε����� ������Ų��
        // ���������� �̵� = �ε��� 1��ŭ ����
        currentIndex++;

        // ������ ����� ��츦 ����� ����ó��
        if (currentIndex > safeboxManager.Origin.Count - 1)
        {
            currentIndex = 0;
        }

        // ���� piece�� ������ �ε����� ������ ��ü�Ѵ�.
        currentPiece = transform.GetChild(currentIndex).gameObject;

        // ���ο� piece���� <Outline> ������Ʈ�� ������ Ȱ��ȭ
        currentPiece.GetComponent<Outline>().enabled = true;
    }

    protected abstract void InputSelectKey();

}
