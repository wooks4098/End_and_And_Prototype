using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 해당 오브젝트에 플레이어가 다가온 경우 UI매니저에게 요청
/// 해당 오브젝트를 사용할 수 있는 상태인지 확인
/// </summary>
public class ObjectUIShow : MonoBehaviour
{

    [SerializeField] PlayerType playerType;
    [SerializeField] Transform ObjectUiPos;
    [SerializeField] bool CanUse = false;// 오브젝트 사용가능한지


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
        switch(playerType)
        {
            case PlayerType.FirstPlayer:
                if (other.tag == "Player1")
                {
                    CanUse = true;
                    GameManager.Instance.PlayerObjectHitin(PlayerType.FirstPlayer);
                    //UIManager.Instance.ObjectUIShow(PlayerType.FirstPlayer);
                }
                break;
            case PlayerType.SecondPlayer:
                if (other.tag == "Player2")
                {
                    CanUse = true;
                    GameManager.Instance.PlayerObjectHitin(PlayerType.SecondPlayer);
                }
                break;
        }
        

        
    }

    private void OnTriggerStay(Collider other)
    {
        switch (playerType)
        {
            case PlayerType.FirstPlayer:
                if (other.tag == "Player1")
                    GameManager.Instance.ObjectUiMove(PlayerType.FirstPlayer, ObjectUiPos.position);
                break;
            case PlayerType.SecondPlayer:
                if (other.tag == "Player2")
                    GameManager.Instance.ObjectUiMove(PlayerType.SecondPlayer, ObjectUiPos.position);
                break;
        }
                
    }

    private void OnTriggerExit(Collider other)
    {
        switch (playerType)
        {
            case PlayerType.FirstPlayer:
                if (other.tag == "Player1")
                {
                    CanUse = false;
                    GameManager.Instance.PlayerObjectHitout(PlayerType.FirstPlayer);
                }
                    break;
            case PlayerType.SecondPlayer:
                if (other.tag == "Player2")
                {
                    CanUse = false;
                    GameManager.Instance.PlayerObjectHitout(PlayerType.SecondPlayer);
                }
                    break;
        }
       
    }

}
