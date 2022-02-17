using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabDoorButtonTrigger : MonoBehaviour
{
    // ������Ʈ
    private Animator buttonAnimator;
    [SerializeField] Animator doorAnimator;

    // ��ư���� �̹� �� �� ���ٿԴ���
    [SerializeField] bool hasExitFromButton = false;


    private void Awake()
    {
        buttonAnimator = GetComponentInParent<Animator>();
    }

    private void Start()
    {
        hasExitFromButton = false;
    }

    private void OnTriggerEnter(Collider _other)
    {
        // �̹� �� �� ��ư�� ��������
        if (hasExitFromButton) return;

        if(_other.tag == "Player1" || _other.tag == "Player2")
        {
            // ������
            buttonAnimator.SetBool("IsOpen", true);
            doorAnimator.SetBool("IsOpen", true);
        }
    }

    private void OnTriggerExit(Collider _other)
    {
        if (_other.tag == "Player1" || _other.tag == "Player2")
        {
            hasExitFromButton = false; // ��ư�� �� �� ���������� üũ

            // ���ݱ�
            buttonAnimator.SetBool("IsOpen", false);
            doorAnimator.SetBool("IsOpen", false);
        }
    }
}
