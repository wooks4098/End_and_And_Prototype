using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbingUpWall : MonoBehaviour
{
    [SerializeField]  bool isPlayer1Up = false;
    [SerializeField] bool isPlayer2Up = false;
    [SerializeField] Transform EndPos;
    //�ö� �� �ӵ�
    [SerializeField] float upSpeed;
    [SerializeField] float frontSpeed;


    //�÷��̾� ��ġ�� �� ����
    IEnumerator PlayerPosChange(Animator _animator, PlayerType _playerType)
    {
        //�� ���� ��ġ ����
        Transform playerTrans = GameManager.Instance.GetPlayerTrans(_playerType);
        CharacterController playerController = playerTrans.GetComponent<CharacterController>();
        float TimeCheck = 1f;
        Vector3 FrontDir; 
        RaycastHit hit;
        while(TimeCheck > 0)
        {
            TimeCheck -= Time.deltaTime;
            Debug.DrawRay(playerTrans.position,playerTrans.forward * 10, Color.blue);
            Debug.DrawRay(new Vector3(playerTrans.position.x, playerTrans.position.y + 0.4f, playerTrans.position.z), playerTrans.forward * 10, Color.blue);

            FrontDir = EndPos.position - playerTrans.position;
            FrontDir = FrontDir.normalized;
            //�÷��̾� ��ġ �ø���
            if (Physics.Raycast(playerTrans.position, playerTrans.forward , out hit, 5f))
            {
                if(hit.collider.tag == "ClimbWall")
                {
                    playerController.Move(FrontDir * Time.deltaTime * upSpeed);
                }
            }
           
                yield return null;
        }


        //�÷��̾� ��, �θ� Rotation�ǵ�����
        Transform playerModelTrans = GameManager.Instance.GetPlayerModelTrans(_playerType);
        //playerTrans.rotation = Quaternion.Euler(0, 0, 0);
        playerModelTrans.localRotation= Quaternion.Euler(0, 0, 0);
       playerModelTrans.localPosition = Vector3.zero;
        //�ִϸ��̼� ����
        _animator.SetTrigger("ClimbEnd");
        yield return null;
        //�÷��̾� ���º���
        GameManager.Instance.PlayerStateChange(_playerType, PlayerState.Walk);

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player1" && isPlayer1Up == false)
        {
            isPlayer1Up = true;
            
            Animator animator = GameManager.Instance.GetPlayerModelTrans(PlayerType.FirstPlayer).GetComponent<Animator>();

            //�����÷��̾ ����Ÿ���  ���� ��� �÷��̾� ���� ��� ���ߵ���
            if (GameManager.Instance.GetPlayerState(PlayerType.FirstPlayer) == PlayerState.ClimbRope)
            {
                GameManager.Instance.GetPlayerController(PlayerType.SecondPlayer).EndHoldRope();

                //Animator otherani = GameManager.Instance.GetPlayerModelTrans(PlayerType.SecondPlayer).GetComponent<Animator>();
                //otherani.SetTrigger("ClimbupFall");
                //otherani.SetTrigger("ClimbupFallExit");
                //otherani.SetBool("HoldingRope",false);



            }

            GameManager.Instance.PlayerStateChange(PlayerType.FirstPlayer, PlayerState.ClimbUpWall);
            animator.SetTrigger("IsClimbinUpWall");
            StartCoroutine(PlayerPosChange(animator,PlayerType.FirstPlayer));
            //Climb up Slider �����
            UIManager.Instance.EndClimbWall(PlayerType.FirstPlayer);
            //Player input ����
            GameManager.Instance.GetPlayerController(PlayerType.FirstPlayer).ClimbWallEnd();


        }
        if (other.tag == "Player2" && isPlayer2Up == false)
        {
            isPlayer2Up = true;
            Animator animator = GameManager.Instance.GetPlayerModelTrans(PlayerType.SecondPlayer).GetComponent<Animator>();

            //�����÷��̾ ����Ÿ���  ���� ��� �÷��̾� ���� ��� ���ߵ���
            if (GameManager.Instance.GetPlayerState(PlayerType.SecondPlayer) == PlayerState.ClimbRope)
            {
                GameManager.Instance.GetPlayerController(PlayerType.FirstPlayer).EndHoldRope();
                //Animator otherani = GameManager.Instance.GetPlayerModelTrans(PlayerType.FirstPlayer).GetComponent<Animator>();
                //otherani.SetTrigger("ClimbupFall");
                //otherani.SetTrigger("ClimbupFallExit");
                //otherani.SetBool("HoldingRope", false);



            }

            GameManager.Instance.PlayerStateChange(PlayerType.SecondPlayer, PlayerState.ClimbUpWall);
            animator.SetTrigger("IsClimbinUpWall");
            StartCoroutine(PlayerPosChange(animator, PlayerType.SecondPlayer));
            //Climb up Slider �����
            UIManager.Instance.EndClimbWall(PlayerType.SecondPlayer);
            //Player input ����
            GameManager.Instance.GetPlayerController(PlayerType.SecondPlayer).ClimbWallEnd();
           
        }
    }

    public void ChangeEndPos(PlayerType _playerType, Vector3 _playerPos)
    {
        EndPos.position = new Vector3(_playerPos.x, EndPos.position.y, _playerPos.z);
        EndPos.localPosition += new Vector3(0, 0, 0.3f);
    }

}
