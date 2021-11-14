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
    public int[,] info;

    private void Awake()
    {
        info = new int[8, 8];
    }

    private void Start()
    {
        info[0, 0] = 0; info[0, 1] = 0; info[0, 2] = 0; info[0, 3] = 0; info[0, 4] = 0; info[0, 5] = 0; info[0, 6] = 0; info[0, 7] = 0;
        info[1, 0] = 1; info[1, 1] = 1; info[1, 2] = 1; info[1, 3] = 0; info[1, 4] = 0; info[1, 5] = 0; info[1, 6] = 0; info[1, 7] = 0;
        info[2, 0] = 0; info[2, 1] = 0; info[2, 2] = 1; info[2, 3] = 0; info[2, 4] = 1; info[2, 5] = 1; info[2, 6] = 1; info[2, 7] = 0;
        info[3, 0] = 0; info[3, 1] = 0; info[3, 2] = 1; info[3, 3] = 1; info[3, 4] = 1; info[3, 5] = 0; info[3, 6] = 1; info[3, 7] = 0;
        info[4, 0] = 0; info[4, 1] = 0; info[4, 2] = 0; info[4, 3] = 0; info[4, 4] = 0; info[4, 5] = 1; info[4, 6] = 1; info[4, 7] = 0;
        info[5, 0] = 0; info[5, 1] = 0; info[5, 2] = 0; info[5, 3] = 0; info[5, 4] = 0; info[5, 5] = 1; info[5, 6] = 0; info[5, 7] = 0;
        info[6, 0] = 0; info[6, 1] = 0; info[6, 2] = 0; info[6, 3] = 0; info[6, 4] = 1; info[6, 5] = 1; info[6, 6] = 0; info[6, 7] = 0;
        info[7, 0] = 0; info[7, 1] = 0; info[7, 2] = 0; info[7, 3] = 0; info[7, 4] = 0; info[7, 5] = 0; info[7, 6] = 0; info[7, 7] = 0;
    }
}
