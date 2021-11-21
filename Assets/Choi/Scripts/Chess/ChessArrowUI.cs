using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChessArrowUI : MonoBehaviour
{
    GameObject goUIArrowParent;
    Transform player;
    [SerializeField] Camera cTopDownCamera;

    [SerializeField] List<GameObject> goArrows;

    // ���
    readonly int upArrowIndex = 0;
    readonly int leftArrowIndex = 1;
    readonly int downArrowIndex = 2;
    readonly int rightArrowIndex = 3;

    private void Awake() 
    {
        goUIArrowParent = transform.GetChild(0).gameObject;
        player = GameObject.FindGameObjectWithTag("Player").transform;

        goArrows = new List<GameObject>();

        for(int i = 0; i < 4; i++)
        {
            goArrows.Add(goUIArrowParent.transform.GetChild(i).gameObject);
        }
    }

    private void Start()
    {
        SetArrowAlpha(goArrows[upArrowIndex]);
        SetArrowAlpha(goArrows[leftArrowIndex], 0.3f);
        SetArrowAlpha(goArrows[downArrowIndex], 0.3f);
        SetArrowAlpha(goArrows[rightArrowIndex], 0.3f);
    }

    void Update()
    {
        // ������Ʈ�� ���� HP Bar ��ġ �̵�
        Vector3 screenPos = cTopDownCamera.WorldToScreenPoint(player.position);
        float x = screenPos.x;

        goUIArrowParent.transform.position = new Vector3(x, screenPos.y, goUIArrowParent.transform.position.z);


        if(player.GetComponent<ChessPlayerController>().IsMoving)
        {
            HideArrows();
        }
        else if (!player.GetComponent<ChessPlayerController>().IsMoving)
        {
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
    /// </summary>
    private void CheckCurrentFloorIndexForArrowUI()
    {
        int tempIndex = player.GetComponent<ChessPlayerController>().GetCurrentFloorIndex();

        // ���� �̵� �Ұ�
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

    private void ActiveArrows()
    {
        Debug.Log("ActiveArrows");

        // �Ҵ�
        goUIArrowParent.SetActive(true);

        // �ε����� �Ǻ��ؼ� UI alpha �� ����
        CheckCurrentFloorIndexForArrowUI();
    }
}
