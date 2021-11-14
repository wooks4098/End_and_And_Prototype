using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum DIRECTION
{
    NONE,
    UP,
    DOWN,
    LEFT,
    RIGHT,
}


public class ChessPlayerController : MonoBehaviour
{
    [SerializeField] ChessManager chessManager;    

    // 방향
    DIRECTION direction = 0;

    // 입력 중인지 구분할 플래그
    bool isMoving = false;

    // 움직이는 속도, 거리
    [SerializeField] float moveSpeed = 1.0f;
    [SerializeField] float moveDistance = 5.0f;

    Vector3 currentPosition;

    int currentIndexH;
    int currentIndexW;


    public event Action onInputDirectionKey;


    private void OnEnable()
    {
        onInputDirectionKey += CalculatePosition;
        //onInputDirectionKey += SetCurrentPosition;
    }
    private void OnDisable()
    {
        onInputDirectionKey -= CalculatePosition;
        //onInputDirectionKey -= SetCurrentPosition;
    }

    private void Start()
    {
        currentPosition = transform.position;
    }

    private void Update()
    {
        InputDirectionKey();

        //isMoving = true;
    }

    private void InputDirectionKey()
    {
        if (Input.GetKey(KeyCode.W))
        {
            direction = DIRECTION.UP;
            onInputDirectionKey();
        }
        else if (Input.GetKey(KeyCode.A))
        {
            direction = DIRECTION.LEFT;
            onInputDirectionKey();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            direction = DIRECTION.DOWN;
            onInputDirectionKey();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            direction = DIRECTION.RIGHT;
            onInputDirectionKey();
        }
    }

    void CalculatePosition()
    {
        switch (direction)
        {
            case DIRECTION.UP:
                {
                    transform.position = Vector3.Lerp(transform.position,
                        new Vector3(currentPosition.x, currentPosition.y, currentPosition.z + moveDistance),
                        0.01f);

                    break;
                }
            case DIRECTION.DOWN:
                {
                    transform.position = Vector3.Lerp(transform.position,
                        new Vector3(currentPosition.x, currentPosition.y, currentPosition.z - moveDistance),
                        0.01f);

                    break;
                }
            case DIRECTION.LEFT:
                {
                    transform.position = Vector3.Lerp(transform.position,
                        new Vector3(currentPosition.x - moveDistance, currentPosition.y, currentPosition.z),
                        0.01f);

                    break;
                }
            case DIRECTION.RIGHT:
                {
                    transform.position = Vector3.Lerp(transform.position,
                        new Vector3(currentPosition.x + moveDistance, currentPosition.y, currentPosition.z),
                        0.01f);

                    break;
                }
            default:
                {
                    break;
                }
        }

        //currentPosition = transform.position;
    }




    public Vector3 CaculateFloorSize(int indexH, int indexW, Transform transform)
    {

        switch(direction)
        {
            case DIRECTION.UP:
                {

                    break;
                }
            case DIRECTION.DOWN:
                {

                    break;
                }
            case DIRECTION.LEFT:
                {

                    break;
                }
            case DIRECTION.RIGHT:
                {

                    break;
                }
            default:
                {
                    break;
                }
        }

        return transform.position;
    }

}
