using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectAManager : BaseSelectManager
{
    public override void CheckAvailable()
    {
        availableList.Clear();

        for (int i = 0; i < safeboxManager.Origin.Count; i++)
        {
            // 일치하는지 검사한 결과가 availableList에 추가된다. (true or false)
            availableList.Add(safeboxManager.IsMatch(i, safeboxManager.SafeboxA));
        }
    }

    protected override void InputActiveKey()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            isActive = true;
        }
    }

    protected override void InputMoveKey()
    {
        // 왼쪽
        if(Input.GetKeyDown(KeyCode.A))
        {
            MoveOnPrev();
        }
    
        // 오른쪽
        if (Input.GetKeyDown(KeyCode.D))
        {
            MoveOnNext();
        }
    }


    protected override void InputSelectKey()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            // 현재 금고의 인덱스를 1만큼 증가시킨다.
            // 금고의 인덱스는 출력할 머터리얼과 관련이 있다.
            safeboxManager.SafeboxA[currentIndex] += 1;

            // 범위를 벗어났을 경우를 대비한 예외처리
            if (safeboxManager.SafeboxA[currentIndex] > safeboxManager.Origin.Count - 1)
            {
                safeboxManager.SafeboxA[currentIndex] = 0;
            }
            // SetMaterial() 을 호출
            safeboxManager.SetMaterial();

            safeboxManager.CheckAllCorrect(safeboxManager.SafeboxA);
        }
    }

    
}