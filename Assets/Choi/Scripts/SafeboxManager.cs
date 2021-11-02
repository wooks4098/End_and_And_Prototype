using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeboxManager : MonoBehaviour
{
    MeshRenderer meshRenderer; // 머터리얼을 갈아끼울 때 사용할 메쉬렌더러
    GameObject piece; // piece = cube 1조각을 의미한다
    Material material; // 교체할 머터리얼의 정보를 잠깐 저장

    [SerializeField] List<Material> origin; // 텍스쳐를 저장할 list
    public List<Material> Origin { get { return origin; } }

    [SerializeField] List<int> correct; // 정답을 저장

    [SerializeField] List<int> safeboxA;
    public List<int> SafeboxA { get { return safeboxA; } }

    [SerializeField] List<int> safeboxB;
    public List<int> SafeboxB { get { return safeboxB; } }

    List<int> arr;  // correct에 값을 부여할 때 사용할 때 사용

    [SerializeField] GameObject safeboxObjectA; // 금고 A의 부모 오브젝트
    [SerializeField] GameObject safeboxObjectB; // 금고 B의 부모 오브젝트


    private void Awake()
    {
        correct = new List<int>();
        arr = new List<int>();
    }

    private void Start()
    {
        SetCorrect();
        SetSafeboxFromCorrect();
    }

    public bool IsMatch(int index, List<int> box)
    {
        // 만약 인덱스의 값이 같다면 = 정답 = 접근불가능(false)
        if (correct[index] == box[index])
        {
            return false;
        }
        else return true;
    }

    private void SetArrFromOrigin()
    {
        arr.Clear();

        // arr에 origin의 index를 할당한다.
        for (int i = 0; i < origin.Count; i++)
        {
            arr.Add(i);
        }
    }

    public void SetCorrect()
    {
        // 컨테이너 비우기
        if (correct.Count > 0)
        {
            correct.Clear();
        }

        // arr에 origin의 index만큼 할당하는 메서드
        SetArrFromOrigin();

        int value;
        int cnt = 0;

        // arr의 원소 개수가 0보다 클 때까지 반복
        while (0 < arr.Count)
        {
            // correct에 arr의 값을 '랜덤으로' 하나씩 할당
            value = arr[Random.Range(0, arr.Count)];
            correct.Add(value);

            // 할당 받은 correct의 값을 arr에서 삭제한다.
            arr.Remove(correct[cnt]);
            // cnt = index를 증가
            cnt++;
        }
    }

    public void SetSafeboxFromCorrect()
    {
        if (safeboxA.Count > 0) safeboxA.Clear();
        if (safeboxB.Count > 0) safeboxB.Clear();

        for (int i = 0; i < correct.Count; i++)
        {
            safeboxA.Add(correct[i]);
            safeboxB.Add(correct[i]);
        }

        // arr에 origin의 index만큼 할당하는 메서드
        SetArrFromOrigin();

        // 인덱스를 가리킬 때 사용할 변수이다.
        int index = 0;

        // safeboxA 값 바꾸기
        while (4 < arr.Count)
        {
            // 인덱스 값을 arr의 원소 중 하나로 설정해준다.
            // arr는 origin으로부터 할당 받았으므로 0부터 끝까지 순서대로 나열되어있다.
            // 즉, arr[1]이면, index = 1이 된다.
            index = arr[Random.Range(0, arr.Count)];

            // List의 원소를 재설정하기 위해 사용하는 메서드.
            ResetSafeboxElements(index, safeboxA);

            // 설정했던 인덱스는 arr에서 제거한다.
            arr.Remove(index);
        }

        // 같은 방법으로 safeboxB도 재설정한다.
        while (0 < arr.Count)
        {
            index = arr[Random.Range(0, arr.Count)];
            ResetSafeboxElements(index, safeboxB);
            arr.Remove(index);
        }
    }

    private void ResetSafeboxElements(int idx, List<int> safe)
    {
        // List에 들어갈 값을 설정한다.
        // arr는 꾸준히 감소하므로 origin의 개수를 사용했다.
        var temp = Random.Range(0, origin.Count);

        // 만약 '저장되어있는 값 == 교체할 값'이면,
        if (safe[idx] == temp)
        {
            // 재설정한다.
            ResetSafeboxElements(idx, safe);
        }
        // 아니라면
        else
        {
            // 값을 교체한다.
            safe[idx] = temp;
        }
    }


    public void SetMaterial()
    {
        for (int i = 0; i < origin.Count; i++)
        {
            // Safebox 오브젝트 A의 자식 = 큐브 1조각을 piece에 할당한다
            piece = safeboxObjectA.transform.GetChild(i).gameObject;
            // 그 piece를 통해 변경할때 사용할 meshRendere도 할당한다
            meshRenderer = piece.GetComponent<MeshRenderer>();

            // 변경할 머터리얼을 origin에서 가져온다
            // safeboxA[i]가 텍스쳐를 불러올 인덱스 값이다
            material = origin[safeboxA[i]];
            // 머터리얼을 적용한다.
            meshRenderer.material = material;

            /* 아래 또한 주체만 다르고 같은 일이다 */

            // Safebox 오브젝트 B의 자식 = 큐브 1조각을 piece에 할당한다
            piece = safeboxObjectB.transform.GetChild(i).gameObject;
            // 그 piece를 통해 변경할때 사용할 meshRendere도 할당한다
            meshRenderer = piece.GetComponent<MeshRenderer>();

            // 변경할 머터리얼을 origin에서 가져온다
            // safeboxB[i]가 텍스쳐를 불러올 인덱스 값이다
            material = origin[safeboxB[i]];
            // 머터리얼을 적용한다.
            meshRenderer.material = material;            
        }        
    }
}
