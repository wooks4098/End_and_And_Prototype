using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbingUpWall : MonoBehaviour
{
    [SerializeField]  bool isPlayer1Up = false;
    [SerializeField] bool isPlayer2Up = false;
    [SerializeField] Transform EndPos;
    //플레이어 위치를 벽 위로
    IEnumerator PlayerPosChange(Animator _animator, PlayerType _playerType)
    {
        //벽 위로 위치 변경
        Transform playerTrans = GameManager.Instance.GetPlayerTrans(_playerType);
        playerTrans.position = new Vector3(playerTrans.position.x, playerTrans.position.y+0.4f, playerTrans.position.z);
        yield return new WaitForSeconds(0.25f);

        playerTrans.position = new Vector3(playerTrans.position.x, playerTrans.position.y+0.4f, playerTrans.position.z);
        yield return new WaitForSeconds(0.25f);

        playerTrans.position = new Vector3(playerTrans.position.x, playerTrans.position.y+0.4f, playerTrans.position.z + 0.1f);
        yield return new WaitForSeconds(0.25f);

        playerTrans.position = new Vector3(playerTrans.position.x, playerTrans.position.y+0.4f, playerTrans.position.z + 0.1f);

        yield return new WaitForSeconds(0.25f);
        //playerTrans.position = new Vector3(playerTrans.position.x, EndPos.position.y, EndPos.position.z);
        playerTrans.position = new Vector3(playerTrans.position.x, playerTrans.position.y + 0.8f, playerTrans.position.z + 0.4f);

        //플레이어 모델, 부모 Rotation되돌리기
        Transform playerModelTrans = GameManager.Instance.GetPlayerModelTrans(_playerType);
        playerTrans.rotation = Quaternion.Euler(0, 0, 0);
        playerModelTrans.rotation = Quaternion.Euler(0, 0, 0);
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


}
