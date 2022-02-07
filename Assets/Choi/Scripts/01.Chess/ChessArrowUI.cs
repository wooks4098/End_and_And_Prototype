using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChessArrowUI : MonoBehaviour
{
    // ȭ��ǥ ui�� �θ� �޾ƿ� ����
    // �ֳ��ϸ� ȭ��ǥ�� �ϳ��� active false �� �� �ƴ϶�,
    // ȭ��ǥ�� �ϳ��� ���� �θ� ��ü�� active false �� �����̱� �����̴�.
    GameObject goUIArrowParent;
    Transform player;

    // UI�� �׸� ������ �Ǵ� ž�� ī�޶�.
    [SerializeField] Camera cTopDownCamera;

    // ȭ��ǥ ui 4���� ������ ����Ʈ
    [SerializeField] List<GameObject> goArrows;

    // ���
    readonly int upArrowIndex = 0; // ���� ȭ��ǥ
    readonly int leftArrowIndex = 1; // ���� ȭ��ǥ
    readonly int downArrowIndex = 2; // �Ʒ��� ȭ��ǥ
    readonly int rightArrowIndex = 3; // ������ ȭ��ǥ

    private void Awake() 
    {
        // ȭ��ǥ UI�� �θ�� ���� ��ũ��Ʈ�� �ִ� ������Ʈ�� �ڽ����κ��� �޾ƿ´�
        goUIArrowParent = transform.GetChild(0).gameObject;

        player = GameObject.FindGameObjectWithTag("Player").transform;

        // ȭ��ǥ ui�� ���� ����Ʈ�� �ʱ�ȭ�ϰ�
        goArrows = new List<GameObject>();

        // for���� ���� 4���� ȭ��ǥ�� ����Ʈ�� �߰��Ѵ�.
        for(int i = 0; i < 4; i++)
        {
            // �տ��� �޾ƿ� goUIArrow�� �ڽ� ��ġ�� ȭ��ǥ ui�� �ִ�.
            goArrows.Add(goUIArrowParent.transform.GetChild(i).gameObject);
        }
    }

    private void Start()
    {
        // �������� = starting floor�̹Ƿ�
        // ȭ��ǥ ui�� ���İ��� �Ʒ��� ���� �����Ѵ�.
        SetArrowAlpha(goArrows[upArrowIndex]);
        SetArrowAlpha(goArrows[leftArrowIndex], 0.3f);
        SetArrowAlpha(goArrows[downArrowIndex], 0.3f);
        SetArrowAlpha(goArrows[rightArrowIndex], 0.3f);
    }

    void Update()
    {
        // ������Ʈ�� ���� ȭ��ǥ ui�� ��ġ �̵�
        // �÷��̾��� ��ǥ�� ī�޶�� ���� �޾ƿ´�.
        Vector3 screenPos = cTopDownCamera.WorldToScreenPoint(player.position);
        float x = screenPos.x;

        // ui �θ��� ��ġ�� �����Ѵ� (ȭ��ǥ ui�� �ϳ��� �����̸� ���ŷο�Ƿ�)
        goUIArrowParent.transform.position = new Vector3(x, screenPos.y, goUIArrowParent.transform.position.z);

        // ChessPlayerController.isMoving == true �̸�, (�����̰� ������)
        if (player.GetComponent<ChessPlayerController>().IsMoving)
        {
            // ȭ��ǥ�� �����
            HideArrows();
        }
        // ChessPlayerController.isMoving == false �̸�, (�����̰� ���� ������)
        else if (!player.GetComponent<ChessPlayerController>().IsMoving)
        {
            // ȭ��ǥ�� ǥ���Ѵ�
            ActiveArrows();
        }
    }

    /// <summary>
    /// ȭ��ǥ�� alpha ���� �ٲٴ� �޼���
    /// </summary>
    /// <param name="_arrow">���õ� arrow object</param>
    /// <param name="_alpha">������ ���� ���� ��</param>
    public void SetArrowAlpha(GameObject _arrow, float _alpha = 1f)
    {
        Image image = _arrow.GetComponent<Image>();
        var imageColor = image.color;
        imageColor.a = _alpha;

        _arrow.GetComponent<Image>().color = imageColor;
    }
    
    /// <summary>
    /// ���� �ε����� �Ǻ� -> �̸� ������� ui ����
    /// Ư�� �������� �̵��� �Ұ��� ���,
    /// �ش� ���� ȭ��ǥ ui�� ���� ���� 0.3f�� ����ȴ�.
    /// </summary>
    private void CheckCurrentFloorIndexForArrowUI()
    {
        // ���� �ε����� ChessPlayerController�κ��� �޾ƿ´�
        int tempIndex = player.GetComponent<ChessPlayerController>().GetCurrentFloorIndex();

        // ���� �̵� �Ұ�
        // 0�� ������������ �����ϱ� ������ ����ó���Ѵ�.
        if (tempIndex <= 5 && tempIndex != 0)
        {
            SetArrowAlpha(goArrows[upArrowIndex], 0.3f);            
        }
        // �Ʒ��� �̵� �Ұ�
        else if (tempIndex >= 30)
        {
            SetArrowAlpha(goArrows[downArrowIndex], 0.3f);
        }
        // �������� �̵� �Ұ�
        else if ((tempIndex % 6) == 0)
        {
            SetArrowAlpha(goArrows[leftArrowIndex], 0.3f);
        }
        //���������� �̵� �Ұ�
        else if ((tempIndex % 6) == 5)
        {
            SetArrowAlpha(goArrows[rightArrowIndex], 0.3f);
        }

        // ���� �� = �������� ���� = �̵��� �� �־�� ��.
        // => ����

        // ������ ���� �̵� �Ұ�
        if (tempIndex <= 5 && (tempIndex % 6) == 5)
        {
            SetArrowAlpha(goArrows[upArrowIndex], 0.3f);
            SetArrowAlpha(goArrows[rightArrowIndex], 0.3f);
        }
        // ���� �Ʒ��� �̵� �Ұ�
        else if (tempIndex >= 30 && (tempIndex % 6) == 0)
        {
            SetArrowAlpha(goArrows[downArrowIndex], 0.3f);
            SetArrowAlpha(goArrows[leftArrowIndex], 0.3f);
        }
        // ������ �Ʒ��� �̵� �Ұ�
        else if (tempIndex >= 30 && (tempIndex % 6) == 5)
        {
            SetArrowAlpha(goArrows[downArrowIndex], 0.3f);
            SetArrowAlpha(goArrows[rightArrowIndex], 0.3f);
        }

        // ��������
        if(tempIndex > 35)
        {
            SetArrowAlpha(goArrows[downArrowIndex], 0.3f);
            SetArrowAlpha(goArrows[leftArrowIndex], 0.3f);
            SetArrowAlpha(goArrows[rightArrowIndex], 0.3f);
        }        
        // ������
        else if(tempIndex < 0)
        {
            SetArrowAlpha(goArrows[upArrowIndex], 0.3f);
            SetArrowAlpha(goArrows[leftArrowIndex], 0.3f);
            SetArrowAlpha(goArrows[rightArrowIndex], 0.3f);
        }
    }

    /// <summary>
    /// �÷��̾� �̵� �߿��� ȭ��ǥ�� �����.
    /// ���� �� ��� ���İ��� 1�� �����Ѵ�.
    /// </summary>
    private void HideArrows()
    {
        // alpha���� ���� 1�� �����ϰ�
        SetArrowAlpha(goArrows[upArrowIndex]);
        SetArrowAlpha(goArrows[leftArrowIndex]);
        SetArrowAlpha(goArrows[downArrowIndex]);
        SetArrowAlpha(goArrows[rightArrowIndex]);

        // ����
        goUIArrowParent.SetActive(false);
    }

    /// <summary>
    /// �÷��̾� �̵��� ���߸� ȭ��ǥ�� ��Ÿ����.
    /// ȭ��ǥ�� ��Ÿ���� ���ÿ� ���İ��� �����Ѵ�.
    /// </summary>
    private void ActiveArrows()
    {
        //Debug.Log("ActiveArrows");

        // �Ҵ�
        goUIArrowParent.SetActive(true);

        // �ε����� �Ǻ��ؼ� UI alpha �� ����
        CheckCurrentFloorIndexForArrowUI();
    }
}
