using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// �ش� ������Ʈ�� �÷��̾ �ٰ��� ��� UI�Ŵ������� ��û
/// </summary>
public class ObjectUIShow : MonoBehaviour
{

    [SerializeField] PlayerType playerType;
    [SerializeField] Transform ObjectUiPos;
    [SerializeField] bool CanUse = false;// ������Ʈ ��밡������
    //testd�� UI�Ŵ����� ����� ��û�ϴ°����� �����ؾ���


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
