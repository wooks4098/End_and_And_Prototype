using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureHp : MonoBehaviour
{
    public float maxHp = 100f;
    public float currentHp = 100f;


    private void OnEnable()
    {
        currentHp = maxHp;
    }

    public void ResetHp()
    {
        currentHp = maxHp;
    }

    // 데미지 받는
    public float GetDamage(float _value)
    {
        currentHp = currentHp - _value;

        return currentHp;
    }
}
