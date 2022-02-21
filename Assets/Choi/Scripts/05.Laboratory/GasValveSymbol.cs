using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GasValveSymbol : MonoBehaviour
{
    // References
    public GameObject goController;
    public GameObject goMovePlate;

    // Positions
    private int xBoard = -1;
    private int yBoard = -1;

    //Variable for keeping track of the player it belongs to "black" or "white"
    private string player;

    // 사용할 모든 스프라이트
    [SerializeField] Sprite[] symbols;

    public void Activate()
    {
        // 게임 컨트롤러 찾기
        goController = GameObject.Find("Gas Valve Controller");

        // 좌표 지정(계산)
        SetCoord();

        switch (this.name)
        {
            case "Symbol (1)":
                GetComponent<Image>().sprite = symbols[0];
                break;
            case "Symbol (2)":
                GetComponent<Image>().sprite = symbols[1];
                break;
            case "Symbol (3)":
                GetComponent<Image>().sprite = symbols[2];
                break;
            case "Symbol (4)":
                GetComponent<Image>().sprite = symbols[3];
                break;
            case "Symbol (5)":
                GetComponent<Image>().sprite = symbols[4];
                break;
        }
    }

    public void SetCoord()
    {
        //
        float x = xBoard;
        float y = yBoard;

        x *= 0.66f;
        y *= 0.66f;

        x += -2.3f;
        y += -2.3f;

        transform.position = new Vector3(x, y, -1.0f);
    }

    public int GetXBoard()
    {
        return xBoard;
    }
    public int GetYBoard()
    {
        return yBoard;
    }

    public void SetXBoard(int _x)
    {
        xBoard = _x;
    }
    public void SetYBoard(int _y)
    {
        yBoard = _y;
    }

    private void OnMouseUp()
    {
        DestroyMovePlates();

        InitiateMovePlates();
    }

    public void InitiateMovePlates()
    {
        LineMovePlate(1,0);
        LMovePlate();
        SurroundMovePlate();
        //PawnMovePlate(xBoard, yBoard - 1);
    }

    public void LineMovePlate(int _xIncrement, int _yIncrement)
    {
        GasValve valve = goController.GetComponent<GasValve>();

        int x = xBoard + _xIncrement;
        int y = yBoard + _yIncrement;

        while (valve.PositionOnBoard(x, y) && valve.GetPosition(x, y) == null)
        {
            //MovePlateSpawn(x, y);
            x += _xIncrement;
            y += _yIncrement;
        }

        if(valve.PositionOnBoard(x,y) && valve.GetPosition(x,y).GetComponent<GasValveSymbol>().player != player)
        {
            //MovePlateAttackSpawn(x, y);
        }
    }
    public void LMovePlate()
    {
        PointMovePlate(xBoard + 1, yBoard + 2);
        PointMovePlate(xBoard - 1, yBoard + 2);
    }
    public void SurroundMovePlate()
    {
        PointMovePlate(xBoard, yBoard + 2);
    }
    public void PointMovePlate(int _x, int _y)
    {
        GasValve gv = goController.GetComponent<GasValve>();
        if(gv.PositionOnBoard(_x,_y))
        {
            GameObject cp = gv.GetPosition(_x, _y);

            if(cp == null)
            {
                //MovePlateSpawn(_x, _y);
            }
            else if(cp.GetComponent<GasValveSymbol>().player != player)
            {

            }
        }
    }
    public void PawnMovePlate()
    {

    }

    public void DestroyMovePlates()
    {
        GameObject[] movePlates = GameObject.FindGameObjectsWithTag("MovePlate");
        for(int i = 0; i < movePlates.Length; i++)
        {
            Destroy(movePlates[i]);
        }
    }
}