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
        // 오브젝트에 따른 HP Bar 위치 이동
        Vector3 screenPos = cTopDownCamera.WorldToScreenPoint(player.position);
        float x = screenPos.x;

        goUIArrow.transform.position = new Vector3(x, screenPos.y, goUIArrow.transform.position.z);
    }
}
