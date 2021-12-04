using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessPlayerHp : MonoBehaviour
{
    private ChessManager chessManager;
    public int playerHp;

    // ������
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
    /// ü���� �ʱ�ȭ�Ѵ�.
    /// ���۰� �������� ���.
    /// </summary>
    public void ResetPlayerHp()
    {
        playerHp = 60;
    }

    // �÷��̾� ü�������� ����
    public int GetPlayerHp()
    {
        return playerHp;
    }

    // Thorn �±װ� �ִ� ������Ʈ�� �浹�ϸ� ü�� ����
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Thorn"))
        {
            LoseHp(thornDamage);
        }
    }

    // ü�� ����
    private void LoseHp(int _point)
    {
        playerHp -= _point;
        Debug.Log("���� ü��: " + playerHp);
    }
}
