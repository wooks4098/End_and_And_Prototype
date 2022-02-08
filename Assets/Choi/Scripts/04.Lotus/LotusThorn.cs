using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 트랩가시의 행동을 다루는 클래스
/// </summary>
public class LotusThorn : MonoBehaviour
{
    // 플레이어와 충돌하면 사라지도록한다.
    private void OnTriggerEnter(Collider _other)
    {
        if (_other.CompareTag("Player1") || _other.CompareTag("Player2"))
        {
            // 아래의 HidePlantThorn 메서드 참고
            // 2초에 한 번씩 비활성화
            Invoke("HidePlantThorn", 2f);
        }
    }

    private void HidePlantThorn()
    {
        this.gameObject.SetActive(false);
    }
}
