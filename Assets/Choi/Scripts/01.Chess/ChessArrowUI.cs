using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChessArrowUI : MonoBehaviour
{
    // 화살표 ui의 부모를 받아올 변수
    // 왜냐하면 화살표를 하나씩 active false 할 게 아니라,
    // 화살표를 하나로 묶은 부모 자체를 active false 할 예정이기 때문이다.
    GameObject goUIArrowParent;
    Transform player;

    // UI를 그릴 기준이 되는 탑뷰 카메라.
    [SerializeField] Camera cTopDownCamera;

    // 화살표 ui 4개를 저장할 리스트
    [SerializeField] List<GameObject> goArrows;

    // 상수
    readonly int upArrowIndex = 0; // 위쪽 화살표
    readonly int leftArrowIndex = 1; // 왼쪽 화살표
    readonly int downArrowIndex = 2; // 아래쪽 화살표
    readonly int rightArrowIndex = 3; // 오른쪽 화살표

    private void Awake() 
    {
        // 화살표 UI의 부모는 현재 스크립트가 있는 오브젝트의 자식으로부터 받아온다
        goUIArrowParent = transform.GetChild(0).gameObject;

        player = GameObject.FindGameObjectWithTag("Player").transform;

        // 화살표 ui를 담을 리스트를 초기화하고
        goArrows = new List<GameObject>();

        // for문을 돌며 4개의 화살표를 리스트에 추가한다.
        for(int i = 0; i < 4; i++)
        {
            // 앞에서 받아온 goUIArrow의 자식 위치에 화살표 ui가 있다.
            goArrows.Add(goUIArrowParent.transform.GetChild(i).gameObject);
        }
    }

    private void Start()
    {
        // 시작지점 = starting floor이므로
        // 화살표 ui의 알파값을 아래와 같이 조절한다.
        SetArrowAlpha(goArrows[upArrowIndex]);
        SetArrowAlpha(goArrows[leftArrowIndex], 0.3f);
        SetArrowAlpha(goArrows[downArrowIndex], 0.3f);
        SetArrowAlpha(goArrows[rightArrowIndex], 0.3f);
    }

    void Update()
    {
        // 오브젝트에 따른 화살표 ui의 위치 이동
        // 플레이어의 좌표를 카메라로 부터 받아온다.
        Vector3 screenPos = cTopDownCamera.WorldToScreenPoint(player.position);
        float x = screenPos.x;

        // ui 부모의 위치를 설정한다 (화살표 ui를 하나씩 움직이면 번거로우므로)
        goUIArrowParent.transform.position = new Vector3(x, screenPos.y, goUIArrowParent.transform.position.z);

        // ChessPlayerController.isMoving == true 이면, (움직이고 있으면)
        if (player.GetComponent<ChessPlayerController>().IsMoving)
        {
            // 화살표를 숨긴다
            HideArrows();
        }
        // ChessPlayerController.isMoving == false 이면, (움직이고 있지 않으면)
        else if (!player.GetComponent<ChessPlayerController>().IsMoving)
        {
            // 화살표를 표시한다
            ActiveArrows();
        }
    }

    /// <summary>
    /// 화살표의 alpha 값을 바꾸는 메서드
    /// </summary>
    /// <param name="_arrow">선택된 arrow object</param>
    /// <param name="_alpha">변경할 알파 값의 양</param>
    public void SetArrowAlpha(GameObject _arrow, float _alpha = 1f)
    {
        Image image = _arrow.GetComponent<Image>();
        var imageColor = image.color;
        imageColor.a = _alpha;

        _arrow.GetComponent<Image>().color = imageColor;
    }
    
    /// <summary>
    /// 현재 인덱스를 판별 -> 이를 기반으로 ui 조절
    /// 특정 방향으로 이동이 불가할 경우,
    /// 해당 방향 화살표 ui의 알파 값이 0.3f로 변경된다.
    /// </summary>
    private void CheckCurrentFloorIndexForArrowUI()
    {
        // 현재 인덱스를 ChessPlayerController로부터 받아온다
        int tempIndex = player.GetComponent<ChessPlayerController>().GetCurrentFloorIndex();

        // 위로 이동 불가
        // 0은 골인지점으로 가야하기 때문에 예외처리한다.
        if (tempIndex <= 5 && tempIndex != 0)
        {
            SetArrowAlpha(goArrows[upArrowIndex], 0.3f);            
        }
        // 아래로 이동 불가
        else if (tempIndex >= 30)
        {
            SetArrowAlpha(goArrows[downArrowIndex], 0.3f);
        }
        // 왼쪽으로 이동 불가
        else if ((tempIndex % 6) == 0)
        {
            SetArrowAlpha(goArrows[leftArrowIndex], 0.3f);
        }
        //오른쪽으로 이동 불가
        else if ((tempIndex % 6) == 5)
        {
            SetArrowAlpha(goArrows[rightArrowIndex], 0.3f);
        }

        // 왼쪽 위 = 도착지점 직전 = 이동할 수 있어야 함.
        // => 제외

        // 오른쪽 위로 이동 불가
        if (tempIndex <= 5 && (tempIndex % 6) == 5)
        {
            SetArrowAlpha(goArrows[upArrowIndex], 0.3f);
            SetArrowAlpha(goArrows[rightArrowIndex], 0.3f);
        }
        // 왼쪽 아래로 이동 불가
        else if (tempIndex >= 30 && (tempIndex % 6) == 0)
        {
            SetArrowAlpha(goArrows[downArrowIndex], 0.3f);
            SetArrowAlpha(goArrows[leftArrowIndex], 0.3f);
        }
        // 오른쪽 아래로 이동 불가
        else if (tempIndex >= 30 && (tempIndex % 6) == 5)
        {
            SetArrowAlpha(goArrows[downArrowIndex], 0.3f);
            SetArrowAlpha(goArrows[rightArrowIndex], 0.3f);
        }

        // 시작지점
        if(tempIndex > 35)
        {
            SetArrowAlpha(goArrows[downArrowIndex], 0.3f);
            SetArrowAlpha(goArrows[leftArrowIndex], 0.3f);
            SetArrowAlpha(goArrows[rightArrowIndex], 0.3f);
        }        
        // 끝지점
        else if(tempIndex < 0)
        {
            SetArrowAlpha(goArrows[upArrowIndex], 0.3f);
            SetArrowAlpha(goArrows[leftArrowIndex], 0.3f);
            SetArrowAlpha(goArrows[rightArrowIndex], 0.3f);
        }
    }

    /// <summary>
    /// 플레이어 이동 중에는 화살표를 숨긴다.
    /// 숨길 때 모든 알파값을 1로 설정한다.
    /// </summary>
    private void HideArrows()
    {
        // alpha값을 전부 1로 변경하고
        SetArrowAlpha(goArrows[upArrowIndex]);
        SetArrowAlpha(goArrows[leftArrowIndex]);
        SetArrowAlpha(goArrows[downArrowIndex]);
        SetArrowAlpha(goArrows[rightArrowIndex]);

        // 끈다
        goUIArrowParent.SetActive(false);
    }

    /// <summary>
    /// 플레이어 이동이 멈추면 화살표가 나타난다.
    /// 화살표가 나타남과 동시에 알파값을 조절한다.
    /// </summary>
    private void ActiveArrows()
    {
        //Debug.Log("ActiveArrows");

        // 켠다
        goUIArrowParent.SetActive(true);

        // 인덱스를 판별해서 UI alpha 값 조절
        CheckCurrentFloorIndexForArrowUI();
    }
}
