using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// �ش� ������Ʈ�� �÷��̾ �ٰ��� ��� UI�Ŵ������� ��û
/// �ش� ������Ʈ�� ����� �� �ִ� �������� Ȯ��
/// </summary>
public class ObjectUIShow : MonoBehaviour
{

    [SerializeField] PlayerType playerType;
    [SerializeField] Transform ObjectUiPos;
    [SerializeField] bool CanUse = false;// ������Ʈ ��밡������


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
