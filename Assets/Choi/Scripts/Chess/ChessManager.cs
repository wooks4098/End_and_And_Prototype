using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChessManager : MonoBehaviour
{
    // ü�� ����
    [SerializeField] ChessArrayInfo chessArray;
    // �ٴ� ������ ������ 1���� �迭
    [SerializeField] int[] actureOfChessFloor;

    // �ٴ� ������ ������ 2���� �迭
    int[][] floorInfo;

    // �б� ���� ���
    readonly int FloorWidth = 6; // ���� ����
    readonly int FloorHeight = 6; // ���� ����
    readonly int AllOfFloorCount = 36; // ��ü ����

    [SerializeField] float floorSize = 5f;
    [SerializeField] List<Floor> floorObejcts;
    
    [SerializeField] Transform tParent;
    //List<bool> checkAvailablefloors;

    // ==============================================
    // �ؽ�ó�� �ٲ� �� ����� �޽� ������
    [SerializeField] MeshRenderer mrFloorRenderer;
    // �ؽ�ó�� �����ϴ� �����̳�
    [SerializeField] List<Texture> texTextureContainer;

    // �б� ���� ����� 
    readonly int rBasicTextureType = 0;     // �Ϲ� Ÿ�� �ؽ��� (�̵� �Ұ���)
    readonly int rAvailableTextureType = 1; // �̵� ���� Ÿ�� �ؽ���
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

        // infoByOne���κ��� ������ �޾ƿͼ� actureOfChessFloor�� �Ҵ� 
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
    /// actureOfChessFloor �����̳ʰ� ������ �ִ� ������ �Ǻ��ϰ�, ����� return�Ѵ�.
    /// </summary>
    /// <param name="_index">���� index�� �ش��ϴ� ���� 1�̸� �ùٸ� ���̹Ƿ�, true�� ��ȯ�Ѵ�.</param>
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
        // GetFloorChecking() �޼��忡 index�� �����ϱ� ���� for��
        // AllOfFloorCount = 36
        for (int i = 0; i < AllOfFloorCount; i++)
        {
            // floorObjects�� ����� �ٴ�(floor)�κ��� �޽÷������� �޾ƿ´�
            mrFloorRenderer = floorObejcts[i].GetComponent<MeshRenderer>();

            if (GetFloorChecking(i))
            {
                // �ؽ��ĸ� �����Ѵ� (rAvailableTextureType = 1, �ùٸ� �濡 �ش��ϴ� �ؽ���)
                mrFloorRenderer.material.SetTexture("_MainTex", texTextureContainer[rAvailableTextureType]);
            }
            else
            {
                // �ؽ��ĸ� �����Ѵ� (rBasicTextureType = 0, �Ϲ� Ÿ�� �ؽ��� (�ùٸ� ���� �ƴ�))
                mrFloorRenderer.material.SetTexture("_MainTex", texTextureContainer[rBasicTextureType]);
            }            
        }        
    }

}
