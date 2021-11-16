using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Direction
{
    None,
    Up,
    Down,
    Left,
    Right,
}


public class ChessPlayerController : MonoBehaviour
{
    // �׽�Ʈ�� ������ �Ϻδ� ���� ��


    [SerializeField] ChessManager chessManager;

    // ����
    [SerializeField] Direction dir;

    // �Է� ������ ������ �÷���
    [SerializeField] bool isMoving = false;

    // �����̴� �ӵ�, �Ÿ�
    [SerializeField] float moveSpeed = 4.0f;
    //[SerializeField] float moveDistance = 5.0f;

    Vector3 v3CurrentPosition;

    int currentFloorIndex = 33;
    [SerializeField] Floor currentFloor;


    private void Start()
    {
        currentFloor = chessManager.GetFloors(currentFloorIndex).GetComponent<Floor>();
        //v3CurrentPosition = transform.position;
    }

    private void Update()
    {
        if (!isMoving)
        {
            InputDirectionKey();
        }

        if (isMoving)
        {
            MoveToDirection();
        }
    }

    private void InputDirectionKey()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            dir = Direction.Up;
            isMoving = true;

            currentFloorIndex -= 6;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            dir = Direction.Down;
            isMoving = true;

            currentFloorIndex += 6;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            dir = Direction.Left;
            isMoving = true;

            currentFloorIndex -= 1;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            dir = Direction.Right;
            isMoving = true;

            currentFloorIndex += 1;
        }
        SelectFloor(currentFloorIndex);
    }

    void MoveToDirection()
    {
        switch(dir)
        {
            case Direction.Up:
            case Direction.Down:
                {
                    // �ε巯�� �̵��� ���� Mathf.MoveTowrads�� ��� 
                    // �̰��� ������ �հ� �ڷ� �̵��� �� ����ǹǷ� z���� ����Ѵ�.
                    // ť�갡 currentFloor (= ���� ������ �ٴ�)�� Ÿ������ �̵��Ѵ�. - �ӵ��� moveSpeed * time.deltaTime��ŭ
                    float newPositonZ = Mathf.MoveTowards(transform.position.z, currentFloor.transform.position.z, moveSpeed * Time.deltaTime);
                    
                    // Vector3�� ����Ͽ� ���ο� ��ǥ�� ������Ʈ.
                    transform.position = new Vector3(transform.position.x, transform.position.y, newPositonZ);


                    // �����ϸ� �÷��׸� false�� ����
                    if (Vector3.Distance(transform.position, currentFloor.transform.position) <= 0.5)
                    {
                        isMoving = false;
                    }
                }
                break;
            case Direction.Left:
            case Direction.Right:
                {
                    float newPositonX = Mathf.MoveTowards(transform.position.x, currentFloor.transform.position.x, moveSpeed * Time.deltaTime);
                    transform.position = new Vector3(newPositonX, transform.position.y, transform.position.z);


                    Debug.Log("newPositonX");

                    if (Vector3.Distance(currentFloor.transform.position, transform.position) <= 0.5)
                    {
                        isMoving = false;
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
