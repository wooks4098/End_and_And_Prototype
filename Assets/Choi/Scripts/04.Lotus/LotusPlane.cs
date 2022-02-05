using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LotusPlane : MonoBehaviour
{
    // ==== ÄÄÆ÷³ÍÆ® ====
    private MeshCollider lotusCollider;

    // ==== bool Çü Ã¼Å© ====
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
                case true: // Æ®·¦ÀÏ ¶§
                    Debug.Log("This is Trap");
                    break;
                case false: // Æ®·¦ÀÌ ¾Æ´Ò ¶§
                    Debug.Log("This is Real Lotus");
                    break;
            }
        }
    }
}
