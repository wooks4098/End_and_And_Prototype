using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    // 식물 가시
    [SerializeField] GameObject goPlantThorn;
    // 부모
    [SerializeField] Transform goParent;


    private void Awake()
    {
        goParent = this.transform;

        // 자식이 있는지 없는지 판별.
        // 자식 중 <Tree> 컴포넌트가 있을 때만 자식에서 goPlantThorn을 가져온다.
        // true라고 해두면 비활성화된 객체도 찾을 수 있다.
        if (gameObject.GetComponentInChildren<Tree>(true) != null)
        {
            goPlantThorn = goParent.GetChild(0).gameObject;

            // 켜져있으면 끈다.
            if(goPlantThorn.activeSelf)
            {
                goPlantThorn.SetActive(false);
            }
        }
    }


    /// <summary>
    /// 현재 식물 가시 정보에 접근
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
