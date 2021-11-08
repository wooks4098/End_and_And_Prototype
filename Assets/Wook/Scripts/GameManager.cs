using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }
    PlayerController player1;
    PlayerController player2;

    private void Awake()
    {

        if (null == instance)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        player1 = GameObject.FindWithTag("Player1").gameObject.GetComponent<PlayerController>();
        player2 = GameObject.FindWithTag("Player2").gameObject.GetComponent<PlayerController>();
 
    }

    //플레이어 움직임 Move관련 input제어
    public void PlayerCanMoveChange(PlayerType playerType, bool CanMove)
    {
        switch (playerType)
        {
            case PlayerType.FirstPlayer:
                player1.PlayerCanMoveChange(CanMove);
                break;
            case PlayerType.SecondPlayer:
                player2.PlayerCanMoveChange(CanMove);
                break;
        }
    }
}
