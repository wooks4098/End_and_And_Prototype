using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessPlayerHp : MonoBehaviour
{
    [SerializeField] ChessManager chessManager;

    [HideInInspector] public int playerHp;

    // ������
    readonly int thornDamage = 20;


    private void Awake()
    {
        //chessManager = GetComponent<ChessManager>();
    }
    private void Start()
    {
        ResetPlayerHp();
    }
    
    void ResetPlayerHp()
    {
        playerHp = 60;
    }

    private void OnEnable()
    {
        //chessManager.OnPlantThornEvent += LoseHp;
    }

    private void OnDisable()
    {
        //chessManager.OnPlantThornEvent -= LoseHp;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Thorn"))
        {
            LoseHp(thornDamage);
        }
    }

    private void LoseHp(int _point)
    {
        playerHp -= _point;
        Debug.Log("���� ü��: " + playerHp);
    }

    public int GetPlayerHp()
    {
        return playerHp;
    }


    // 
}
