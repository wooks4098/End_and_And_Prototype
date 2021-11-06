using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 해당 오브젝트에 플레이어가 다가온 경우 UI매니저에게 요청
/// </summary>
public class ObjectUIShow : MonoBehaviour
{

    [SerializeField] PlayerType playerType;

    //testd용 UI매니저를 만들어 요청하는것으로 수정해야함
    [SerializeField] ObjectUI objectui1;
    [SerializeField] ObjectUI objectui2;


    private void OnTriggerEnter(Collider other)
    {

        if(other.tag == "Player1" && playerType == PlayerType.FirstPlayer)
        {
            objectui1.Show();
        }

        if (other.tag == "Player2" && playerType == PlayerType.SecondPlayer)
        {
            objectui2.Show();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player1" && playerType == PlayerType.FirstPlayer)
        {
            objectui1.Move();
        }

        if (other.tag == "Player2" && playerType == PlayerType.SecondPlayer)
        {
            objectui2.Move();

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player1" && playerType == PlayerType.FirstPlayer)
        {
            objectui1.hide();

        }

        if (other.tag == "Player2" && playerType == PlayerType.SecondPlayer)
        {
            objectui2.hide();

        }
    }

}
