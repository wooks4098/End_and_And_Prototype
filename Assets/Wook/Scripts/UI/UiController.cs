using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// ���� UI�� �����ϴ� Ŭ����
/// ��) Player1�� UI�� ����
/// </summary>
/// 
enum SliderDirection
{
    Left = 0,
    Right,
}
public class UiController : MonoBehaviour
{

    //������Ʈ UI��
    public Camera camera; //�÷��̾� ī�޶�
    public RectTransform Canvas; //canvas recttransfrom
    public RectTransform image; //�̹��� ��ġ

    //Climb Wall Move UI
    [Space] [Space]
    [SerializeField] bool isClimbWall = false; //��Ÿ�� ������
    [SerializeField] Slider climbWallSlider; //�����̴�
    [SerializeField] SliderDirection dir;

    [SerializeField] float sliderSpeed; //�����ƴ� �ӵ�
    [SerializeField] float minPos; //Ÿ�� �ּ� ��ġ
    [SerializeField] float maxPos; //Ÿ�� �ִ� ��ġ
    [SerializeField] RectTransform SliderTimingRect; //Ÿ�� Rect



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
            Debug.Log("�����̴� true");
            return true;
        }
        else
        {
            Debug.Log("�����̴� false");
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
