using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbingUpWall : MonoBehaviour
{
    [SerializeField]  bool isPlayer1Up = false;
    [SerializeField] bool isPlayer2Up = false;
    [SerializeField] Transform EndPos;
    //올라갈 때 속도
    [SerializeField] float upSpeed;
    [SerializeField] float frontSpeed;


    //플레이어 위치를 벽 위로
    IEnumerator PlayerPosChange(Animator _animator, PlayerType _playerType)
    {
        //벽 위로 위치 변경
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
            //플레이어 위치 올리기
            if (Physics.Raycast(playerTrans.position, playerTrans.forward , out hit, 5f))
            {
                Debug.Log(hit.collider.name);
                if(hit.collider.tag == "ClimbWall")
                {
                    playerController.Move(FrontDir * Time.deltaTime * upSpeed);
                }
            }
           
                yield return null;
        }


        //플레이어 모델, 부모 Rotation되돌리기
        Transform playerModelTrans = GameManager.Instance.GetPlayerModelTrans(_playerType);
        //playerTrans.rotation = Quaternion.Euler(0, 0, 0);
        playerModelTrans.localRotation= Quaternion.Euler(0, 0, 0);
       playerModelTrans.localPosition = Vector3.zero;
        //애니메이션 변경
        _animator.SetTrigger("ClimbEnd");
        yield return null;
        //플레이어 상태변경
        GameManager.Instance.PlayerStateChange(_playerType, PlayerState.Walk);

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player1" && isPlayer1Up == false)
        {
            isPlayer1Up = true;
            Animator animator = GameManager.Instance.GetPlayerModelTrans(PlayerType.FirstPlayer).GetComponent<Animator>();
            GameManager.Instance.PlayerStateChange(PlayerType.FirstPlayer, PlayerState.ClimbUpWall);
            animator.SetTrigger("IsClimbinUpWall");
            StartCoroutine(PlayerPosChange(animator,PlayerType.FirstPlayer));

        }
        if (other.tag == "Player2" && isPlayer2Up == false)
        {
            isPlayer2Up = true;
            Animator animator = GameManager.Instance.GetPlayerModelTrans(PlayerType.SecondPlayer).GetComponent<Animator>();
            GameManager.Instance.PlayerStateChange(PlayerType.SecondPlayer, PlayerState.ClimbUpWall);
            animator.SetTrigger("IsClimbinUpWall");
            StartCoroutine(PlayerPosChange(animator, PlayerType.SecondPlayer));
        }
    }

    public void ChangeEndPos(PlayerType _playerType, Vector3 _playerPos)
    {
        EndPos.position = new Vector3(_playerPos.x, EndPos.position.y, _playerPos.z);
        EndPos.localPosition += new Vector3(0, 0, 0.3f);
    }

}
