using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChessManager : MonoBehaviour
{
    [SerializeField] private ChessPlayerController player;

    // 체스 정보
    [SerializeField] private ChessArrayInfo chessArray;
    // 바닥 정보를 저장할 1차원 배열
    [SerializeField] private int[] actureOfChessFloor;


    // 읽기 전용 상수
    // private readonly int FloorWidth = 6; // 가로 개수
    // private readonly int FloorHeight = 6; // 세로 개수
    private readonly int AllOfFloorCount = 36; // 전체 개수
    
    // floor를 저장할 컨테이너
    [SerializeField] private List<Floor> floorObejcts;
    
    // 부모로 사용할 transform
    [SerializeField] private Transform tParent;

    // ==============================================

    // 텍스처를 바꿀 때 사용할 메시 렌더러
    private MeshRenderer mrFloorRenderer;
    // 텍스처를 저장하는 컨테이너
    [SerializeField] private List<Texture> texTextureContainer;

    // ==============================================

    // 읽기 전용 상수형 
    private readonly int rBasicTextureType = 0;     // 일반 타입 텍스쳐 (이동 불가능)
    private readonly int rAvailableTextureType = 1; // 이동 가능 타입 텍스쳐

    // 상수 - 시간
    private readonly float stayTime = 10f;
    // 현재 시간
    private float currentTime;
    // =============================================

    // 코루틴을 저장할 변수
    private Coroutine coroutine;
    // 코루틴이 실행중인가? 를 판별
    private bool isCoroutineRunning = false;
    public bool IsCoroutineRunning { get { return isCoroutineRunning; } }



    private void Awake()
    {
        player = GameObject.Find("Player1").GetComponent<ChessPlayerController>();

        floorObejcts = new List<Floor>();

        actureOfChessFloor = new int[36];
    }

    // 이벤트 할당, 해제
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
        // 할당
        tParent.GetComponentsInChildren<Floor>(floorObejcts);

        // infoByOne으로부터 정보를 받아와서 actureOfChessFloor에 할당 
        // AllOfFloorCount = 36
        for (int m = 0; m < AllOfFloorCount; m++)
        {
            actureOfChessFloor[m] = chessArray.infoByOne[m];
        }
    }

    // 식물 가시 호출(생성)
    void CreatePlantThorn(int _index)
    {
        Debug.Log("CreatePlantThorn");

        if(coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        coroutine = StartCoroutine(CreatingTimeCountCoroutine(stayTime, _index)); 
    }

    // 실제로 식물 가시를 생성하는 코루틴 
    IEnumerator CreatingTimeCountCoroutine(float _time, int _index)
    {
        //Debug.Log("CreatingTimeCountCoroutine");

        // 현재 코루틴이 돌아가는지 체크
        isCoroutineRunning = true;

        // 무한루프
        while (true)
        {
            // 식물 가시를 생성한 후에
            ActivePlantThorn(_index);

            // 10초를 기다리게 한다
            yield return new WaitForSeconds(10f);
        }                 
    }

    /// <summary>
    /// 식물 가시 생성
    /// </summary>
    /// <param name="_index"></param>
    private void ActivePlantThorn(int _index)
    {
        // 인덱스가 범위를 벗어나면 빠져나간다
        if (_index > 35 || _index < 0) return;

        // 식물 가시의 정보를 현재 바닥으로부터 받아온다.
        GameObject thorn = floorObejcts[_index].GetPlantThorn();
        if (!thorn) return;

        // 꺼져있으면 켠다.
        if (!thorn.activeSelf)
        {
            thorn.SetActive(true);
        }
    }

    /// <summary>
    /// 식물 가시 생성 코루틴을 강제로 멈추게한다.
    /// </summary>
    /// <param name="_index"></param>
    private void StopTimeCountCoroutine(int _index)
    {
        // 만약 코루틴이 실행되고 있으면 
        if(isCoroutineRunning)
        {
            // 코루틴을 강제로 멈춘다.
            Debug.Log("StopCoroutine");
            StopCoroutine(coroutine);
        }
    }

    // 현재 바닥 정보에 접근
    public Floor GetFloorObjects(int _index)
    {
        return floorObejcts[_index];
    }


    /// <summary>
    /// actureOfChessFloor 컨테이너가 가지고 있는 정보를 판별하고, 결과를 return한다.
    /// </summary>
    /// <param name="_index">현재 index에 해당하는 값이 1이면 올바른 길이므로, true를 반환한다.</param>
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
    /// GetFloorCheck()을 기반으로 텍스쳐를 설정
    /// 초회 한 번만 부름
    /// </summary>
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
