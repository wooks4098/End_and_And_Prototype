using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureAggro : MonoBehaviour
{
    // 플레이어1의 포인트를 저장
    private float player1Point;
    public float Player1Point { get { return player1Point; } set { player1Point += value; } }

    // 플레이어2의 포인트를 저장
    private float player2Point;
    public float Player2Point { get { return player2Point; } set { player2Point += value; } }


    /// <summary>
    /// 0으로 전체 초기화
    /// </summary>
    public void ResetPoint()
    {
        player1Point = 0;
        player2Point = 0;
    }
}
