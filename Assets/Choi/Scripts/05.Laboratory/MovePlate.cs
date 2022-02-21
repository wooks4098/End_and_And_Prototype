using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovePlate : MonoBehaviour
{
    public GameObject controller;

    GameObject reference = null;

    // Board Positions, not world Positions
    int matrixX;
    int matrixY;

    // false: movement, true: attacking
    public bool attack = false;

    private void OnEnable()
    {
        
    }

    public void Start()
    {
        if(attack)
        {
            // Change to red
            gameObject.GetComponent<Image>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
            // gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        }
    }

    public void OnMouseUp()
    {
        controller = GameObject.Find("Gas Valve Controller");

        if(attack)
        {
            GameObject cp = controller.GetComponent<GasValve>().GetPosition(matrixX, matrixY);

            Destroy(cp);
        }

        controller.GetComponent<GasValve>().SetPositionEmpty(reference.GetComponent<GasValveSymbol>().GetXBoard(),
                                                             reference.GetComponent<GasValveSymbol>().GetYBoard());

        reference.GetComponent<GasValveSymbol>().SetXBoard(matrixX);
        reference.GetComponent<GasValveSymbol>().SetYBoard(matrixY);

        reference.GetComponent<GasValveSymbol>().SetCoord();

        controller.GetComponent<GasValve>().SetPosition(reference);

        reference.GetComponent<GasValveSymbol>().DestroyMovePlates();
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
