using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 식물 가시의 행동을 다루는 클래스
/// </summary>
public class PlantThorn : MonoBehaviour
{
    // 플레이어와 충돌하면 사라지도록한다.
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            // 아래의 HidePlantThorn 메서드 참고
            Invoke("HidePlantThorn", 1f);
        }
    }

    private void HidePlantThorn()
    {
        this.gameObject.SetActive(false);
    }
}
