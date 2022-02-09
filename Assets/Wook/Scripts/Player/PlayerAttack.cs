using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �÷��̾� ���� ��ũ��Ʈ
/// 
/// </summary>

public class PlayerAttack : MonoBehaviour
{

    [SerializeField] PlayerType playerType;
    CharacterController characterController;
    PlayerController playerController;
    Animator ani;

    [Header("Attack")]
    //���� ����
    [SerializeField] float attackCooltime;
    [SerializeField] int AttackComboCount = 0;//�޺� ��ȣ �ʱ� 0 | �������� 1~3
    [SerializeField] bool CanAttack = true; // ���� ��������
    [SerializeField] ParticleSystem[] AttackParticle;//���� ��ƼŬ
    [SerializeField] float[] ParticleDelay;//��ƼŬ ���ð�
    [SerializeField] BoxCollider AttackCollider;//���� �浹�� �ݶ��̴�
    [Header("Skin")]
    //����
    [SerializeField] GameObject HandSword; //�տ� ����ִ� Į
    [SerializeField] GameObject BackSword; //� ������� Į



    private void Awake()
    {
        ani = GetComponent<Animator>();
    }
    private void Start()
    {
        SetInput();
    }

    //PlayerController���� ������Ʈ ��������
    public void Getcomponent(PlayerType _playerType, CharacterController _characterController, PlayerController _PlayerController)
    {
        playerType = _playerType;
        characterController = _characterController;
        playerController = _PlayerController;
    }

    //input event ���
    void SetInput()
    {
        switch (playerType)
        {
            case PlayerType.FirstPlayer:
                InputManager.Instance.OnAttackPlayer1 += AttackClick;
                break;

            case PlayerType.SecondPlayer:
                InputManager.Instance.OnAttackPlayer2 += AttackClick;

                break;
        }
    }

    //����Ű�� ����
    void AttackClick(PlayerState playerstate)
    {
        //���� ������ �������� Ȯ��
        if (CanAttack == false)
            return;
        switch (playerstate)
        {
            case PlayerState.ClimbWall:
            case PlayerState.ClimbUpWall:
            case PlayerState.ClimbWallFall:
            case PlayerState.HoldRope:
            case PlayerState.SafeBox:
            case PlayerState.Die:
                return;
        }

        if (AttackComboCount < 2)
        {//�޺��� �̾� �� �� �ִ� ��Ȳ
            if(AttackComboCount == 0  && playerController.GetUseSword()==false)
            {//ù �����̸鼭 ���� ���� ���� ����
                StartCoroutine(DrawSword());
            }
            else
            {
                StartCoroutine(ComboAttack(false));
            }
            CanAttack = false;
        }
        else if (AttackComboCount == 2)
        {//������ �޺�����
            StartCoroutine(ComboAttack(false));
            StartCoroutine(EndComboAttack());
        }
        else
        {
            StartCoroutine(EndComboAttack());
        }
    }
    //�޺� ����
    IEnumerator EndComboAttack()
    {
        CanAttack = false;
        yield return new WaitForSeconds(0.4f);
        playerController.PlayerStateChange(PlayerState.Walk);
        yield return new WaitForSeconds(0.2f);
        CanAttack = true;
        AttackComboCount = 0;
    }
    //���� ��Ÿ��
    IEnumerator AttackCooltime()
    {
        yield return new WaitForSeconds(attackCooltime);
        if(AttackComboCount < 3)
            CanAttack = true;
        AttackCollider.enabled = false;
        //�޺� ���� üũ 
        yield return new WaitForSeconds(0.5f);
        if(CanAttack == true)
            StartCoroutine(EndComboAttack());

    }

    //Į ������
    IEnumerator DrawSword()
    {
        if (playerController.GetUseSword() == false)
        {
            //�ִ�
            ani.SetTrigger("DrawSword");
            yield return new WaitForSeconds(0.3f);
            HandSword.SetActive(true);
            BackSword.SetActive(false);
            playerController.ChangeUseSowrd(true);

            yield return new WaitForSeconds(0.5f);
            StartCoroutine(ComboAttack(true));
        }
    }
    

    IEnumerator ComboAttack(bool isDrawSword)
    {
        string str;
        int attackcount = AttackComboCount;
        AttackCollider.enabled = true;
        if (isDrawSword)
        {//Į�� �̰� �����ϴ� ���

        }
        else
        {
            str = "Attack" + AttackComboCount.ToString();
            ani.SetTrigger(str);
        }
        playerController.PlayerStateChange(PlayerState.Attack);
        StartCoroutine(ShowEffect(attackcount));
        StartCoroutine(AttackCooltime());
        AttackComboCount++;

        yield return null;
    }

    //���� ����Ʈ �����ֱ� animation Event
    public void ShowAttackEffectInAni(int _particleNumber)
    {
        StartCoroutine(ShowEffect(_particleNumber));
    }

    //���� ����Ʈ �����ֱ�
    IEnumerator ShowEffect(int _particleNumber)
    {
        yield return new WaitForSeconds(ParticleDelay[_particleNumber]);
        AttackParticle[_particleNumber].Emit(1);
    }
}
