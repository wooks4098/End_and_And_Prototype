using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovePlate : MonoBehaviour
{
    public GameObject controller;

    GameObject reference = null;

    // Board Positions, not world Positions
    private int matrixX;
    private int matrixY;

    // 블록(문양) 인덱스
    private int index;

    // false: 블록 선택, true: 블록 이동 가능
    public bool isConfirm = false;

    public void Start()
    {
        
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.D))
        {
            index++;
            if(index > 5)
            {
                index = 0;
            }
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.A))
        {
            index--;
            if(index < 0)
            {
                index = 5;
            }
        }
    }
    
    private void ChangeColor()
    {
        // 색상 변경
        if (isConfirm)
        {
            // Change to red
            gameObject.GetComponent<Image>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        }
        else
        {
            gameObject.GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
        }
    }
       

    public void SetCoords(int _x, int _y)
    {
        matrixX = _x;
        matrixY = _y;
    }

    public void SetReference(GameObject _obj)
    {
        reference = _obj;
    }

    public GameObject GetReference()
    {
        return reference;
    }
}
