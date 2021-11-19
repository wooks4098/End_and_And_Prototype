using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbingUpWall : MonoBehaviour
{
    bool isPlayer1Up = false;
    bool isPlayer2Up = false;
    //플레이어 위치를 벽 위로
    IEnumerator PlayerPosChange(Animator _animator)
    {
        yield return new WaitForSeconds(3.14f);
        _animator.SetTrigger("ClimbEnd");

    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.transform.name);
        if (other.tag == "Player1" && isPlayer1Up == false)
        {
            isPlayer1Up = true;
            Animator animator = GameManager.Instance.GetPlayerModelTrans(PlayerType.FirstPlayer).GetComponent<Animator>();
            GameManager.Instance.PlayerStateChange(PlayerType.FirstPlayer, PlayerState.ClimbUpWall);
            //animator.applyRootMotion = true;
            animator.SetBool("IsClimbinUpWall", true);
            StartCoroutine(PlayerPosChange(animator));

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
