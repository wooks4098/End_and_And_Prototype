using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChessManager : MonoBehaviour
{
    // 바닥 정보를 저장할 2차원 배열
    int[,] floorInfo;

    int floorWidth = 6;
    int floorHeight = 6;

    [SerializeField] float floorSize = 5f;


    private void Start()
    {
        floorInfo = new int[8, 8];

        for (int i = (floorHeight + 1); i >= 0; i--) 
        {
            for (int j = (floorWidth + 1); j >= 0; j--)
            {
                floorInfo[i, j] = 0;
            }                
        }
    }
    
    public int[,] GetFloorCount()
    {
        return floorInfo;
    }
}
