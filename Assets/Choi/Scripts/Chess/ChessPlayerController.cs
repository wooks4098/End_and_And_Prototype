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
    //[SerializeField] float moveDistance = 5.0f;

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

        if (isMoving)
        {
            SelectFloor(currentFloorIndex);
            MoveToDirection();
        }
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
                        // 부드러운 이동을 위해 Mathf.MoveTowrads를 사용 
                        // 이것은 지금은 앞과 뒤로 이동할 때 실행되므로 z값을 계산한다.
                        // 큐브가 currentFloor (= 현재 지정된 바닥)을 타겟으로 이동한다. - 속도는 moveSpeed * time.deltaTime만큼
                        float newPositonZ = Mathf.MoveTowards(transform.position.z, currentFloor.transform.position.z, moveSpeed * Time.deltaTime);

                        // Vector3를 사용하여 새로운 좌표로 업데이트.
                        transform.position = new Vector3(transform.position.x, transform.position.y, newPositonZ);

                        // 도착하면 플래그를 false로 변경
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

    void SelectFloor(int _index)
    {
        currentFloor = chessManager.GetFloors(_index).GetComponent<Floor>();
    }
}
