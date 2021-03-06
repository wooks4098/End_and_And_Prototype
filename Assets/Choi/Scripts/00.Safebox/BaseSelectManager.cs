using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BaseSelectManager : MonoBehaviour
{
    [SerializeField] protected int currentIndex;
    [SerializeField] protected GameObject currentPiece = null;

    [SerializeField] protected SafeboxManager safeboxManager;

    // 접근할 수 있는지를 체크하는 list
    // true = 접근 가능][false = 접근 불가능  
    [SerializeField] protected List<bool> availableList;


    // 줌인 카메라
    [SerializeField] Camera zoomInCamera;

    // 보일 ui
    [SerializeField] protected ObjectUIShow objectUiShow;

    // 금고에 입장할 때 실행되는 이벤트
    public UnityEvent OnTriggerEnterSafeBox;

    // 카메라가 활성 상태인지 체크
    protected bool isZoomInCameraActive = false;


    protected void Awake()
    {
        availableList = new List<bool>();
    }

    protected void Start()
    {
        // 현재 선택된 조각 할당
        currentPiece = transform.GetChild(currentIndex).gameObject;

        // 전처리 1.활성화 2.인덱스할당
        CheckAvailable();
        
        // 시작할 인덱스를 선택하는 메서드
        // 아웃라인에 진입할 때 켜기로 했으므로 지금은 끔.
        // SetStartIndex();

        // 줌인 카메라 끄기
        if (zoomInCamera.gameObject.activeSelf == true)
        {
            zoomInCamera.gameObject.SetActive(false);
        }

        //inputManger 등록
        SetInputKey();
    }

    protected void OpenSafeBox()
    {
        if (zoomInCamera.gameObject.activeSelf == false)
        {
            zoomInCamera.gameObject.SetActive(true);
        }
    }

    public void CloseSafeBox()
    {
        // 카메라 끄기
        if (zoomInCamera.gameObject.activeSelf == true)
        {
            zoomInCamera.gameObject.SetActive(false);
        }   
    }

    public void SetStartIndex()
    {
        MoveOnNext();
    }

    public abstract void CheckAvailable();
    protected abstract void InputMoveKey(MoveType _moveType, PlayerState _playerState);

    protected void MoveOnPrev()
    {
        if (currentPiece == null) return;

        // 현재 piece (= 금고 cube 중 1개)에서 <Outline> 컴포넌트를 가져와 비활성화
        currentPiece.GetComponent<Outline>().enabled = false;

        // 현재 인덱스를 감소시킨다
        // 왼쪽으로 이동 = 인덱스 1만큼 감소
        //currentIndex--;
        CalculateMoveOnPrev();


        // 현재 piece를 변경한 인덱스의 것으로 교체한다.
        currentPiece = transform.GetChild(currentIndex).gameObject;

        // 새로운 piece에서 <Outline> 컴포넌트를 가져와 활성화
        currentPiece.GetComponent<Outline>().enabled = true;
    }

    protected void MoveOnNext()
    {
        if (currentPiece == null) return;

        // 현재 piece (= 금고 cube 중 1개)에서 <Outline> 컴포넌트를 가져와 비활성화
        currentPiece.GetComponent<Outline>().enabled = false;

        // 현재 인덱스를 증가시킨다
        // 오른쪽으로 이동 = 인덱스 1만큼 증가
        //currentIndex++;
        CalculateMoveOnNext();


        // 현재 piece를 변경한 인덱스의 것으로 교체한다.
        currentPiece = transform.GetChild(currentIndex).gameObject;

        // 새로운 piece에서 <Outline> 컴포넌트를 가져와 활성화
        currentPiece.GetComponent<Outline>().enabled = true;
    }

    protected abstract void InputSelectKey(PlayerType _playerType, PlayerState _playerState);

    protected void CalculateMoveOnPrev()
    {
        currentIndex = currentIndex - 1;

        // 범위를 벗어났을 경우를 대비한 예외처리
        if (currentIndex < 0)
        {
            currentIndex = safeboxManager.Origin.Count - 1;
        }

        // availableList == false이면 지나갈 수 없으니
        if (availableList[currentIndex] == false)
        {
            // 재귀를 통해 인덱스를 다시 한 번 감소시킨다.
            CalculateMoveOnPrev();
        }
    }

    protected void CalculateMoveOnNext()
    {
        currentIndex = currentIndex + 1;

        // 범위를 벗어났을 경우를 대비한 예외처리
        if (currentIndex > safeboxManager.Origin.Count - 1)
        {
            currentIndex = 0;
        }

        // availableList == false이면 지나갈 수 없으니
        if (availableList[currentIndex] == false)
        {
            // 재귀를 통해 인덱스를 다시 한 번 증가시킨다.
            CalculateMoveOnNext();
        }
    }

    protected abstract void InputActiveKey(PlayerType _playerType, PlayerState _playerState);

    //inputManager에 등록
    protected abstract void SetInputKey();
}
