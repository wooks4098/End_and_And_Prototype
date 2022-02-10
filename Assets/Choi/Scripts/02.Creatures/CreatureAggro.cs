using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureAggro : MonoBehaviour
{
    // �÷��̾�1�� ����Ʈ�� ����
    private float player1Point;
    public float Player1Point { get { return player1Point; } set { player1Point += value; } }

    // �÷��̾�2�� ����Ʈ�� ����
    private float player2Point;
    public float Player2Point { get { return player2Point; } set { player2Point += value; } }


    /// <summary>
    /// 0���� ��ü �ʱ�ȭ
    /// </summary>
    public void ResetPoint()
    {
        player1Point = 0;
        player2Point = 0;
    }
}
