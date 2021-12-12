using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 개별 UI를 관리하는 클래스
/// 예) Player1의 UI를 관리
/// </summary>
/// 
enum SliderDirection
{
    Left = 0,
    Right,
}
public class UiController : MonoBehaviour
{
    [SerializeField] PlayerType playerType;
    //오브젝트 UI용
    public Camera camera; //플레이어 카메라
    public RectTransform Canvas; //canvas recttransfrom
    public RectTransform image; //이미지 위치

    //StatusUI
    [Space][Space]
    [SerializeField] Slider HpSlider; //Hp 슬라이더
    [SerializeField] Text HpText;
    [SerializeField] Slider ThirstyWallSlider; //Thirsty 슬라이더


    //Climb Wall Move UI
    [Space] [Space]
    [SerializeField] bool isClimbWall = false; //벽타기 중인지
    [SerializeField] Slider climbWallSlider; //벽타기 슬라이더
    [SerializeField] SliderDirection dir;

    [SerializeField] float climbWallSliderSpeed; //슬리아더 속도
    [SerializeField] float minPos; //타겟 최소 위치
    [SerializeField] float maxPos; //타겟 최대 위치
    [SerializeField] RectTransform SliderTimingRect; //타겟 Rect

    [SerializeField] bool isHoldRope = false; //로프를 잡고있는 중인지
    [SerializeField] Slider sliderRopeHold; //로프 슬라이더
    [SerializeField] float sliderRopeHoldSpeed; //슬라이더 속도
    [SerializeField] float sliderRopeHoldAddValue; //슬라이더 증가값

    #region Status


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
            ChangeHpUi(70);
        if (Input.GetKeyDown(KeyCode.K))
            ChangeHpUi(40);
    }
    public void ChangeHpUi(float _hp)
    { //_hp : 변경된 체력

        HpText.text = _hp.ToString() + "%";

        StartCoroutine(StatusSliderValueChange(HpSlider, _hp));
    }


    //Status슬라이더 값 변경
    IEnumerator StatusSliderValueChange(Slider _slider, float _ReslutValue)
    {
        float FalltimeCheck = 0;
        float Falltime = 0.5f;
        float SliderValue = _slider.value;
        float ChangeValue;
        while (FalltimeCheck <= Falltime)
        {
            FalltimeCheck += Time.deltaTime;

            ChangeValue = Mathf.Lerp(SliderValue, _ReslutValue, FalltimeCheck / Falltime);
            _slider.value += ChangeValue - _slider.value;
            yield return null;
        }
    }


    #endregion

    #region ClimbWall

    public void StartClimbWall()
    {
        isClimbWall = true;
        climbWallSlider.gameObject.SetActive(true);
        climbWallSlider.value = 0;
        minPos = SliderTimingRect.anchoredPosition.x;
        maxPos = SliderTimingRect.sizeDelta.x + minPos;

        StartCoroutine(ClimbWallSliderMove());
    }
    public void EndClimbWall()
    {
        isClimbWall = false;
        climbWallSlider.gameObject.SetActive(false);
        
    }
    public bool isSliderTriggerCheck()
    {
        if (climbWallSlider.value >= minPos && climbWallSlider.value <= maxPos)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    IEnumerator ClimbWallSliderMove()
    {
        //Mathf.PingPong()
        while(isClimbWall == true)
        {
            if (dir == SliderDirection.Right)
            {
                climbWallSlider.value += climbWallSliderSpeed * Time.deltaTime;
                if (climbWallSlider.value >= climbWallSlider.maxValue)
                    dir = SliderDirection.Left;
            }
            else if (dir == SliderDirection.Left)
            {
                climbWallSlider.value -= climbWallSliderSpeed * Time.deltaTime;
                if (climbWallSlider.value <= climbWallSlider.minValue)
                    dir = SliderDirection.Right;
            }
            yield return null;
        }
    }

    #endregion


    #region Holding Rope

    public void StartHoldRope()
    {
        isHoldRope = true;
        sliderRopeHold.gameObject.SetActive(true);
        sliderRopeHold.value = sliderRopeHold.maxValue;
        StartCoroutine(HoldRopeSliderMove());
    }

    public void EndHoldRope()
    {
        isHoldRope = false;
        sliderRopeHold.gameObject.SetActive(false);
        GameManager.Instance.PlayerStateChange(playerType,PlayerState.ClimbWallFall);
        GameManager.Instance.GetPlayerController(playerType).HoldRopeFall();
    }

    //로프 잡기 슬라이더 움직임
    IEnumerator HoldRopeSliderMove()
    {
        while (isHoldRope == true)
        {
            sliderRopeHold.value -= Time.deltaTime * sliderRopeHoldSpeed;
            if (sliderRopeHold.value <= 0)
            {//로프잡고있기 실패
                EndHoldRope();
                //로프를 타고 올라오는 플레이어 떨어지도록 
                StartCoroutine(ClimbFallOtherPlayer());

            }
            yield return null;
        }
    }

    //다른 플레이어 벽에서 떨어지도록
    IEnumerator ClimbFallOtherPlayer()
    {
        yield return new WaitForSeconds(0.3f);
        GameManager.Instance.GetPlayerController(PlayerType.FirstPlayer == playerType ? PlayerType.SecondPlayer : playerType)
        .StartClimbWallFall();
    }

    //슬라이더 클릭시 Value 증가
    public void AddHoldRopeValue()
    {
        sliderRopeHold.value += sliderRopeHoldAddValue;
    }
    #endregion

    public void ObjectUIShow( )
    {
        image.gameObject.SetActive(true);
    }
    public void ObjectUIHide( )
    {
        image.gameObject.SetActive(false);
    }

    public void ObjectUIMove(Vector3 objPos)
    {
        var screenPos = camera.WorldToScreenPoint(objPos);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(Canvas, screenPos, camera, out Vector2 pos);
        image.localPosition = pos;
    }
}
