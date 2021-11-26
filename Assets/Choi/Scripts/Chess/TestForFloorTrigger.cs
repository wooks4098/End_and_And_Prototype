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
    [SerializeField] ChessPlayerController chessPlayerController;

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
        chessPlayerController = FindObjectOfType<ChessPlayerController>();
    }

    private void Start()
    {
        if(goChessPlayer1.activeSelf)
        {
            goChessPlayer1.SetActive(false);
        }

        if(goArrowUI.activeSelf)
        {
            goArrowUI.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider _other)
    {
        // Player1 = ��� ĳ����
        if(_other.CompareTag("Player1"))
        {
            if(OnEnterStart != null) OnEnterStart.Invoke();
        }        
        // Player = ü�� ť�� ĳ����
        else if(_other.CompareTag("Player"))
        {
            if (OnEnterGoal != null) OnEnterGoal.Invoke();
        }
    }

    public void OnChangeWhenEnterStart()
    {
        goArrowUI.gameObject.SetActive(true);

        cBasicCamera.gameObject.SetActive(false);
        cTopViewCamera.gameObject.SetActive(true);
        
        goPlayer1.SetActive(false);
        goChessPlayer1.SetActive(true);

        chessPlayerController.CanMovingInChess = true;
    }

    public void OnChangeWhenEnterGoal()
    {
        goArrowUI.gameObject.SetActive(false);

        cTopViewCamera.gameObject.SetActive(false);
        cBasicCamera.gameObject.SetActive(true);

        // ��ǥ ��ü �� (ť���� x,z �� + ��� ĳ������ y��)
        goPlayer1.transform.position = new Vector3(goChessPlayer1.transform.position.x, goPlayer1.transform.position.y, goChessPlayer1.transform.position.z);
        // Ȱ��ȭ
        goPlayer1.SetActive(true);
        goChessPlayer1.SetActive(false);

        chessPlayerController.CanMovingInChess = false;
    }

}
