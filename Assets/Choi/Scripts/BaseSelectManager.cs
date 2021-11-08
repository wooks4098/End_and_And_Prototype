using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSelectManager : MonoBehaviour
{
    [SerializeField] protected int currentIndex;
    [SerializeField] protected GameObject currentPiece = null;

    [SerializeField] protected SafeboxManager safeboxManager;

    // 접근할 수 있는지를 체크하는 list
    // true = 접근 가능, false = 접근 불가능  
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

    public void SetStartIndex()
    {
        MoveOnNext();
    }

    public abstract void CheckAvailable();
    protected abstract void InputMoveKey();

    protected void MoveOnPrev()
    {
        if (currentPiece == null) return;

        // 현재 piece (= 금고 cube 중 1개)에서 <Outline> 컴포넌트를 가져와 비활성화
        currentPiece.GetComponent<Outline>().enabled = false;

        // 현재 인덱스를 감소시킨다
        // 왼쪽으로 이동 = 인덱스 1만큼 감소
        //currentIndex--;
        CalculateMoveOnPrev();


        // 현재 piece를 변경한 인덱스의 것으로 교체한다.
        currentPiece = transform.GetChild(currentIndex).gameObject;

        // 새로운 piece에서 <Outline> 컴포넌트를 가져와 활성화
        currentPiece.GetComponent<Outline>().enabled = true;
    }

    protected void MoveOnNext()
    {
        if (currentPiece == null) return;

        // 현재 piece (= 금고 cube 중 1개)에서 <Outline> 컴포넌트를 가져와 비활성화
        currentPiece.GetComponent<Outline>().enabled = false;

        // 현재 인덱스를 증가시킨다
        // 오른쪽으로 이동 = 인덱스 1만큼 증가
        //currentIndex++;
        CalculateMoveOnNext();


        // 현재 piece를 변경한 인덱스의 것으로 교체한다.
        currentPiece = transform.GetChild(currentIndex).gameObject;

        // 새로운 piece에서 <Outline> 컴포넌트를 가져와 활성화
        currentPiece.GetComponent<Outline>().enabled = true;
    }

    protected abstract void InputSelectKey();

    protected void CalculateMoveOnPrev()
    {
        currentIndex = currentIndex - 1;

        // 범위를 벗어났을 경우를 대비한 예외처리
        if (currentIndex < 0)
        {
            currentIndex = safeboxManager.Origin.Count - 1;
        }

        // availableList == false이면 지나갈 수 없으니
        if (availableList[currentIndex] == false)
        {
            // 재귀를 통해 인덱스를 다시 한 번 감소시킨다.
            CalculateMoveOnPrev();
        }
    }

    protected void CalculateMoveOnNext()
    {
        currentIndex = currentIndex + 1;

        // 범위를 벗어났을 경우를 대비한 예외처리
        if (currentIndex > safeboxManager.Origin.Count - 1)
        {
            currentIndex = 0;
        }

        // availableList == false이면 지나갈 수 없으니
        if (availableList[currentIndex] == false)
        {
            // 재귀를 통해 인덱스를 다시 한 번 증가시킨다.
            CalculateMoveOnNext();
        }
    }   
     
}
