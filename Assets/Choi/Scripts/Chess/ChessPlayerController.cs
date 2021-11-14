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
    // 테스트가 끝나면 일부는 감출 것


    [SerializeField] ChessManager chessManager;

    // 방향
    [SerializeField] DIRECTION direction = 0;

    // 입력 중인지 구분할 플래그
    [SerializeField] bool isMoving = false;

    // 움직이는 속도, 거리
    [SerializeField] float moveSpeed = 4.0f;
    [SerializeField] float moveDistance = 5.0f;

    Vector3 v3CurrentPosition;

    int currentFloorIndex = 3;
    [SerializeField] Floor currentFloor;


    private void Start()
    {
        currentFloor = chessManager.GetFloors(currentFloorIndex).GetComponent<Floor>();
        //v3CurrentPosition = transform.position;
    }

    private void Update()
    {
        InputDirectionKey();
    }

    private void InputDirectionKey()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            direction = DIRECTION.UP;
            isMoving = true;

            currentFloorIndex += 6;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            direction = DIRECTION.DOWN;
            isMoving = true;

            currentFloorIndex -= 6;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            direction = DIRECTION.LEFT;
            isMoving = true;

            currentFloorIndex -= 1;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            direction = DIRECTION.RIGHT;
            isMoving = true;

            currentFloorIndex += 1;
        }

        if(isMoving)
        {
            SelectFloor(currentFloorIndex);
            MoveToDirection();
        }
    }

    void MoveToDirection()
    {
        switch(direction)
        {
            case DIRECTION.UP:
            case DIRECTION.DOWN:
                {
                    if(isMoving)
                    {
                        float newPositonZ = Mathf.MoveTowards(transform.position.z, currentFloor.transform.position.z, moveSpeed * Time.deltaTime);
                        transform.position = new Vector3(transform.position.x, transform.position.y, newPositonZ);

                        if (transform.position == currentFloor.transform.position)
                        {
                            isMoving = false;
                        }
                    }
                }
                break;
            case DIRECTION.LEFT:
            case DIRECTION.RIGHT:
                {
                    if (isMoving)
                    {
                        float newPositonX = Mathf.MoveTowards(transform.position.x, currentFloor.transform.position.x, moveSpeed * Time.deltaTime);
                        transform.position = new Vector3(newPositonX, transform.position.y, transform.position.z);

                        if (transform.position == currentFloor.transform.position)
                        {
                            isMoving = false;
                        }
                    }
                }
                break;
            default:
                break;
        }
    }

    Floor SelectFloor(int _index)
    {
        currentFloor = chessManager.GetFloors(_index).GetComponent<Floor>();
        return currentFloor;
    }
}
