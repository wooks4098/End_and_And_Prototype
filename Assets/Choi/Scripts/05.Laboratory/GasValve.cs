using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasValve : MonoBehaviour
{
    [SerializeField] GameObject piece;

    private GameObject[,] positions = new GameObject[5, 5];
    private GameObject[] symbols = new GameObject[5];

    private int currentSymbol = 0; // 0~4, ÃÑ 5°³

    private void Start()
    {
        symbols = new GameObject[] { Create(0, 1, 2), Create(1, 4, 0), Create(2, 3, 1), Create(3, 2, 4), Create(4, 0, 3)};

        for (int i = 0; i < symbols.Length; i++)
        {
            SetPosition(symbols[i]);
        }
    }

    public GameObject Create(int _current, int _x, int _y)
    {
        GameObject obj = Instantiate(piece, new Vector3(0, 0, -1), Quaternion.identity);
        GasValveSymbol sm = obj.GetComponent<GasValveSymbol>();
        sm.name = name;

        sm.SetXBoard(_x);
        sm.SetYBoard(_y);
        sm.Activate();

        return obj;
    }

    public void SetPosition(GameObject _obj)
    {
        GasValveSymbol sm = _obj.GetComponent<GasValveSymbol>();

        positions[sm.GetXBoard(), sm.GetYBoard()] = _obj;
    }

    public void SetPositionEmpty(int _x, int _y)
    {
        positions[_x, _y] = null;
    }

    public GameObject GetPosition(int _x, int _y)
    {
        return positions[_x, _y];
    }

    public bool PositionOnBoard(int _x, int _y)
    {
        if( _x < 0 || _y < 0 || _x >= positions.GetLength(0) || _y >= positions.GetLength(1))
        {
            return false;
        }

        return true;
    }
}