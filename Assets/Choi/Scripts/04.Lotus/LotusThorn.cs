using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ʈ�������� �ൿ�� �ٷ�� Ŭ����
/// </summary>
public class LotusThorn : MonoBehaviour
{
    // �÷��̾�� �浹�ϸ� ����������Ѵ�.
    private void OnTriggerEnter(Collider _other)
    {
        if (_other.CompareTag("Player1") || _other.CompareTag("Player2"))
        {
            // �Ʒ��� HidePlantThorn �޼��� ����
            // 2�ʿ� �� ���� ��Ȱ��ȭ
            Invoke("HidePlantThorn", 2f);
        }
    }

    private void HidePlantThorn()
    {
        this.gameObject.SetActive(false);
    }
}
