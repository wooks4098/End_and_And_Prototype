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
    [SerializeField] List<Floor> floors;
    
    [SerializeField] Transform tParent;


    // 
    [SerializeField] ChessArrayInfo chessArray;
    [SerializeField] List<int> checkfloors;


    private void Awake()
    {
        floorInfo = new int[8, 8];        

        floors = new List<Floor>();
        checkfloors = new List<int>();

    }
    private void Start()
    {
        // =======================================================
        tParent.GetComponentsInChildren<Floor>(floors);


        for (int i = (floorHeight + 1); i >= 0; i--) 
        {
            for (int j = (floorWidth + 1); j >= 0; j--)
            {
                floorInfo[i, j] = chessArray.info[i, j];
            }                
        }

        // ==================================================
        int[,] tempList = new int[6, 6];
        for (int i = 1; i <= floorHeight; i++)
        {
            for (int j = 1; j <= floorWidth; j++)
            {
                tempList[i,j] = chessArray.info[i, j];
            }
        }

        // 2차원 배열을 1차원 배열로
        foreach (var item in tempList)
        {
            for(int i = 0; i < 36; i++)
            {
                checkfloors[i] = item;
                Debug.Log(checkfloors[i]);
            }
        }

    }
    

    public Floor GetFloors(int _index)
    {
        return floors[_index];
    }

    public int[,] GetFloorCount()
    {
        return floorInfo;
    }
}
