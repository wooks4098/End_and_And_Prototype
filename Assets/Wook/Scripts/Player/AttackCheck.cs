using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ���ݽ� �浹������ �Ͽ� ������ �������� �ִ� Ŭ����
/// </summary>
public class AttackCheck : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
<<<<<<< Updated upstream
        //if(other.tag == "ũ��ó")
            //����
=======
        if (other.tag == "Enemy")
        {
            //Enemy������ �ֱ�
            CreatureHp creatureHp = other.GetComponent<CreatureHp>();
            creatureHp.GetDamage(10f);
        }
>>>>>>> Stashed changes
    }
}
   
