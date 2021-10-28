using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum InputDir
{
    right = 0,
    left,
    front,
    back,

}


public class TestPlayer : MonoBehaviour
{
    [SerializeField]  float moveSpeed; //이동속도
    [SerializeField] float rotateSpeed; //회전속도
    Vector3 moveDirection; //이동방향
    //float desiredRotationAngle; //회전 각
    bool isRun = false;

    CharacterController characterController;
    Transform CameraTransform;
    [SerializeField] Animator ani;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        //ani = GetComponentInChildren<Animator>();
        CameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        //float z = Input.GetAxisRaw("Vertical");
        Move();
        MoveTo(moveDirection);
    }

    void Move()
    {
        bool Move = false;

        if(Input.GetKey(KeyCode.LeftShift))
        {
            isRun = true;
        }
        else
        {
            isRun = false;
        }
       


        if (Input.GetKey(KeyCode.W))
        {
            moveDirection = GetDirection(InputDir.front);
            ani.SetBool("WalkFront", true);
            ani.SetBool("WalkBack", false);
        }
        else if(Input.GetKey(KeyCode.S))
        {
            moveDirection = GetDirection(InputDir.back);
            ani.SetBool("WalkFront", false);
            ani.SetBool("WalkBack", true);
            isRun = false;
        }
        else
        {
            moveDirection = Vector3.zero;
            ani.SetBool("WalkFront", false);
            ani.SetBool("WalkBack", false);
            isRun = false;

        }
        ani.SetBool("Run", isRun);
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(new Vector3(0, -rotateSpeed * Time.deltaTime, 0));
        }
        else if(Input.GetKey(KeyCode.D))
        {
            transform.Rotate(new Vector3(0, rotateSpeed * Time.deltaTime, 0));
        }


    }

    void MoveTo(Vector3 direction)
    {
        characterController.Move(direction * (isRun == false ? moveSpeed : moveSpeed * 2.3f) * Time.deltaTime);

    }



    //public void HandleMovementDirection(Vector3 direction)
    //{
    //    desiredRotationAngle = Vector3.Angle(transform.forward, direction);
    //    var crossProduct = Vector3.Cross(transform.forward, direction).y;
    //    if (crossProduct < 0)
    //    {
    //        desiredRotationAngle *= -1;
    //    }

    //}

    //void RotatePlayer_Rotation() //앞으로 갈때 플레이어 회전
    //{
    //    //if (desiredRotationAngle_Front > 10 || desiredRotationAngle_Front < -10)
    //        transform.Rotate(Vector3.up * desiredRotationAngle * rotateSpeed * Time.deltaTime);
    //}


    Vector3 GetDirection(InputDir _inputDir)
    {
        var cameraFowardDirection = CameraTransform.forward;
        Vector3 directionToMoveIn = Vector3.Scale(cameraFowardDirection, (Vector3.right + Vector3.forward));
        Debug.DrawRay(Camera.main.transform.position, cameraFowardDirection * 10, Color.red);
        Debug.DrawRay(Camera.main.transform.position, directionToMoveIn * 10, Color.blue);

        if(_inputDir == InputDir.front)
        {
            return directionToMoveIn;
        }
        else if(_inputDir == InputDir.back)
        {
            return -directionToMoveIn;
        }
        return directionToMoveIn;
    }
}
