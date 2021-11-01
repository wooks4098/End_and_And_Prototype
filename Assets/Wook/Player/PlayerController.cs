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

    [SerializeField] float moveSpeed; //이동속도
    [SerializeField] float rotateSpeed; //회전속도
    Vector3 moveDirection; //이동방향
    bool isRun = false;

    CharacterController characterController;
    Transform CameraTransform;
    Animator ani;



    private void Awake()
    {
        
    }

}
