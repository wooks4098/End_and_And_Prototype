using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectManager : MonoBehaviour
{
    int currentIndex = 0;
    GameObject currentPiece = null;

    [SerializeField] SafeboxManager safeboxManager;

    // safeboxA 혹은 B의 값을 값을 복사해올 컨테이너
    [SerializeField] List<int> safeboxList;
    // 접근할 수 있는지를 체크하는 list
    [SerializeField] List<bool> availableList; // true = 접근 가능, false = 접근 불가능  


    private void Awake()
    {
        availableList = new List<bool>();
    }

    private void Start()
    {
        // 걸어둔 Tag로 어떤 금고인지 구분한다.
        if (gameObject.CompareTag("SafeBoxA"))
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
            // 일치하는지 검사한 결과가 availableList에 추가된다. (true or false)
            availableList.Add(safeboxManager.IsMatch(i, safeboxList));
        }
    }
}