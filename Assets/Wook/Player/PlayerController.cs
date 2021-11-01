using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerType
{//1P 2P
    FirstPlayer = 0,
    SecondPlayer, 
}


public class PlayerController : MonoBehaviour
{
    [SerializeField] PlayerType playerType;

    [SerializeField] float moveSpeed; //�̵��ӵ�
    [SerializeField] float rotateSpeed; //ȸ���ӵ�
    Vector3 moveDirection; //�̵�����
    bool isRun = false;

    CharacterController characterController;
    Transform CameraTransform;
    Animator ani;



    private void Awake()
    {
        
    }

}
