using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectBManager : BaseSelectManager
{
    private void Start()
    {
        // 걸어둔 Tag로 어떤 금고인지 구분한다.
        if (gameObject.CompareTag("SafeBoxB"))
        {
            //safeboxList = safeboxManager.SafeboxB;
            currentPiece = transform.GetChild(currentIndex).gameObject;
        }
    }

    

    public override void CheckAvailable()
    {
        availableList.Clear();

        for (int i = 0; i < safeboxManager.Origin.Count; i++)
        {
            // 일치하는지 검사한 결과가 availableList에 추가된다. (true or false)
            availableList.Add(safeboxManager.IsMatch(i, safeboxManager.SafeboxB));
        }
    }

    protected override void InputMoveKey()
    {
        // 왼쪽
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveOnPrev(1);
        }
    
        // 오른쪽
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveOnNext(1);
        }
    }


    protected override void InputSelectKey()
    {
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            //Debug.Log("safeboxManager.SafeboxB[currentIndex[1]]" + safeboxManager.SafeboxB[currentIndex].ToString());
    
            // 현재 금고의 인덱스를 1만큼 증가시킨다.
            // 금고의 인덱스는 출력할 머터리얼과 관련이 있다.
            safeboxManager.SafeboxB[currentIndex] += 1;

            // 범위를 벗어났을 경우를 대비한 예외처리
            if (safeboxManager.SafeboxB[currentIndex] > safeboxManager.Origin.Count - 1)
            {
                safeboxManager.SafeboxB[currentIndex] = 0;
            }

            // SetMaterial() 을 호출
            safeboxManager.SetMaterial();

            safeboxManager.CheckAllCorrect(safeboxManager.SafeboxB);
        }
    }
}