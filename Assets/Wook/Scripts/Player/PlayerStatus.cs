using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// �÷��̾��� ���¸� �����ϴ� Ŭ����
/// �÷��̾��� HP ������ (�̵��ӵ��� ����)
/// </summary>
public class PlayerStatus : MonoBehaviour
{
    //ü��
    [SerializeField] float hp;
    [SerializeField] float maxHp;

    //����
    [SerializeField] bool isspores; //���ڿ� �ߵ�������
    [SerializeField] float sporesdecreasefigure; //������ hp ���Ҽ�ġ (�ʴ� ��ġ)



    public void ChangeHp(float _changeHp)
    {
        Mathf.Max(hp + _changeHp, 0, maxHp);
        if(hp <= 0)
        {
            //����
        }
    }


    #region ���ڰ���

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
