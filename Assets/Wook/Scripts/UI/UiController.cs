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

    //오브젝트 UI용
    public Camera camera; //플레이어 카메라
    public RectTransform Canvas; //canvas recttransfrom
    public RectTransform image; //이미지 위치

    //Climb Wall Move UI
    [Space] [Space]
    [SerializeField] bool isClimbWall = false; //벽타기 중인지
    [SerializeField] Slider climbWallSlider; //슬라이더
    [SerializeField] SliderDirection dir;

    [SerializeField] float sliderSpeed; //슬리아더 속도
    [SerializeField] float minPos; //타겟 최소 위치
    [SerializeField] float maxPos; //타겟 최대 위치
    [SerializeField] RectTransform SliderTimingRect; //타겟 Rect



    #region ClimbWall

    public void StartClimbWall()
    {
        isClimbWall = true;
        climbWallSlider.gameObject.SetActive(true);
        climbWallSlider.value = 0;
        minPos = SliderTimingRect.anchoredPosition.x;
        maxPos = SliderTimingRect.sizeDelta.x + minPos;

        StartCoroutine(SliderMove());
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
            Debug.Log("슬라이더 true");
            return true;
        }
        else
        {
            Debug.Log("슬라이더 false");
            return false;
        }
    }

    IEnumerator SliderMove()
    {
        while(isClimbWall == true)
        {
            if (dir == SliderDirection.Right)
            {
                climbWallSlider.value += sliderSpeed * Time.deltaTime;
                if (climbWallSlider.value >= climbWallSlider.maxValue)
                    dir = SliderDirection.Left;
            }
            else if (dir == SliderDirection.Left)
            {
                climbWallSlider.value -= sliderSpeed * Time.deltaTime;
                if (climbWallSlider.value <= climbWallSlider.minValue)
                    dir = SliderDirection.Right;
            }
            yield return null;
        }
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
