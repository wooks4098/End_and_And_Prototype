using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChessManager : MonoBehaviour
{
    [SerializeField] ChessPlayerController player;

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
    
    // floor�� ������ �����̳�
    [SerializeField] List<Floor> floorObejcts;
    
    // �θ�� ����� transform
    [SerializeField] Transform tParent;

    // ==============================================
    
    // �ؽ�ó�� �ٲ� �� ����� �޽� ������
    MeshRenderer mrFloorRenderer;
    // �ؽ�ó�� �����ϴ� �����̳�
    [SerializeField] List<Texture> texTextureContainer;

    // ==============================================
    
    // �б� ���� ����� 
    readonly int rBasicTextureType = 0;     // �Ϲ� Ÿ�� �ؽ��� (�̵� �Ұ���)
    readonly int rAvailableTextureType = 1; // �̵� ���� Ÿ�� �ؽ���

    // ��� - �ð�
    readonly float stayTime = 10f;
    // ���� �ð�
    float currentTime;
    // =============================================

    //public event Action<int> OnPlantThornEvent;


    private void Awake()
    {
        player = GameObject.Find("Player1").GetComponent<ChessPlayerController>();

        floorInfo = new int[8][];
        for (int i = 0; i < 8; i++)
        {
            floorInfo[i] = new int[8];
        }

        floorObejcts = new List<Floor>();

        actureOfChessFloor = new int[36];
    }

    private void OnEnable()
    {
        player.OnEnterWrongFloorEvent += ActivePlantThornOnEnterEvent;
        player.OnStayWrongFloorEvent += CreatePlantThorn;
        player.OnExitWrongFloorEvent += ResetCreateThornTimeOnExitEvent;
    }
    private void OnDisable()
    {
        player.OnEnterWrongFloorEvent -= ActivePlantThornOnEnterEvent;
        player.OnStayWrongFloorEvent -= CreatePlantThorn;
        player.OnExitWrongFloorEvent -= ResetCreateThornTimeOnExitEvent;
    }

    private void Start()
    {
        // �Ҵ�
        tParent.GetComponentsInChildren<Floor>(floorObejcts);

        /*
        for (int i = (FloorHeight + 1); i >= 0; i--)
        {
            for (int j = (FloorWidth + 1); j >= 0; j--)
            {
                floorInfo[i][j] = chessArray.infoByTwo[i][j];
            }
        }
        */

        // infoByOne���κ��� ������ �޾ƿͼ� actureOfChessFloor�� �Ҵ� 
        // AllOfFloorCount = 36
        for (int m = 0; m < AllOfFloorCount; m++)
        {
            actureOfChessFloor[m] = chessArray.infoByOne[m];
        }
    }

    void CreatePlantThorn(int _index)
    {
        StartCoroutine(CreatingTimeCoroutine(stayTime, _index));
    }

    IEnumerator CreatingTimeCoroutine(float _time, int _index)
    {
        currentTime = _time;

        while (currentTime > 1.0f)
        {
            currentTime -= Time.deltaTime;

            yield return new WaitForFixedUpdate();
        }

        while (currentTime <= 1.0f)
            ActivePlantThorn(_index);
    }

    private void ActivePlantThorn(int _index)
    {
        GameObject Thorn = floorObejcts[_index].GetPlantThorn();
        //Debug.Log(floorObejcts[_index]);

        if (!Thorn.activeSelf)
        {
            Debug.Log("Throw.Active");
            Thorn.SetActive(true);
        }
    }


    private void ActivePlantThornOnEnterEvent(int _index)
    {
        if (_index > 35 || _index < 0) return;

        Debug.Log("Throw.Active.Enter");

        GameObject Thorn = floorObejcts[_index].GetPlantThorn();

        if ((!Thorn.activeSelf)) 
        {
            Thorn.SetActive(true);
        }
    }

    private void ResetCreateThornTimeOnExitEvent(int _index)
    {
        Debug.Log("Throw.Exit");
        currentTime = stayTime;
    }

    public Floor GetFloorObjects(int _index)
    {
        return floorObejcts[_index];
    }

    public int GetIndexOfFloorObjects(Floor _floor)
    {
        int temp = floorObejcts.BinarySearch(_floor);

        return temp;
    }


    /// <summary>
    /// actureOfChessFloor �����̳ʰ� ������ �ִ� ������ �Ǻ��ϰ�, ����� return�Ѵ�.
    /// </summary>
    /// <param name="_index">���� index�� �ش��ϴ� ���� 1�̸� �ùٸ� ���̹Ƿ�, true�� ��ȯ�Ѵ�.</param>
    /// <returns></returns>
    public bool GetFloorChecking(int _index)
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

        //Debug.Log(check);

        return check;
    }


    /// <summary>
    /// GetFloorCheck()�� ������� �ؽ��ĸ� ����
    /// ��ȸ �� ���� �θ�
    /// </summary>
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
