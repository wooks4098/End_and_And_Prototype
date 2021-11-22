using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessPlayerHp : MonoBehaviour
{
    [SerializeField] ChessManager chessManager;

    public int playerHp;

    // 데미지
    readonly int thornDamage = 20;


    private void Awake()
    {
    }
    private void Start()
    {
        ResetPlayerHp();
    }
    
    public void ResetPlayerHp()
    {
        playerHp = 60;
    }
    public int GetPlayerHp()
    {
        return playerHp;
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
        Debug.Log("현재 체력: " + playerHp);
    }


    // 
}
