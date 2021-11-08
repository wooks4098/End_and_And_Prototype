using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 개별 UI를 관리하는 클래스
/// 예) Player1의 UI를 관리
/// </summary>
public class UiController : MonoBehaviour
{

    //오브젝트 UI용
    public Camera camera; //플레이어 카메라
    public RectTransform Canvas; //canvas recttransfrom
    public RectTransform image; //이미지 위치
    public GameObject Image; //이지미 게임오브젝트

    public void ObjectUIShow( )
    {
        Image.SetActive(true);
    }
    public void ObjectUIHide( )
    {
        Image.SetActive(false);
    }

    public void ObjectUIMove(Vector3 objPos)
    {
        var screenPos = camera.WorldToScreenPoint(objPos);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(Canvas, screenPos, camera, out Vector2 pos);
        image.localPosition = pos;
    }
}
