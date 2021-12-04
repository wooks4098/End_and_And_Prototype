using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 플레이어의 상태를 관리하는 클래스
/// 플레이어의 HP 감염등 (이동속도는 제외)
/// </summary>
public class PlayerStatus : MonoBehaviour
{
    //체력
    [SerializeField] float hp;
    [SerializeField] float maxHp;

    //포자
    [SerializeField] bool isspores; //포자에 중독중인지
    [SerializeField] float sporesdecreasefigure; //포자의 hp 감소수치 (초당 수치)



    public void ChangeHp(float _changeHp)
    {
        Mathf.Max(hp + _changeHp, 0, maxHp);
        if(hp <= 0)
        {
            //죽음
        }
    }


    #region 포자관련

    public void StartSpores()
    {
        StartCoroutine(Spores());
    }

    IEnumerator Spores()
    {
        while(isspores)
        {
            ChangeHp(-sporesdecreasefigure);
            yield return new WaitForSeconds(1f);
        }
    }
    #endregion



}
