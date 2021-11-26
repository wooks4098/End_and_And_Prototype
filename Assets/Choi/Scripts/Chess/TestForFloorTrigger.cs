using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// �׽�Ʈ���̶� �Ű�Ⱦ��� ���Ƴ���!!
/// ī�޶� ��ȯ �� ���������� ������
/// </summary>
public class TestForFloorTrigger : MonoBehaviour
{
    private ChessPlayerController chessPlayerController;

    // ���� �״� �� ������Ʈ��
    [SerializeField] Camera cBasicCamera; // ��� ĳ���Ϳ��� �޸� ī�޶�
    [SerializeField] Camera cTopViewCamera; // ť�� ĳ���͸� �ٶ� ž�� ī�޶�
    [SerializeField] GameObject goPlayer1; // ��� ĳ����
    [SerializeField] GameObject goChessPlayer1; // ü������ ������ ť�� ĳ����
    [SerializeField] GameObject goArrowUI; 

    // ����Ƽ �̺�Ʈ - ������Ʈ���� ��������
    public UnityEvent OnEnterStart;
    public UnityEvent OnEnterGoal;


    private void Awake()
    {
        // Chess Player Controller�� ã�´�
        // ť�� ĳ������ �������� �����ϱ� ���� - isMoveOnChess
        chessPlayerController = FindObjectOfType<ChessPlayerController>();
    }

    private void Start()
    {
        // ü���� ť�� ĳ���Ͱ� ���� �ִٸ�
        if(goChessPlayer1.activeSelf)
        {
            // ���� (������ �� ��� �� ��)
            goChessPlayer1.SetActive(false);
        }

        // ȭ��ǥ UI�� �����ִٸ�
        if(goArrowUI.activeSelf)
        {
            // ���� (������ �� ��� �� ��)
            goArrowUI.SetActive(false);
        }
    }

    /// <summary>
    /// ���� ������Ʈ�� �浹���� ��...
    /// </summary>
    private void OnTriggerEnter(Collider _other)
    {
        // Player1�� �浹�ϸ�
        // *** Player1 = ��� ĳ����
        if(_other.CompareTag("Player1"))
        {
            if(OnEnterStart != null) OnEnterStart.Invoke();
        }      
        // Player�� �浹�ϸ�
        // *** Player = ü�� ť�� ĳ����
        else if(_other.CompareTag("Player"))
        {
            if (OnEnterGoal != null) OnEnterGoal.Invoke();
        }
    }

    /// <summary>
    /// Start Point�� ������ �� ����...
    /// </summary>
    public void OnChangeWhenEnterStart()
    {
        // ȭ��ǥ UI = ��
        goArrowUI.gameObject.SetActive(true);

        // �Ϲ� ī�޶� = ��
        cBasicCamera.gameObject.SetActive(false);
        // ü���� ž�� ī�޶� = ��
        cTopViewCamera.gameObject.SetActive(true);
        
        // ��� ĳ���� = ��
        goPlayer1.SetActive(false);
        // ü�� ť�� ĳ���� = ��
        goChessPlayer1.SetActive(true);

        // ü���ǿ��� ť�갡 ������ �� �ְ� true�� ����
        chessPlayerController.CanMoveOnChess = true;
    }

    /// <summary>
    /// Goal Point�� ������ �� ����...
    /// </summary>
    public void OnChangeWhenEnterGoal()
    {
        // ȭ��ǥ UI = ��
        goArrowUI.gameObject.SetActive(false);

        // ü���� ž�� ī�޶� = ��
        cTopViewCamera.gameObject.SetActive(false);
        // �Ϲ� ī�޶� = ��
        cBasicCamera.gameObject.SetActive(true);

        // ��ǥ ��ü �� (ť���� x,z �� + ��� ĳ������ y��)
        goPlayer1.transform.position = new Vector3(goChessPlayer1.transform.position.x, goPlayer1.transform.position.y, goChessPlayer1.transform.position.z);
        // ��� ĳ���� = ��
        goPlayer1.SetActive(true);
        // ü�� ť�� ĳ���� = �� ��
        goChessPlayer1.SetActive(false);

        // ü���ǿ��� ť�갡 ������ �� ������ false�� ����
        chessPlayerController.CanMoveOnChess = false;
    }

}
