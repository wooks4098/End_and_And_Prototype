using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbWallReset : MonoBehaviour
{
    [SerializeField] Transform Player1;
    [SerializeField] Transform Player2;

    [SerializeField] ClimbWall climbWall;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
            Reset();
    }
    public void Reset()
    {
        climbWall.Reset();
        GameManager.Instance.PlayerStateChange(PlayerType.FirstPlayer, PlayerState.Walk);
        GameManager.Instance.PlayerStateChange(PlayerType.SecondPlayer, PlayerState.Walk);
        GameManager.Instance.GetPlayerModelTrans(PlayerType.FirstPlayer).GetComponent<Animator>().Play("Idle");
        GameManager.Instance.GetPlayerModelTrans(PlayerType.SecondPlayer).GetComponent<Animator>().Play("Idle");
        GameManager.Instance.GetPlayerTrans(PlayerType.FirstPlayer).position = Player1.position;
        GameManager.Instance.GetPlayerTrans(PlayerType.SecondPlayer).position = Player2.position;
    }
}
