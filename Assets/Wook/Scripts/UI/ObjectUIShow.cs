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
    //testd용 UI매니저를 만들어 요청하는것으로 수정해야함
    [SerializeField] ObjectUI objectui1;
    [SerializeField] ObjectUI objectui2;

    [SerializeField] bool P1CanUse = false;
    [SerializeField] bool P2CanUse = false;

    private void OnTriggerEnter(Collider other)
    {

        if(other.tag == "Player1" && playerType == PlayerType.FirstPlayer)
        {
            UIManager.Instance.ObjectUIShow(PlayerType.FirstPlayer);
            //objectui1.Show();
            P1CanUse = true;
        }

        if (other.tag == "Player2" && playerType == PlayerType.SecondPlayer)
        {
            UIManager.Instance.ObjectUIShow(PlayerType.SecondPlayer);

            //objectui2.Show();
            P2CanUse = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player1" && playerType == PlayerType.FirstPlayer)
        {
            UIManager.Instance.ObjectUIMove(PlayerType.FirstPlayer, ObjectUiPos.position);

            //objectui1.Move();
        }

        if (other.tag == "Player2" && playerType == PlayerType.SecondPlayer)
        {
            UIManager.Instance.ObjectUIMove(PlayerType.SecondPlayer, ObjectUiPos.position);
            //objectui2.Move();

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player1" && playerType == PlayerType.FirstPlayer)
        {
            UIManager.Instance.ObjectUIHide(PlayerType.FirstPlayer);
            //objectui1.hide();
            P1CanUse = false;
        }

        if (other.tag == "Player2" && playerType == PlayerType.SecondPlayer)
        {
            UIManager.Instance.ObjectUIHide(PlayerType.SecondPlayer);
            //objectui2.hide();
            P1CanUse = false;
        }
    }

}
