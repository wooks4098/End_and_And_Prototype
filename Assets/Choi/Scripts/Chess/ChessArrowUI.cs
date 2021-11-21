using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChessArrowUI : MonoBehaviour
{
    GameObject goUIArrowParent;
    Transform player;
    [SerializeField] Camera cTopDownCamera;

    [SerializeField] List<GameObject> goArrows;

    // 상수
    readonly int upArrowIndex = 0;
    readonly int leftArrowIndex = 1;
    readonly int downArrowIndex = 2;
    readonly int rightArrowIndex = 3;

    private void Awake() 
    {
        goUIArrowParent = transform.GetChild(0).gameObject;
        player = GameObject.FindGameObjectWithTag("Player").transform;

        goArrows = new List<GameObject>();

        for(int i = 0; i < 4; i++)
        {
            goArrows.Add(goUIArrowParent.transform.GetChild(i).gameObject);
        }
    }

    private void Start()
    {
        SetArrowAlpha(goArrows[upArrowIndex]);
        SetArrowAlpha(goArrows[leftArrowIndex], 0.3f);
        SetArrowAlpha(goArrows[downArrowIndex], 0.3f);
        SetArrowAlpha(goArrows[rightArrowIndex], 0.3f);
    }

    void Update()
    {
        // 오브젝트에 따른 HP Bar 위치 이동
        Vector3 screenPos = cTopDownCamera.WorldToScreenPoint(player.position);
        float x = screenPos.x;

        goUIArrowParent.transform.position = new Vector3(x, screenPos.y, goUIArrowParent.transform.position.z);


        if(player.GetComponent<ChessPlayerController>().IsMoving)
        {
            HideArrows();
        }
        else if (!player.GetComponent<ChessPlayerController>().IsMoving)
        {
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
    /// </summary>
    private void CheckCurrentFloorIndexForArrowUI()
    {
        int tempIndex = player.GetComponent<ChessPlayerController>().GetCurrentFloorIndex();

        // 위로 이동 불가
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

    private void ActiveArrows()
    {
        Debug.Log("ActiveArrows");

        // 켠다
        goUIArrowParent.SetActive(true);

        // 인덱스를 판별해서 UI alpha 값 조절
        CheckCurrentFloorIndexForArrowUI();
    }
}
