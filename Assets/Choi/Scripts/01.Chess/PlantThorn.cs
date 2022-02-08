using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �Ĺ� ������ �ൿ�� �ٷ�� Ŭ����
/// </summary>
public class PlantThorn : MonoBehaviour
{
    // �÷��̾�� �浹�ϸ� ����������Ѵ�.
    private void OnTriggerEnter(Collider _other)
    {
        if (_other.CompareTag("Player"))
        {
            // �Ʒ��� HidePlantThorn �޼��� ����
            Invoke("HidePlantThorn", 1f);
        }
    }

    private void HidePlantThorn()
    {
        this.gameObject.SetActive(false);
    }
}
