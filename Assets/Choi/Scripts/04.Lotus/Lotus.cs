using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lotus : MonoBehaviour
{
    /* ============== 컴포넌트 ============== */
    // 트랩, 체스에서 Tree 프리팹을 연꽃 기믹용으로 변환하여 사용
    [SerializeField] LotusThorn goTrap;
    // 연꽃의 콜라이더
    private BoxCollider lotusCollider;

    /* ============== bool 형 체크 ============== */
    // 트랩인가?
    [SerializeField] bool isTrap;
    public bool GetIsTrap() { return isTrap; }

    /* ============== 이벤트 ============== */
    // 연꽃 활성화 및 비활성화
    public Action OnActive { get; set; } // 활성화
    public Action InActive { get; set; } // 비활성화

    /* ============== 코루틴 ============== */
    // 코루틴을 저장할 변수
    private Coroutine coroutine;
    // 코루틴이 돌아가는지 확인
    private bool isCoroutineRunning;



    private void Awake()
    {
        lotusCollider = GetComponent<BoxCollider>();

        // isTrap이 true이면 (= 이 연꽃이 트랩이면)
        if (isTrap)
        {
            // Trap을 할당 = 기존에 있던 Tree 프리팹 사용
            goTrap = this.transform.GetChild(0).GetComponent<LotusThorn>();

            // 켜져있으면 끈다.
            if (goTrap.gameObject.activeSelf)
            {
                goTrap.gameObject.SetActive(false);
            }
        }
    }

    private void Start()
    {
        // 연꽃의 콜라이더를 꺼둔다
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
    /// Lotus(연꽃) 활성화
    /// OnActive?.Invoke();
    /// </summary>
    private void ActiveLotus()
    {
        // 콜라이더 활성화
        lotusCollider.enabled = true;
    }
    /// <summary>
    /// Lotus(연꽃) 비활성화
    /// InActive?.Invoke();
    /// </summary>
    private void InactiveLotus()
    {
        // 콜라이더 비활성화
        lotusCollider.enabled = false;
    }

    /// <summary>
    /// 연꽃에 들어왔을 때
    /// </summary>
    private void OnTriggerEnter(Collider _other)
    {
        // 태그 검사
        if(_other.CompareTag("Player1")|| _other.CompareTag("Player2"))
        {
            // 트랩인지 아닌지 판별
            switch (isTrap)
            {
                case true: // 트랩일 때
                    {
                        Debug.Log("This is Trap");

                        // 코루틴이 실행중이면 
                        if(coroutine != null)
                        {
                            // 멈추고
                            StopCoroutine(coroutine);
                        }
                        // 새로운 코루틴 실행
                        coroutine = StartCoroutine(ActiveLotusTrap());

                        break;
                    }
                case false: // 트랩이 아닐 때
                    {
                        Debug.Log("This is Real Lotus");

                        break;
                    }
            }
        }
    }

    /// <summary>
    /// 연꽃에서 나갈 때
    /// </summary>
    private void OnTriggerExit(Collider _other)
    {
        // 태그 검사
        if (_other.CompareTag("Player1") || _other.CompareTag("Player2"))
        {
            // 트랩인 곳을 빠져나가면
            if(isTrap)
            {
                // 만약 코루틴이 실행되고 있으면 
                if (isCoroutineRunning)
                {
                    // 코루틴을 강제로 멈춘다.
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
        // 현재 코루틴이 돌아가는지 체크
        isCoroutineRunning = true;

        // 무한루프
        while (true)
        {
            // 꺼져있으면 켠다.
            if (!goTrap.gameObject.activeSelf)
            {
                goTrap.gameObject.SetActive(true);
            }

            // 5초에 한 번씩 트랩(goTrap) 활성화 시도
            yield return new WaitForSeconds(5f);
        }
    }
}