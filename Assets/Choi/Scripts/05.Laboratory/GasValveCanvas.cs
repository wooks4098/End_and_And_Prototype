using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasValveCanvas : MonoBehaviour
{
    [SerializeField] Camera camPlayerA;
    [SerializeField] Camera camPlayerB;

    void Start()
    {
        // Canvas 카메라 설정 참고
        // https://gist.github.com/oviniciusfaria/139a1239adf787a3cef8e521db7f84b0

        Canvas canvas = gameObject.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = camPlayerB;
    }
}