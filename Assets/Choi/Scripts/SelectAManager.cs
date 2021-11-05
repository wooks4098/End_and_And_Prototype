using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectAManager : BaseSelectManager
{
    private void Start()
    {
        // �ɾ�� Tag�� � �ݰ����� �����Ѵ�.
        if (gameObject.CompareTag("SafeBoxA"))
        {
            //safeboxList = safeboxManager.SafeboxA;
            currentPiece = transform.GetChild(currentIndex).gameObject;
        }
    }


    public override void CheckAvailable()
    {
        availableList.Clear();

        for (int i = 0; i < safeboxManager.Origin.Count; i++)
        {
            // ��ġ�ϴ��� �˻��� ����� availableList�� �߰��ȴ�. (true or false)
            availableList.Add(safeboxManager.IsMatch(i, safeboxManager.SafeboxA));
        }
    }

    protected override void InputMoveKey()
    {
        // ����
        if(Input.GetKeyDown(KeyCode.A))
        {
            MoveOnPrev(0);
        }
    
        // ������
        if (Input.GetKeyDown(KeyCode.D))
        {
            MoveOnNext(0);
        }
    }


    protected override void InputSelectKey()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            //Debug.Log("safeboxManager.SafeboxB[currentIndex[1]]" + safeboxManager.SafeboxA[currentIndex].ToString());

            safeboxManager.SafeboxA[currentIndex] += 1;

            if (safeboxManager.SafeboxA[currentIndex] > safeboxManager.Origin.Count - 1)
            {
                safeboxManager.SafeboxA[currentIndex] = 0;
            }
            safeboxManager.SetMaterial();
        }
    }
}