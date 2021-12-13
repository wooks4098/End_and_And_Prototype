using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreaturePool : MonoBehaviour
{
    private static CreaturePool instance;
    public static CreaturePool GetInstance()
    {
        return instance;
    }

    // ũ���� ������
    public GameObject prefabs;

    // ũ���� ���� ����Ʈ
    private List<GameObject> creatures = new List<GameObject>();

    [SerializeField] Transform[] createPositions;

    private Coroutine activeCoroutine;

    // ���� ������ ũ������ ��
    private readonly int creatureMaxCount = 8;

    // ���� ��� ���� ũ���� �ε���
    private int currentCreatureIndex = 0;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
    }

    void Start()
    {
        // ũ���� 10�� �̸� ����
        for (int i = 0; i < creatureMaxCount; ++i)
        {
            GameObject creature = Instantiate<GameObject>(prefabs);

            // ���� �߻��ϱ� �������� ��Ȱ��ȭ ���ش�.
            creature.SetActive(false);

            creatures.Add(creature);
        }
    }

    void Update()
    {
        ActiveEnemy();
    }

    private void ActiveEnemy()
    {
        if (activeCoroutine == null)
        {
            activeCoroutine = StartCoroutine(ActiveEnemyCoroutine());
        }
    }

    // ũ���� Ȱ��ȭ
    private IEnumerator ActiveEnemyCoroutine()
    {
        while (true)
        {
            // �����Ǿ���� ������ ���� �����Ǿ� ������ ����
            if (creatures[currentCreatureIndex].gameObject.activeSelf)
            {
                // ������ �ε����� ���� ��ȯ�ߴٸ� ������ ��ȣ -> 0���� ��ȭ
                if (currentCreatureIndex >= creatureMaxCount - 1)
                {
                    currentCreatureIndex = 0;
                }
                else
                {
                    // �ƴϸ� �׳� �ε��� ����
                    currentCreatureIndex++;
                }

                break;
            }

            // �� Ȱ��ȭ
            creatures[currentCreatureIndex].gameObject.SetActive(true);

            // ������ �ε����� ���� ��ȯ�ߴٸ� ������ ��ȣ -> 0���� ��ȭ
            if (currentCreatureIndex >= creatureMaxCount - 1)
            {
                currentCreatureIndex = 0;
            }
            else
            {
                // �ƴϸ� �׳� �ε��� ����
                currentCreatureIndex++;
            }

            yield return new WaitForSecondsRealtime(7f);
        }

        activeCoroutine = null;
    }

    public Transform GetCreatePosition()
    {
        int index = Random.Range(0, createPositions.Length);

        return createPositions[index];
    }
}
