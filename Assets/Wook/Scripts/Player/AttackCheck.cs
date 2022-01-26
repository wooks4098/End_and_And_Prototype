using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 공격시 충돌판정을 하여 적에게 데미지를 주는 클래스
/// </summary>
public class AttackCheck : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            //Enemy데미지 주기
            CreatureHp creatureHp = other.GetComponent<CreatureHp>();
            creatureHp.GetDamage(10f);
        }
    }
}
