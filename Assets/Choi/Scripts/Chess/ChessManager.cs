using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChessManager : MonoBehaviour
{
    // 바닥 정보를 저장할 2차원 배열
    int[,] floorInfo;
    [SerializeField] int[] actureOfChessFloor;

    readonly int floorWidth = 6;
    readonly int floorHeight = 6;

    [SerializeField] float floorSize = 5f;
    [SerializeField] List<Floor> floors;
    
    [SerializeField] Transform tParent;

    // ==============================================
    [SerializeField] MeshRenderer mrFloorRenderer;
    [SerializeField] List<Texture> texTextureContainer;

    Material mFloorMaterial;

    // ==============================================
    [SerializeField] ChessArrayInfo chessArray;
    List<bool> checkAvailablefloors;


    private void Awake()
    {
        floorInfo = new int[8, 8];        

        floors = new List<Floor>();
        checkAvailablefloors = new List<bool>();

        actureOfChessFloor = new int[36];

    }

    private void Start()
    {
        tParent.GetComponentsInChildren<Floor>(floors);


        for (int i = (floorHeight + 1); i >= 0; i--) 
        {
            for (int j = (floorWidth + 1); j >= 0; j--)
            {
                floorInfo[i, j] = chessArray.infoByTwo[i, j];
            }
        }

        for (int m = 0; m < 36; m++)
        {
            actureOfChessFloor[m] = chessArray.infoByOne[m];
        }

    }
    

    public Floor GetFloors(int _index)
    {
        return floors[_index];
    }

    bool GetFloorChecking(int _index)
    {
        bool check;

        if(actureOfChessFloor[_index] == 1)
        {
            check = true;
        }
        else
        {
            check = false;
        }

        Debug.Log(check);

        return check;
    }

    public void SetChessFloorTexture()
    {
        for(int i = 0; i < 36; i++)
        {
            mrFloorRenderer = floors[i].GetComponent<MeshRenderer>();

            if (GetFloorChecking(i))
            {
                //mFloorMaterial = mMaterialContainer[1];
                mrFloorRenderer.material.SetTexture("_MainTex", texTextureContainer[1]);
            }
            else
            {
                //mFloorMaterial = mMaterialContainer[0];
                mrFloorRenderer.material.SetTexture("_MainTex", texTextureContainer[0]);
            }
            //mrFloorRenderer.material = mFloorMaterial;
            
        }        
    }

}
