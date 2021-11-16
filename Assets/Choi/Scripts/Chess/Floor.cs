using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    // �Ĺ� ����
    [SerializeField] GameObject goPlantThron;
    // �θ�
    [SerializeField] Transform goParent;

    // �ð�
    readonly float triggerTime = 7f;
    readonly float endTime = 5f;

    private void Awake()
    {
        goParent = this.transform;

        // �ڽ��� �ִ��� ������ �Ǻ�.
        // �ڽ� �� <Tree> ������Ʈ�� ���� ���� �ڽĿ��� goPlantThron�� �����´�.
        // true��� �صθ� ��Ȱ��ȭ�� ��ü�� ã�� �� �ִ�.
        if (gameObject.GetComponentInChildren<Tree>(true) != null)
        {
            goPlantThron = goParent.GetChild(0).gameObject;


            // ���������� ����.
            if(goPlantThron.activeSelf)
            {
                goPlantThron.SetActive(false);
            }
        }
    }



    public GameObject GetPlantThron()
    {
        return goPlantThron;
    }
}
