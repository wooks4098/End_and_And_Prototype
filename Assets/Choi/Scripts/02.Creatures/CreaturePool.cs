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

    // 크리쳐 프리팹
    public GameObject prefabs;

    // 크리쳐 관리 리스트
    private List<GameObject> creatures = new List<GameObject>();

    [SerializeField] Transform[] createPositions;

    private Coroutine activeCoroutine;

    // 내가 생성할 크리쳐의 수
    private readonly int creatureMaxCount = 8;

    // 현재 대기 중인 크리쳐 인덱스
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
        // 크리쳐 10개 미리 생성
        for (int i = 0; i < creatureMaxCount; ++i)
        {
            GameObject creature = Instantiate<GameObject>(prefabs);

            // 적을 발사하기 전까지는 비활성화 해준다.
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

    // 크리쳐 활성화
    private IEnumerator ActiveEnemyCoroutine()
    {
        while (true)
        {
            // 생성되어야할 순번의 적이 생성되어 있으면 리턴
            if (creatures[currentCreatureIndex].gameObject.activeSelf)
            {
                // 마지막 인덱스의 적을 소환했다면 마지막 번호 -> 0으로 변화
                if (currentCreatureIndex >= creatureMaxCount - 1)
                {
                    currentCreatureIndex = 0;
                }
                else
                {
                    // 아니면 그냥 인덱스 증가
                    currentCreatureIndex++;
                }

                break;
            }

            // 적 활성화
            creatures[currentCreatureIndex].gameObject.SetActive(true);

            // 마지막 인덱스의 적을 소환했다면 마지막 번호 -> 0으로 변화
            if (currentCreatureIndex >= creatureMaxCount - 1)
            {
                currentCreatureIndex = 0;
            }
            else
            {
                // 아니면 그냥 인덱스 증가
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
