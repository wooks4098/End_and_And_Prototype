using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    // �Ĺ� ����
    [SerializeField] GameObject goPlantThorn;
    // �θ�
    [SerializeField] Transform goParent;


    private void Awake()
    {
        goParent = this.transform;

        // �ڽ��� �ִ��� ������ �Ǻ�.
        // �ڽ� �� <Tree> ������Ʈ�� ���� ���� �ڽĿ��� goPlantThorn�� �����´�.
        // true��� �صθ� ��Ȱ��ȭ�� ��ü�� ã�� �� �ִ�.
        if (gameObject.GetComponentInChildren<Tree>(true) != null)
        {
            goPlantThorn = goParent.GetChild(0).gameObject;

            // ���������� ����.
            if(goPlantThorn.activeSelf)
            {
                goPlantThorn.SetActive(false);
            }
        }
    }


    /// <summary>
    /// ���� �Ĺ� ���� ������ ����
    /// </summary>
    /// <returns></returns>
    public GameObject GetPlantThorn()
    {
        if (gameObject.GetComponentInChildren<Tree>(true) == null)
        {
            return null;
        }

        return goPlantThorn;
    }
}
