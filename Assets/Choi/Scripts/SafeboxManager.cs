using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeboxManager : MonoBehaviour
{
    MeshRenderer meshRenderer; // ���͸����� ���Ƴ��� �� ����� �޽�������
    GameObject piece; // piece = cube 1������ �ǹ��Ѵ�
    Material material; // ��ü�� ���͸����� ������ ��� ����

    [SerializeField] List<Material> origin; // �ؽ��ĸ� ������ list
    public List<Material> Origin { get { return origin; } }

    [SerializeField] List<int> correct; // ������ ����

    [SerializeField] List<int> safeboxA;
    public List<int> SafeboxA { get { return safeboxA; } }

    [SerializeField] List<int> safeboxB;
    public List<int> SafeboxB { get { return safeboxB; } }

    List<int> arr;  // correct�� ���� �ο��� �� ����� �� ���

    [SerializeField] GameObject safeboxObjectA; // �ݰ� A�� �θ� ������Ʈ
    [SerializeField] GameObject safeboxObjectB; // �ݰ� B�� �θ� ������Ʈ


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
        // ���� �ε����� ���� ���ٸ� = ���� = ���ٺҰ���(false)
        if (correct[index] == box[index])
        {
            return false;
        }
        else return true;
    }

    private void SetArrFromOrigin()
    {
        arr.Clear();

        // arr�� origin�� index�� �Ҵ��Ѵ�.
        for (int i = 0; i < origin.Count; i++)
        {
            arr.Add(i);
        }
    }

    public void SetCorrect()
    {
        // �����̳� ����
        if (correct.Count > 0)
        {
            correct.Clear();
        }

        // arr�� origin�� index��ŭ �Ҵ��ϴ� �޼���
        SetArrFromOrigin();

        int value;
        int cnt = 0;

        // arr�� ���� ������ 0���� Ŭ ������ �ݺ�
        while (0 < arr.Count)
        {
            // correct�� arr�� ���� '��������' �ϳ��� �Ҵ�
            value = arr[Random.Range(0, arr.Count)];
            correct.Add(value);

            // �Ҵ� ���� correct�� ���� arr���� �����Ѵ�.
            arr.Remove(correct[cnt]);
            // cnt = index�� ����
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

        // arr�� origin�� index��ŭ �Ҵ��ϴ� �޼���
        SetArrFromOrigin();

        // �ε����� ����ų �� ����� �����̴�.
        int index = 0;

        // safeboxA �� �ٲٱ�
        while (4 < arr.Count)
        {
            // �ε��� ���� arr�� ���� �� �ϳ��� �������ش�.
            // arr�� origin���κ��� �Ҵ� �޾����Ƿ� 0���� ������ ������� �����Ǿ��ִ�.
            // ��, arr[1]�̸�, index = 1�� �ȴ�.
            index = arr[Random.Range(0, arr.Count)];

            // List�� ���Ҹ� �缳���ϱ� ���� ����ϴ� �޼���.
            ResetSafeboxElements(index, safeboxA);

            // �����ߴ� �ε����� arr���� �����Ѵ�.
            arr.Remove(index);
        }

        // ���� ������� safeboxB�� �缳���Ѵ�.
        while (0 < arr.Count)
        {
            index = arr[Random.Range(0, arr.Count)];
            ResetSafeboxElements(index, safeboxB);
            arr.Remove(index);
        }
    }

    private void ResetSafeboxElements(int idx, List<int> safe)
    {
        // List�� �� ���� �����Ѵ�.
        // arr�� ������ �����ϹǷ� origin�� ������ ����ߴ�.
        var temp = Random.Range(0, origin.Count);

        // ���� '����Ǿ��ִ� �� == ��ü�� ��'�̸�,
        if (safe[idx] == temp)
        {
            // �缳���Ѵ�.
            ResetSafeboxElements(idx, safe);
        }
        // �ƴ϶��
        else
        {
            // ���� ��ü�Ѵ�.
            safe[idx] = temp;
        }
    }


    public void SetMaterial()
    {
        for (int i = 0; i < origin.Count; i++)
        {
            // Safebox ������Ʈ A�� �ڽ� = ť�� 1������ piece�� �Ҵ��Ѵ�
            piece = safeboxObjectA.transform.GetChild(i).gameObject;
            // �� piece�� ���� �����Ҷ� ����� meshRendere�� �Ҵ��Ѵ�
            meshRenderer = piece.GetComponent<MeshRenderer>();

            // ������ ���͸����� origin���� �����´�
            // safeboxA[i]�� �ؽ��ĸ� �ҷ��� �ε��� ���̴�
            material = origin[safeboxA[i]];
            // ���͸����� �����Ѵ�.
            meshRenderer.material = material;

            /* �Ʒ� ���� ��ü�� �ٸ��� ���� ���̴� */

            // Safebox ������Ʈ B�� �ڽ� = ť�� 1������ piece�� �Ҵ��Ѵ�
            piece = safeboxObjectB.transform.GetChild(i).gameObject;
            // �� piece�� ���� �����Ҷ� ����� meshRendere�� �Ҵ��Ѵ�
            meshRenderer = piece.GetComponent<MeshRenderer>();

            // ������ ���͸����� origin���� �����´�
            // safeboxB[i]�� �ؽ��ĸ� �ҷ��� �ε��� ���̴�
            material = origin[safeboxB[i]];
            // ���͸����� �����Ѵ�.
            meshRenderer.material = material;            
        }        
    }
}
