using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectManager : MonoBehaviour
{
    int currentIndex = 0;
    GameObject currentPiece = null;

    [SerializeField] SafeboxManager safeboxManager;

    // safeboxA Ȥ�� B�� ���� ���� �����ؿ� �����̳�
    [SerializeField] List<int> safeboxList;
    // ������ �� �ִ����� üũ�ϴ� list
    [SerializeField] List<bool> availableList; // true = ���� ����, false = ���� �Ұ���  


    private void Awake()
    {
        availableList = new List<bool>();
    }

    private void Start()
    {
        // �ɾ�� Tag�� � �ݰ����� �����Ѵ�.
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
            // ��ġ�ϴ��� �˻��� ����� availableList�� �߰��ȴ�. (true or false)
            availableList.Add(safeboxManager.IsMatch(i, safeboxList));
        }
    }
}