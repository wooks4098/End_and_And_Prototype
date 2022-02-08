using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lotus : MonoBehaviour
{
    /* ============== ������Ʈ ============== */
    // Ʈ��, ü������ Tree �������� ���� ��Ϳ����� ��ȯ�Ͽ� ���
    [SerializeField] LotusThorn goTrap;
    // ������ �ݶ��̴�
    private BoxCollider lotusCollider;

    /* ============== bool �� üũ ============== */
    // Ʈ���ΰ�?
    [SerializeField] bool isTrap;
    public bool GetIsTrap() { return isTrap; }

    /* ============== �̺�Ʈ ============== */
    // ���� Ȱ��ȭ �� ��Ȱ��ȭ
    public Action OnActive { get; set; } // Ȱ��ȭ
    public Action InActive { get; set; } // ��Ȱ��ȭ

    /* ============== �ڷ�ƾ ============== */
    // �ڷ�ƾ�� ������ ����
    private Coroutine coroutine;
    // �ڷ�ƾ�� ���ư����� Ȯ��
    private bool isCoroutineRunning;



    private void Awake()
    {
        lotusCollider = GetComponent<BoxCollider>();

        // isTrap�� true�̸� (= �� ������ Ʈ���̸�)
        if (isTrap)
        {
            // Trap�� �Ҵ� = ������ �ִ� Tree ������ ���
            goTrap = this.transform.GetChild(0).GetComponent<LotusThorn>();

            // ���������� ����.
            if (goTrap.gameObject.activeSelf)
            {
                goTrap.gameObject.SetActive(false);
            }
        }
    }

    private void Start()
    {
        // ������ �ݶ��̴��� ���д�
        lotusCollider.enabled = false;
    }

    #region Enable, Disable
    private void OnEnable()
    {
        OnActive += ActiveLotus;
        InActive += InactiveLotus;
    }
    private void OnDisable()
    {
        OnActive -= ActiveLotus;
        InActive -= InactiveLotus;
    }
    #endregion

    /// <summary>
    /// Lotus(����) Ȱ��ȭ
    /// OnActive?.Invoke();
    /// </summary>
    private void ActiveLotus()
    {
        // �ݶ��̴� Ȱ��ȭ
        lotusCollider.enabled = true;
    }
    /// <summary>
    /// Lotus(����) ��Ȱ��ȭ
    /// InActive?.Invoke();
    /// </summary>
    private void InactiveLotus()
    {
        // �ݶ��̴� ��Ȱ��ȭ
        lotusCollider.enabled = false;
    }

    /// <summary>
    /// ���ɿ� ������ ��
    /// </summary>
    private void OnTriggerEnter(Collider _other)
    {
        // �±� �˻�
        if(_other.CompareTag("Player1")|| _other.CompareTag("Player2"))
        {
            // Ʈ������ �ƴ��� �Ǻ�
            switch (isTrap)
            {
                case true: // Ʈ���� ��
                    {
                        Debug.Log("This is Trap");

                        // �ڷ�ƾ�� �������̸� 
                        if(coroutine != null)
                        {
                            // ���߰�
                            StopCoroutine(coroutine);
                        }
                        // ���ο� �ڷ�ƾ ����
                        coroutine = StartCoroutine(ActiveLotusTrap());

                        break;
                    }
                case false: // Ʈ���� �ƴ� ��
                    {
                        Debug.Log("This is Real Lotus");

                        break;
                    }
            }
        }
    }

    /// <summary>
    /// ���ɿ��� ���� ��
    /// </summary>
    private void OnTriggerExit(Collider _other)
    {
        // �±� �˻�
        if (_other.CompareTag("Player1") || _other.CompareTag("Player2"))
        {
            // Ʈ���� ���� ����������
            if(isTrap)
            {
                // ���� �ڷ�ƾ�� ����ǰ� ������ 
                if (isCoroutineRunning)
                {
                    // �ڷ�ƾ�� ������ �����.
                    Debug.Log("StopCoroutine");
                    StopCoroutine(coroutine);
                }
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private IEnumerator ActiveLotusTrap()
    {
        // ���� �ڷ�ƾ�� ���ư����� üũ
        isCoroutineRunning = true;

        // ���ѷ���
        while (true)
        {
            // ���������� �Ҵ�.
            if (!goTrap.gameObject.activeSelf)
            {
                goTrap.gameObject.SetActive(true);
            }

            // 5�ʿ� �� ���� Ʈ��(goTrap) Ȱ��ȭ �õ�
            yield return new WaitForSeconds(5f);
        }
    }
}