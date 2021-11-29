using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// ��Ÿ�� or ����Ÿ�� �� UI�� �÷��̾� �̵���Ű�� ��ũ��
/// </summary>
/// 
enum SliderDirection
{
    Left = 0,
    Right,
}
public class ClimbWallPlayerMove : MonoBehaviour
{
    [SerializeField] Slider slider;
    CharacterController playerController;
    Animator ani;

    [SerializeField] bool isClimbing = false;//��Ÿ�⸦ �����ߴ���
    [SerializeField] SliderDirection dir;
    [SerializeField] float speed;
    private void Update()
    {
        SliderMove();
    }

    //�����̴� �¿�� �����̵���
    void SliderMove()
    {
        if(dir == SliderDirection.Right)
        {
            slider.value += speed * Time.deltaTime;
            if (slider.value >= slider.maxValue)
                dir = SliderDirection.Left;
        }
        else if(dir == SliderDirection.Left)
        {
            slider.value -= speed * Time.deltaTime;
            if (slider.value <= slider.minValue)
                dir = SliderDirection.Right;
        }
        
    }

    void TimingCheck(PlayerType _playerType, PlayerState _playerState)
    {

    }

    void SetPlaerComponent(PlayerType _playerType)
    {
        playerController = GameManager.Instance.GetPlayerTrans(_playerType).GetComponent<CharacterController>();
        ani = GameManager.Instance.GetPlayerModelTrans(_playerType).GetComponent<Animator>();
        switch (_playerType)
        {
            case PlayerType.FirstPlayer:
                InputManager.Instance.OnUsePlayer1 += TimingCheck;
                break;
            case PlayerType.SecondPlayer:
                InputManager.Instance.OnUsePlayer2 += TimingCheck;
                break;
        }
        dir = SliderDirection.Right;
    }
}
