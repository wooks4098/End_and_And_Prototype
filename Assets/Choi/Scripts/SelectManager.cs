using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectManager : MonoBehaviour
{
    // index 0�� = �ݰ� A
    // index 1�� = �ݰ� B
    [SerializeField] int[] currentIndex;
    [SerializeField] GameObject[] currentPiece = null;

    [SerializeField] SafeboxManager safeboxManager;

    // safeboxA Ȥ�� B�� ���� ���� �����ؿ� �����̳�
    [SerializeField] List<int> safeboxList;
    // ������ �� �ִ����� üũ�ϴ� list
    // true = ���� ����, false = ���� �Ұ���  
    [SerializeField] List<bool> availableList; 


    private void Awake()
    {
        availableList = new List<bool>();
        currentIndex = new int[2];
        currentPiece = new GameObject[2];
    }

    private void Start()
    {
        currentIndex[0] = 0;
        currentIndex[1] = 0;

        // �ɾ�� Tag�� � �ݰ����� �����Ѵ�.
        if (gameObject.CompareTag("SafeBoxA"))
        {
            safeboxList = safeboxManager.SafeboxA;
            currentPiece[0] = transform.GetChild(currentIndex[0]).gameObject;
        }            
        else if (gameObject.CompareTag("SafeBoxB"))
        {
            safeboxList = safeboxManager.SafeboxB;
            currentPiece[1] = transform.GetChild(currentIndex[1]).gameObject;
        }
    }

    private void Update()
    {
        //CheckAvailable();

        SelectInputKey();
    }

    public void CheckAvailable()
    {
        availableList.Clear();

        for (int i = 0; i < safeboxManager.Origin.Count; i++)
        {
            // ��ġ�ϴ��� �˻��� ����� availableList�� �߰��ȴ�. (true or false)
            availableList.Add(safeboxManager.IsMatch(i, safeboxList));
        }
    }

    void SelectInputKey()
    {
        // ����
        if(Input.GetKeyDown(KeyCode.W))
        {            
        }
        else if(Input.GetKeyDown(KeyCode.UpArrow))
        {

        }

        // ����
        if(Input.GetKeyDown(KeyCode.A))
        {
            MoveOnPrev(0);
        }
        else if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveOnPrev(1);
        }

        // �Ʒ���
        if (Input.GetKeyDown(KeyCode.S))
        {
            
        }
        else if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            
        }

        // ������
        if (Input.GetKeyDown(KeyCode.D))
        {
            MoveOnNext(0);
        }
        else if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveOnNext(1);
        }
    }

    // index 0 = �ݰ� A / index 1 = �ݰ� B
    private void MoveOnPrev(int index)
    {
        if (currentPiece[index] == null) return;

        currentPiece[index].GetComponent<Outline>().enabled = false;
        currentIndex[index]--;
        if (currentIndex[index] < 0)
        {
            currentIndex[index] = 7;
        }

        currentPiece[index] = transform.GetChild(currentIndex[index]).gameObject;
        currentPiece[index].GetComponent<Outline>().enabled = true;
    }

    private void MoveOnNext(int index)
    {
        if (currentPiece[index] == null) return;

        currentPiece[index].GetComponent<Outline>().enabled = false;
        currentIndex[index]++;
        if (currentIndex[index] > 7)
        {
            currentIndex[index] = 0;
        }
        currentPiece[index] = transform.GetChild(currentIndex[index]).gameObject;
        currentPiece[index].GetComponent<Outline>().enabled = true;
    }
}