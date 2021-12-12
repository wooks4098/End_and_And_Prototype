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
    [SerializeField] PlayerType playerType;
    //������Ʈ UI��
    public Camera camera; //�÷��̾� ī�޶�
    public RectTransform Canvas; //canvas recttransfrom
    public RectTransform image; //�̹��� ��ġ

    //StatusUI
    [Space][Space]
    [SerializeField] Slider HpSlider; //Hp �����̴�
    [SerializeField] Text HpText;
    [SerializeField] Slider ThirstyWallSlider; //Thirsty �����̴�


    //Climb Wall Move UI
    [Space] [Space]
    [SerializeField] bool isClimbWall = false; //��Ÿ�� ������
    [SerializeField] Slider climbWallSlider; //��Ÿ�� �����̴�
    [SerializeField] SliderDirection dir;

    [SerializeField] float climbWallSliderSpeed; //�����ƴ� �ӵ�
    [SerializeField] float minPos; //Ÿ�� �ּ� ��ġ
    [SerializeField] float maxPos; //Ÿ�� �ִ� ��ġ
    [SerializeField] RectTransform SliderTimingRect; //Ÿ�� Rect

    [SerializeField] bool isHoldRope = false; //������ ����ִ� ������
    [SerializeField] Slider sliderRopeHold; //���� �����̴�
    [SerializeField] float sliderRopeHoldSpeed; //�����̴� �ӵ�
    [SerializeField] float sliderRopeHoldAddValue; //�����̴� ������

    #region Status


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
            ChangeHpUi(70);
        if (Input.GetKeyDown(KeyCode.K))
            ChangeHpUi(40);
    }
    public void ChangeHpUi(float _hp)
    { //_hp : ����� ü��

        HpText.text = _hp.ToString() + "%";

        StartCoroutine(StatusSliderValueChange(HpSlider, _hp));
    }


    //Status�����̴� �� ����
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

    //���� ��� �����̴� ������
    IEnumerator HoldRopeSliderMove()
    {
        while (isHoldRope == true)
        {
            sliderRopeHold.value -= Time.deltaTime * sliderRopeHoldSpeed;
            if (sliderRopeHold.value <= 0)
            {//��������ֱ� ����
                EndHoldRope();
                //������ Ÿ�� �ö���� �÷��̾� ���������� 
                StartCoroutine(ClimbFallOtherPlayer());

            }
            yield return null;
        }
    }

    //�ٸ� �÷��̾� ������ ����������
    IEnumerator ClimbFallOtherPlayer()
    {
        yield return new WaitForSeconds(0.3f);
        GameManager.Instance.GetPlayerController(PlayerType.FirstPlayer == playerType ? PlayerType.SecondPlayer : playerType)
        .StartClimbWallFall();
    }

    //�����̴� Ŭ���� Value ����
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
