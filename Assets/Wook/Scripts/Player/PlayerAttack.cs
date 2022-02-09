using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어 공격 스크립트
/// 
/// </summary>

public class PlayerAttack : MonoBehaviour
{

    [SerializeField] PlayerType playerType;
    CharacterController characterController;
    PlayerController playerController;
    Animator ani;

    [Header("Attack")]
    //공격 변수
    [SerializeField] float attackCooltime;
    [SerializeField] int AttackComboCount = 0;//콤보 번호 초기 0 | 공격종류 1~3
    [SerializeField] bool CanAttack = true; // 공격 가능한지
    [SerializeField] ParticleSystem[] AttackParticle;//공격 파티클
    [SerializeField] float[] ParticleDelay;//파티클 대기시간
    [SerializeField] BoxCollider AttackCollider;//공격 충돌용 콜라이더
    [Header("Skin")]
    //무기
    [SerializeField] GameObject HandSword; //손에 들고있는 칼
    [SerializeField] GameObject BackSword; //등에 집어넣은 칼



    private void Awake()
    {
        ani = GetComponent<Animator>();
    }
    private void Start()
    {
        SetInput();
    }

    //PlayerController에서 컴포넌트 가져오기
    public void Getcomponent(PlayerType _playerType, CharacterController _characterController, PlayerController _PlayerController)
    {
        playerType = _playerType;
        characterController = _characterController;
        playerController = _PlayerController;
    }

    //input event 등록
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

    //공격키를 누름
    void AttackClick(PlayerState playerstate)
    {
        //공격 가능한 조건인지 확인
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
        {//콤보를 이어 갈 수 있는 상황
            if(AttackComboCount == 0  && playerController.GetUseSword()==false)
            {//첫 공격이면서 검을 뽑지 않은 상태
                StartCoroutine(DrawSword());
            }
            else
            {
                StartCoroutine(ComboAttack(false));
            }
            CanAttack = false;
        }
        else if (AttackComboCount == 2)
        {//마지막 콤보공격
            StartCoroutine(ComboAttack(false));
            StartCoroutine(EndComboAttack());
        }
        else
        {
            StartCoroutine(EndComboAttack());
        }
    }
    //콤보 종료
    IEnumerator EndComboAttack()
    {
        CanAttack = false;
        yield return new WaitForSeconds(0.4f);
        playerController.PlayerStateChange(PlayerState.Walk);
        yield return new WaitForSeconds(0.2f);
        CanAttack = true;
        AttackComboCount = 0;
    }
    //공격 쿨타임
    IEnumerator AttackCooltime()
    {
        yield return new WaitForSeconds(attackCooltime);
        if(AttackComboCount < 3)
            CanAttack = true;
        AttackCollider.enabled = false;
        //콤보 끊김 체크 
        yield return new WaitForSeconds(0.5f);
        if(CanAttack == true)
            StartCoroutine(EndComboAttack());

    }

    //칼 꺼내기
    IEnumerator DrawSword()
    {
        if (playerController.GetUseSword() == false)
        {
            //애니
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
        {//칼을 뽑고 공격하는 경우

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

    //공격 이펙트 보여주기 animation Event
    public void ShowAttackEffectInAni(int _particleNumber)
    {
        StartCoroutine(ShowEffect(_particleNumber));
    }

    //공격 이펙트 보여주기
    IEnumerator ShowEffect(int _particleNumber)
    {
        yield return new WaitForSeconds(ParticleDelay[_particleNumber]);
        AttackParticle[_particleNumber].Emit(1);
    }
}
