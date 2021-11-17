using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 해당 오브젝트에 플레이어가 다가온 경우 UI매니저에게 요청
/// </summary>
public class ObjectUIShow : MonoBehaviour
{

    [SerializeField] PlayerType playerType;
    [SerializeField] Transform ObjectUiPos;
    [SerializeField] bool CanUse = false;// 오브젝트 사용가능한지
    //testd용 UI매니저를 만들어 요청하는것으로 수정해야함


    public bool GetCanUse()
    {
        return CanUse;
    }

    public PlayerType GetPlayerType()
    {
        return playerType;
    }

    private void OnTriggerEnter(Collider other)
    {

        if(other.tag == "Player1" && playerType == PlayerType.FirstPlayer)
        {
            CanUse = true;
            GameManager.Instance.PlayerObjectHitin(PlayerType.FirstPlayer);
            //UIManager.Instance.ObjectUIShow(PlayerType.FirstPlayer);
        }

        if (other.tag == "Player2" && playerType == PlayerType.SecondPlayer)
        {
            CanUse = true;
            GameManager.Instance.PlayerObjectHitin(PlayerType.SecondPlayer);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player1" && playerType == PlayerType.FirstPlayer)
        {
            GameManager.Instance.ObjectUiMove(PlayerType.FirstPlayer, ObjectUiPos.position);

        }

        if (other.tag == "Player2" && playerType == PlayerType.SecondPlayer)
        {
            GameManager.Instance.ObjectUiMove(PlayerType.SecondPlayer, ObjectUiPos.position);


        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player1" && playerType == PlayerType.FirstPlayer)
        {
            CanUse = false;
            GameManager.Instance.PlayerObjectHitout(PlayerType.FirstPlayer);

        }

        if (other.tag == "Player2" && playerType == PlayerType.SecondPlayer)
        {
            CanUse = false;
            GameManager.Instance.PlayerObjectHitout(PlayerType.SecondPlayer);

        }
    }

}
