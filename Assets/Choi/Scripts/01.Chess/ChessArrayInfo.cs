using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// 1 1 0 0 0 0
// 0 1 0 1 1 1
// 0 1 1 1 0 1
// 0 0 0 0 1 1
// 0 0 0 0 1 0
// 0 0 0 1 1 0


/// <summary>
/// 
/// </summary>

[Serializable]
public class ChessArrayInfo : MonoBehaviour
{
    public int[][] infoByTwo; // 이차원에 저장한 체스판 정보 (연산할 바깥 테두리 체스판 정보까지 저장)
    public int[] infoByOne; // 일차원에 저장한 체스판 정보 (실제 체스판 정보만 저장)

    private void Awake()
    {
        infoByTwo = new int[8][];
        for (int i = 0; i < 8; i++)
        {
            infoByTwo[i] = new int[8];
        }

        infoByOne = new int[36];
        
        SetArray();
    }

    private void SetArray()
    {
        // 2차원 배열
        infoByTwo[0][0] = 1; infoByTwo[0][1] = 0; infoByTwo[0][2] = 0; infoByTwo[0][3] = 0; infoByTwo[0][4] = 0; infoByTwo[0][5] = 0; infoByTwo[0][6] = 0; infoByTwo[0][7] = 0;
        infoByTwo[1][0] = 1; infoByTwo[1][1] = 1; infoByTwo[1][2] = 1; infoByTwo[1][3] = 0; infoByTwo[1][4] = 0; infoByTwo[1][5] = 0; infoByTwo[1][6] = 0; infoByTwo[1][7] = 0;
        infoByTwo[2][0] = 0; infoByTwo[2][1] = 0; infoByTwo[2][2] = 1; infoByTwo[2][3] = 0; infoByTwo[2][4] = 1; infoByTwo[2][5] = 1; infoByTwo[2][6] = 1; infoByTwo[2][7] = 0;
        infoByTwo[3][0] = 0; infoByTwo[3][1] = 0; infoByTwo[3][2] = 1; infoByTwo[3][3] = 1; infoByTwo[3][4] = 1; infoByTwo[3][5] = 0; infoByTwo[3][6] = 1; infoByTwo[3][7] = 0;
        infoByTwo[4][0] = 0; infoByTwo[4][1] = 0; infoByTwo[4][2] = 0; infoByTwo[4][3] = 0; infoByTwo[4][4] = 0; infoByTwo[4][5] = 1; infoByTwo[4][6] = 1; infoByTwo[4][7] = 0;
        infoByTwo[5][0] = 0; infoByTwo[5][1] = 0; infoByTwo[5][2] = 0; infoByTwo[5][3] = 0; infoByTwo[5][4] = 0; infoByTwo[5][5] = 1; infoByTwo[5][6] = 0; infoByTwo[5][7] = 0;
        infoByTwo[6][0] = 0; infoByTwo[6][1] = 0; infoByTwo[6][2] = 0; infoByTwo[6][3] = 0; infoByTwo[6][4] = 1; infoByTwo[6][5] = 1; infoByTwo[6][6] = 0; infoByTwo[6][7] = 0;
        infoByTwo[7][0] = 0; infoByTwo[7][1] = 0; infoByTwo[7][2] = 0; infoByTwo[7][3] = 0; infoByTwo[7][4] = 0; infoByTwo[7][5] = 1; infoByTwo[7][6] = 0; infoByTwo[7][7] = 0;

        // 1차원 배열
        infoByOne[0] = 1; infoByOne[1] = 1; infoByOne[2] = 0; infoByOne[3] = 0; infoByOne[4] = 0; infoByOne[5] = 0;
        infoByOne[6] = 0; infoByOne[7] = 1; infoByOne[8] = 0; infoByOne[9] = 1; infoByOne[10] = 1; infoByOne[11] = 1;
        infoByOne[12] = 0; infoByOne[13] = 1; infoByOne[14] = 1; infoByOne[15] = 1; infoByOne[16] = 0; infoByOne[17] = 1;
        infoByOne[18] = 0; infoByOne[19] = 0; infoByOne[20] = 0; infoByOne[21] = 0; infoByOne[22] = 1; infoByOne[23] = 1;
        infoByOne[24] = 0; infoByOne[25] = 0; infoByOne[26] = 0; infoByOne[27] = 0; infoByOne[28] = 1; infoByOne[29] = 0;
        infoByOne[30] = 0; infoByOne[31] = 0; infoByOne[32] = 0; infoByOne[33] = 1; infoByOne[34] = 1; infoByOne[35] = 0;
    }
}

