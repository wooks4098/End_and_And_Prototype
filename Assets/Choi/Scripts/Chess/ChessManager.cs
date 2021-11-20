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

    // floor �� ĭ ũ��
    [SerializeField] float floorSize = 5f;
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

    // �ð�
    readonly float triggerTime = 5f;
    // =============================================

    public event Action<int> OnPlantThornEvent;
    public event Action<int> ExitWrongFloorEvent;


    private void OnEnable()
    {
        player.OnWrongFloorEvent += CreatePlantThorn;
        ExitWrongFloorEvent += HidePlantThorn;
    }
    private void OnDisable()
    {
        player.OnWrongFloorEvent -= CreatePlantThorn;
        ExitWrongFloorEvent -= HidePlantThorn;
    }

    void CreatePlantThorn(int _index)
    {
        StartCoroutine(CreatingTimeCoroutine(triggerTime, _index));
    }

    IEnumerator CreatingTimeCoroutine(float _triggerTime, int _index)
    {
        //yield return triggerTime;

        while (_triggerTime > 1.0f)
        {
            _triggerTime -= Time.deltaTime;

            yield return new WaitForFixedUpdate();
        }

        ActivePlantThorn(_index);
    }

    private void ActivePlantThorn(int _index)
    {
        //Debug.Log("Trigger");
        //Debug.Log(_index);

        //Debug.Log("Throw");
        // ������ ����
        //GameObject go = Instantiate(goPlantThorn, transform.position, Quaternion.identity);
        //go.transform.parent = goParent.transform;

        GameObject Thorn = floorObejcts[_index].GetPlantThorn();
        //Debug.Log(floorObejcts[_index]);


        if (!Thorn.activeSelf)
        {
            Debug.Log("Throw.Active");
            Thorn.SetActive(true);
        }

        //OnPlantThornEvent(thornDamage);
    }

    private void HidePlantThorn(int _index)
    {
        GameObject Thorn = floorObejcts[_index].GetPlantThorn();

        if (Thorn.activeSelf)
        {
            Debug.Log("Throw.Hide");
            Thorn.SetActive(false);
        }
    }


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

    private void Start()
    {
        // �Ҵ�
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
