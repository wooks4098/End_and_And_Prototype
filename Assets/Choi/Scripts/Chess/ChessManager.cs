using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChessManager : MonoBehaviour
{
    // 체스 정보
    [SerializeField] ChessArrayInfo chessArray;
    // 바닥 정보를 저장할 1차원 배열
    [SerializeField] int[] actureOfChessFloor;

    // 바닥 정보를 저장할 2차원 배열
    int[][] floorInfo;

    // 읽기 전용 상수
    readonly int FloorWidth = 6; // 가로 개수
    readonly int FloorHeight = 6; // 세로 개수
    readonly int AllOfFloorCount = 36; // 전체 개수

    [SerializeField] float floorSize = 5f;
    [SerializeField] List<Floor> floorObejcts;
    
    [SerializeField] Transform tParent;
    //List<bool> checkAvailablefloors;

    // ==============================================
    // 텍스처를 바꿀 때 사용할 메시 렌더러
    [SerializeField] MeshRenderer mrFloorRenderer;
    // 텍스처를 저장하는 컨테이너
    [SerializeField] List<Texture> texTextureContainer;

    // 읽기 전용 상수형 
    readonly int rBasicTextureType = 0;     // 일반 타입 텍스쳐 (이동 불가능)
    readonly int rAvailableTextureType = 1; // 이동 가능 타입 텍스쳐
    // ==============================================


    private void Awake()
    {
        floorInfo = new int[8][];
        for (int i = 0; i < 8; i++)
        {
            floorInfo[i] = new int[8];
        }

        floorObejcts = new List<Floor>();
        //checkAvailablefloors = new List<bool>();

        actureOfChessFloor = new int[36];
    }

    private void Start()
    {
        tParent.GetComponentsInChildren<Floor>(floorObejcts);


        for (int i = (FloorHeight + 1); i >= 0; i--) 
        {
            for (int j = (FloorWidth + 1); j >= 0; j--)
            {
                floorInfo[i][j] = chessArray.infoByTwo[i][j];
            }
        }

        // infoByOne으로부터 정보를 받아와서 actureOfChessFloor에 할당 
        // AllOfFloorCount = 36
        for (int m = 0; m < AllOfFloorCount; m++)
        {
            actureOfChessFloor[m] = chessArray.infoByOne[m];
        }

    }
    

    public Floor GetFloors(int _index)
    {
        return floorObejcts[_index];
    }

    /// <summary>
    /// actureOfChessFloor 컨테이너가 가지고 있는 정보를 판별하고, 결과를 return한다.
    /// </summary>
    /// <param name="_index">현재 index에 해당하는 값이 1이면 올바른 길이므로, true를 반환한다.</param>
    /// <returns></returns>
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
        // GetFloorChecking() 메서드에 index를 전달하기 위한 for문
        // AllOfFloorCount = 36
        for (int i = 0; i < AllOfFloorCount; i++)
        {
            // floorObjects에 저장된 바닥(floor)로부터 메시렌더러를 받아온다
            mrFloorRenderer = floorObejcts[i].GetComponent<MeshRenderer>();

            if (GetFloorChecking(i))
            {
                // 텍스쳐를 변경한다 (rAvailableTextureType = 1, 올바른 길에 해당하는 텍스쳐)
                mrFloorRenderer.material.SetTexture("_MainTex", texTextureContainer[rAvailableTextureType]);
            }
            else
            {
                // 텍스쳐를 변경한다 (rBasicTextureType = 0, 일반 타입 텍스쳐 (올바른 길이 아님))
                mrFloorRenderer.material.SetTexture("_MainTex", texTextureContainer[rBasicTextureType]);
            }            
        }        
    }

}
