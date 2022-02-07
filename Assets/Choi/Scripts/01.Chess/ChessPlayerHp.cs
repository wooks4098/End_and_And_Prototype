using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessPlayerHp : MonoBehaviour
{
    private ChessManager chessManager;
    public int playerHp;

    // 데미지
    readonly int thornDamage = 20;

    private void Awake()
    {
        chessManager = FindObjectOfType<ChessManager>();
    }

    private void Start()
    {
        ResetPlayerHp();
    }
    
    /// <summary>
    /// 체력을 초기화한다.
    /// 시작과 리스폰에 사용.
    /// </summary>
    public void ResetPlayerHp()
    {
        playerHp = 60;
    }

    // 플레이어 체력정보에 접근
    public int GetPlayerHp()
    {
        return playerHp;
    }

    // Thorn 태그가 있는 오브젝트와 충돌하면 체력 감소
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Thorn"))
        {
            LoseHp(thornDamage);
        }
    }

    // 체력 감소
    private void LoseHp(int _point)
    {
        playerHp -= _point;
        Debug.Log("현재 체력: " + playerHp);
    }
}
