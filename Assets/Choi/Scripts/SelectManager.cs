using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectManager : MonoBehaviour
{
    int currentIndex = 0;
    GameObject currentPiece = null;

    [SerializeField] SafeboxManager safeboxManager;
    [SerializeField] List<int> safeboxList;
    [SerializeField] List<bool> availableList;


    private void Awake()
    {
        availableList = new List<bool>();
    }

    private void Start()
    {
        if(gameObject.CompareTag("SafeBoxA"))
        {
            safeboxList = safeboxManager.SafeboxA;
        }            
        else if (gameObject.CompareTag("SafeBoxB"))
        {
            safeboxList = safeboxManager.SafeboxB;
        }
    }

    private void Update()
    {
        CheckAvailable();
    }

    private void CheckAvailable()
    {
        availableList.Clear();

        for (int i = 0; i < safeboxManager.Origin.Count; i++)
        {
            availableList.Add(safeboxManager.IsMatch(i, safeboxList));
        }
    }
}