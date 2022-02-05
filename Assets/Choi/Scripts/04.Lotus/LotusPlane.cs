using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LotusPlane : MonoBehaviour
{
    // ==== ������Ʈ ====
    private MeshCollider lotusCollider;

    // ==== bool �� üũ ====
    [SerializeField] bool isTrap;
    public bool GetIsTrap() { return isTrap; }

    private void Awake()
    {
        lotusCollider = GetComponent<MeshCollider>();
    }

    private void OnTriggerEnter(Collider _other)
    {
        if(_other.CompareTag("Player1")|| _other.CompareTag("Player2"))
        {
            switch (isTrap)
            {
                case true: // Ʈ���� ��
                    Debug.Log("This is Trap");
                    break;
                case false: // Ʈ���� �ƴ� ��
                    Debug.Log("This is Real Lotus");
                    break;
            }
        }
    }
}
