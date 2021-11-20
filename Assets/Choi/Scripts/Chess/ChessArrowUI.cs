using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChessArrowUI : MonoBehaviour
{
    GameObject goUIArrow;
    Transform player;
    [SerializeField] Camera cTopDownCamera;

    private void Awake()
    {
        goUIArrow = transform.GetChild(0).gameObject;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void Update()
    {
        // ������Ʈ�� ���� HP Bar ��ġ �̵�
        Vector3 screenPos = cTopDownCamera.WorldToScreenPoint(player.position);
        float x = screenPos.x;

        goUIArrow.transform.position = new Vector3(x, screenPos.y, goUIArrow.transform.position.z);
    }
}
