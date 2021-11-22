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
        Transform playerTrans = GameManager.Instance.GetPlayerTrans(_playerType);
        playerTrans.position = new Vector3(playerTrans.position.x, playerTrans.position.y+0.3f, playerTrans.position.z);
        yield return new WaitForSeconds(0.25f);

        playerTrans.position = new Vector3(playerTrans.position.x, playerTrans.position.y+0.3f, playerTrans.position.z);
        yield return new WaitForSeconds(0.25f);

        playerTrans.position = new Vector3(playerTrans.position.x, playerTrans.position.y+0.3f, playerTrans.position.z + 0.1f);
        yield return new WaitForSeconds(0.25f);

        playerTrans.position = new Vector3(playerTrans.position.x, playerTrans.position.y+0.3f, playerTrans.position.z + 0.1f);

        yield return new WaitForSeconds(0.25f);

        //_animator.Play("Idle");
        _animator.SetTrigger("ClimbEnd");
        Debug.Log("애니종료");
        yield return null;
        playerTrans.position = new Vector3(playerTrans.position.x, EndPos.position.y, EndPos.position.z);
        Debug.Log(playerTrans.position);
        GameManager.Instance.PlayerStateChange(PlayerType.FirstPlayer, PlayerState.Walk);

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player1" && isPlayer1Up == false)
        {
            isPlayer1Up = true;
            Animator animator = GameManager.Instance.GetPlayerModelTrans(PlayerType.FirstPlayer).GetComponent<Animator>();
            GameManager.Instance.PlayerStateChange(PlayerType.FirstPlayer, PlayerState.ClimbUpWall);
            animator.SetBool("IsClimbinUpWall", true);
            StartCoroutine(PlayerPosChange(animator,PlayerType.FirstPlayer));

        }
        if (other.tag == "Player2" && isPlayer2Up == false)
        {
            isPlayer2Up = true;
            Animator animator = GameManager.Instance.GetPlayerModelTrans(PlayerType.SecondPlayer).GetComponent<Animator>();
            GameManager.Instance.PlayerStateChange(PlayerType.SecondPlayer, PlayerState.ClimbUpWall);
            animator.applyRootMotion = true;
            animator.SetBool("IsClimbinUpWall", true);
            //StartCoroutine(PlayerPosChange(animator));


        }
    }


    //private void OnCollisionEnter(Collision collision)
    //{
    //    Debug.Log(collision.transform.name);
    //    if(collision.transform.tag == "Player1")
    //    {
    //        GameManager.Instance.PlayerStateChange(PlayerType.FirstPlayer, PlayerState.Wait);
    //        GameManager.Instance.GetPlayerModelTrans(PlayerType.FirstPlayer).GetComponent<Animator>().SetBool("IsClimbinUpWall", true);
    //    }
    //    if (collision.transform.tag == "Player2")
    //    {
    //        GameManager.Instance.PlayerStateChange(PlayerType.SecondPlayer, PlayerState.Wait);
    //        GameManager.Instance.GetPlayerModelTrans(PlayerType.SecondPlayer).GetComponent<Animator>().SetBool("IsClimbinUpWall", true);

    //    }
    //}
}
