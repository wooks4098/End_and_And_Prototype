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

    // 사용할 모든 스프라이트
    [SerializeField] Sprite[] symbols;


    public void Activate()
    {
        // 게임 컨트롤러 찾기
        goController = GameObject.Find("Gas Valve Controller");

        // 좌표 지정
        SetCoordinate();

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

    public void SetCoordinate()
    {
        float x = xBoard;
        float y = yBoard;

        x *= 0.66f;
        y *= 0.66f;

        x += -2.3f;
        y += -2.3f;

        transform.position = new Vector3(x, y, -1.0f);
    }
}
