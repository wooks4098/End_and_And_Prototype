using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectBManager : BaseSelectManager
{
    private void Start()
    {
        // �ɾ�� Tag�� � �ݰ����� �����Ѵ�.
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
            // ��ġ�ϴ��� �˻��� ����� availableList�� �߰��ȴ�. (true or false)
            availableList.Add(safeboxManager.IsMatch(i, safeboxManager.SafeboxB));
        }
    }

    protected override void InputMoveKey()
    {
        // ����
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveOnPrev(1);
        }
    
        // ������
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
    
            // ���� �ݰ��� �ε����� 1��ŭ ������Ų��.
            // �ݰ��� �ε����� ����� ���͸���� ������ �ִ�.
            safeboxManager.SafeboxB[currentIndex] += 1;

            // ������ ����� ��츦 ����� ����ó��
            if (safeboxManager.SafeboxB[currentIndex] > safeboxManager.Origin.Count - 1)
            {
                safeboxManager.SafeboxB[currentIndex] = 0;
            }

            // SetMaterial() �� ȣ��
            safeboxManager.SetMaterial();

            safeboxManager.CheckAllCorrect(safeboxManager.SafeboxB);
        }
    }
}