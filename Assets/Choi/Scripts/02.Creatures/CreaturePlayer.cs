using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreaturePlayer : MonoBehaviour
{
    // 어그로 테스트용 점수
    public int score = 0;

    [SerializeField] float playerHp = 100f;

    [SerializeField] bool isDead;

    private void Start()
    {
        score = 0;

        playerHp = 100f;
        isDead = false;
    }

    private void Update()
    {
        if(playerHp <= 0f)
        {
            isDead = true;
        }
    }

    public bool GetIsDead()
    {
        return isDead;
    }

    public float CalculatePlayerHP(float _value = 0)
    {
        playerHp = playerHp - _value;

        return playerHp;
    }
}
