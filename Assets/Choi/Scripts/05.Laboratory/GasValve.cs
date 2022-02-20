using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasValve : MonoBehaviour
{
    [SerializeField] GameObject piece;

    private GameObject[,] positions = new GameObject[5, 5];
    private GameObject[] symbols = new GameObject[5];

    private void Start()
    {
        Instantiate(piece, new Vector3(0, 0, -1), Quaternion.identity);
    }
}