using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChessManager : MonoBehaviour
{
    [SerializeField] private ChessPlayerController player;

    // ü�� ����
    [SerializeField] private ChessArrayInfo chessArray;
    // �ٴ� ������ ������ 1���� �迭
    [SerializeField] private int[] actureOfChessFloor;


    // �б� ���� ���
    // private readonly int FloorWidth = 6; // ���� ����
    // private readonly int FloorHeight = 6; // ���� ����
    private readonly int AllOfFloorCount = 36; // ��ü ����
    
    // floor�� ������ �����̳�
    [SerializeField] private List<Floor> floorObejcts;
    
    // �θ�� ����� transform
    [SerializeField] private Transform tParent;

    // ==============================================

    // �ؽ�ó�� �ٲ� �� ����� �޽� ������
    private MeshRenderer mrFloorRenderer;
    // �ؽ�ó�� �����ϴ� �����̳�
    [SerializeField] private List<Texture> texTextureContainer;

    // ==============================================

    // �б� ���� ����� 
    private readonly int rBasicTextureType = 0;     // �Ϲ� Ÿ�� �ؽ��� (�̵� �Ұ���)
    private readonly int rAvailableTextureType = 1; // �̵� ���� Ÿ�� �ؽ���

    // ��� - �ð�
    private readonly float stayTime = 10f;
    // ���� �ð�
    private float currentTime;
    // =============================================

    // �ڷ�ƾ�� ������ ����
    private Coroutine coroutine;
    // �ڷ�ƾ�� �������ΰ�? �� �Ǻ�
    private bool isCoroutineRunning = false;
    public bool IsCoroutineRunning { get { return isCoroutineRunning; } }



    private void Awake()
    {
        player = FindObjectOfType<ChessPlayerController>();

        floorObejcts = new List<Floor>();

        actureOfChessFloor = new int[36];
    }

    // �̺�Ʈ �Ҵ�, ����
    private void OnEnable()
    {
        player.OnEnterWrongFloorEvent += ActivePlantThorn;
        player.OnStayWrongFloorEvent += CreatePlantThorn;
        player.OnExitWrongFloorEvent += StopTimeCountCoroutine;
    }
    private void OnDisable()
    {
        player.OnEnterWrongFloorEvent -= ActivePlantThorn;
        player.OnStayWrongFloorEvent -= CreatePlantThorn;
        player.OnExitWrongFloorEvent -= StopTimeCountCoroutine;
    }

    private void Start()
    {
        // �Ҵ�
        tParent.GetComponentsInChildren<Floor>(floorObejcts);

        // infoByOne���κ��� ������ �޾ƿͼ� actureOfChessFloor�� �Ҵ� 
        // AllOfFloorCount = 36
        for (int m = 0; m < AllOfFloorCount; m++)
        {
            actureOfChessFloor[m] = chessArray.infoByOne[m];
        }
    }

    // �Ĺ� ���� ȣ��(����)
    void CreatePlantThorn(int _index)
    {
        Debug.Log("CreatePlantThorn");

        if(coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        coroutine = StartCoroutine(CreatingTimeCountCoroutine(stayTime, _index)); 
    }

    // ������ �Ĺ� ���ø� �����ϴ� �ڷ�ƾ 
    IEnumerator CreatingTimeCountCoroutine(float _time, int _index)
    {
        //Debug.Log("CreatingTimeCountCoroutine");

        // ���� �ڷ�ƾ�� ���ư����� üũ
        isCoroutineRunning = true;

        // ���ѷ���
        while (true)
        {
            // �Ĺ� ���ø� ������ �Ŀ�
            ActivePlantThorn(_index);

            // 10�ʸ� ��ٸ��� �Ѵ�
            yield return new WaitForSeconds(10f);
        }                 
    }

    /// <summary>
    /// �Ĺ� ���� ����
    /// </summary>
    /// <param name="_index"></param>
    private void ActivePlantThorn(int _index)
    {
        // �ε����� ������ ����� ����������
        if (_index > 35 || _index < 0) return;

        // �Ĺ� ������ ������ ���� �ٴ����κ��� �޾ƿ´�.
        GameObject thorn = floorObejcts[_index].GetPlantThorn();
        if (!thorn) return;

        // ���������� �Ҵ�.
        if (!thorn.activeSelf)
        {
            thorn.SetActive(true);
        }
    }

    /// <summary>
    /// �Ĺ� ���� ���� �ڷ�ƾ�� ������ ���߰��Ѵ�.
    /// </summary>
    /// <param name="_index"></param>
    private void StopTimeCountCoroutine(int _index)
    {
        // ���� �ڷ�ƾ�� ����ǰ� ������ 
        if(isCoroutineRunning)
        {
            // �ڷ�ƾ�� ������ �����.
            Debug.Log("StopCoroutine");
            StopCoroutine(coroutine);
        }
    }

    // ���� �ٴ� ������ ����
    public Floor GetFloorObjects(int _index)
    {
        return floorObejcts[_index];
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