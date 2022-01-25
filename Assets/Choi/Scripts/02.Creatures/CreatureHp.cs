using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureHp : MonoBehaviour
{
    public int maxHp = 100;
    public int currentHp = 20;


    public void ResetHp()
    {
        currentHp = maxHp;
    }

    // 데미지 받는
    public void GetDamage(float _value)
    {

    }
}
