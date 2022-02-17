using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabDoorButtonTrigger : MonoBehaviour
{
    // ������Ʈ
    private Animator animator;

    // ��
    [SerializeField] GameObject goDoor;

    // ��ư���� �̹� �� �� ���ٿԴ���
    [SerializeField] bool hasExitFromButton = false;


    private void Awake()
    {
        animator = GetComponentInParent<Animator>();
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
            animator.SetBool("IsOpen", true);
            goDoor.GetComponent<Animator>().SetBool("IsOpen", true);
        }
    }

    private void OnTriggerExit(Collider _other)
    {
        if (_other.tag == "Player1" || _other.tag == "Player2")
        {
            hasExitFromButton = false; // ��ư�� �� �� ���������� üũ

            // ���ݱ�
            animator.SetBool("IsOpen", false);
            goDoor.GetComponent<Animator>().SetBool("IsOpen", false);
        }
    }
}
