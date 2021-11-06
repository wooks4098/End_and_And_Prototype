using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// �ش� ������Ʈ�� �÷��̾ �ٰ��� ��� UI�Ŵ������� ��û
/// </summary>
public class ObjectUIShow : MonoBehaviour
{

    [SerializeField] PlayerType playerType;

    //testd�� UI�Ŵ����� ����� ��û�ϴ°����� �����ؾ���
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
