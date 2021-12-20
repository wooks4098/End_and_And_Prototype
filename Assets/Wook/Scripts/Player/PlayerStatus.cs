using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// �÷��̾��� ���¸� �����ϴ� Ŭ����
/// �÷��̾��� HP ������ (�̵��ӵ��� ����)
/// </summary>
public class PlayerStatus : MonoBehaviour
{
    [SerializeField] PlayerType playerType;
    //ü��
    [SerializeField] float hp;
    [SerializeField] float maxHp;

    //�񸶸�
    [SerializeField] float thirst;
    [SerializeField] float maxthirst;


    //����
    [SerializeField] bool isspores; //���ڿ� �ߵ�������
    [SerializeField] float sporesdecreasefigure; //������ hp ���Ҽ�ġ (�ʴ� ��ġ)


    public void ChangeHp(float _changeHp)
    {
        hp = Mathf.Clamp((hp + _changeHp), 0, maxHp);
        UIManager.Instance.ChangeHpUi(playerType, hp);
        if (hp <= 0)
        {
            //����
        }
    }

    public void Changethirst(float _changeThirst)
    {
        Mathf.Max(thirst + _changeThirst, 0, maxthirst);
        if (hp <= 0)
        {
            //�񸶸� ü��--
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