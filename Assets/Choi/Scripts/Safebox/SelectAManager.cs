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
            // ��ġ�ϴ��� �˻��� ����� availableList�� �߰��ȴ�. (true or false)
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
        // ����
        if(Input.GetKeyDown(KeyCode.A))
        {
            MoveOnPrev();
        }
    
        // ������
        if (Input.GetKeyDown(KeyCode.D))
        {
            MoveOnNext();
        }
    }


    protected override void InputSelectKey()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            // ���� �ݰ��� �ε����� 1��ŭ ������Ų��.
            // �ݰ��� �ε����� ����� ���͸���� ������ �ִ�.
            safeboxManager.SafeboxA[currentIndex] += 1;

            // ������ ����� ��츦 ����� ����ó��
            if (safeboxManager.SafeboxA[currentIndex] > safeboxManager.Origin.Count - 1)
            {
                safeboxManager.SafeboxA[currentIndex] = 0;
            }
            // SetMaterial() �� ȣ��
            safeboxManager.SetMaterial();

            safeboxManager.CheckAllCorrect(safeboxManager.SafeboxA);
        }
    }

    
}