using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 플레이어의 상태를 관리하는 클래스
/// 플레이어의 HP 감염등 (이동속도는 제외)
/// </summary>
public class PlayerStatus : MonoBehaviour
{
    [SerializeField] PlayerType playerType;
    PlayerController playerController;
    //체력
    [SerializeField] float hp;
    [SerializeField] float maxHp;

    //목마름
    [SerializeField] float thirst;
    [SerializeField] float maxthirst;


    //포자
    [SerializeField] bool isspores; //포자에 중독중인지
    [SerializeField] float sporesdecreasefigure; //포자의 hp 감소수치 (초당 수치)

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    public void ChangeHp(float _changeHp)
    {
        hp = Mathf.Clamp((hp + _changeHp), 0, maxHp);
        UIManager.Instance.ChangeHpUi(playerType, hp);
        if (hp <= 0)
        {
            //임시 빈사상태
            if (playerController.GetPlayerState() == PlayerState.Crawl)
                return;
            playerController.PlayerStateChange(PlayerState.Crawl);


        }
    }


    public void Changethirst(float _changeThirst)
    {
        Mathf.Max(thirst + _changeThirst, 0, maxthirst);
        if (hp <= 0)
        {
            //목마름 체력--
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
