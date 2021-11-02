using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectManager : MonoBehaviour
{
    // index 0번 = 금고 A
    // index 1번 = 금고 B
    [SerializeField] int[] currentIndex;
    [SerializeField] GameObject[] currentPiece = null;

    [SerializeField] SafeboxManager safeboxManager;

    // safeboxA 혹은 B의 값을 값을 복사해올 컨테이너
    [SerializeField] List<int> safeboxList;
    // 접근할 수 있는지를 체크하는 list
    // true = 접근 가능, false = 접근 불가능  
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

        // 걸어둔 Tag로 어떤 금고인지 구분한다.
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
            // 일치하는지 검사한 결과가 availableList에 추가된다. (true or false)
            availableList.Add(safeboxManager.IsMatch(i, safeboxList));
        }
    }

    void SelectInputKey()
    {
        // 위쪽
        if(Input.GetKeyDown(KeyCode.W))
        {            
        }
        else if(Input.GetKeyDown(KeyCode.UpArrow))
        {

        }

        // 왼쪽
        if(Input.GetKeyDown(KeyCode.A))
        {
            MoveOnPrev(0);
        }
        else if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveOnPrev(1);
        }

        // 아래쪽
        if (Input.GetKeyDown(KeyCode.S))
        {
            
        }
        else if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            
        }

        // 오른쪽
        if (Input.GetKeyDown(KeyCode.D))
        {
            MoveOnNext(0);
        }
        else if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveOnNext(1);
        }
    }

    // index 0 = 금고 A / index 1 = 금고 B
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