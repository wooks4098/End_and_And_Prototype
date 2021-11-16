using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    // 식물 가시
    [SerializeField] GameObject goPlantThron;
    // 부모
    [SerializeField] Transform goParent;

    // 시간
    readonly float triggerTime = 7f;
    readonly float endTime = 5f;

    private void Awake()
    {
        goParent = this.transform;

        // 자식이 있는지 없는지 판별.
        // 자식 중 <Tree> 컴포넌트가 있을 때만 자식에서 goPlantThron을 가져온다.
        // true라고 해두면 비활성화된 객체도 찾을 수 있다.
        if (gameObject.GetComponentInChildren<Tree>(true) != null)
        {
            goPlantThron = goParent.GetChild(0).gameObject;


            // 켜져있으면 끈다.
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
