using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectUI : MonoBehaviour
{
    public Camera camera;
    public Transform obj;
    public RectTransform image;
    public RectTransform Canvas;
    public GameObject Image;
    //private void FixedUpdate()
    //{
    //    Debug.Log(camera.WorldToScreenPoint(obj.position));

    //    var screenPos = camera.WorldToScreenPoint(obj.position);
    //    RectTransformUtility.ScreenPointToLocalPointInRectangle(Canvas, screenPos, camera, out Vector2 pos);
    //    image.localPosition = pos;

    //}
    public void hide()
    {
        Image.SetActive(false);
    }

    public void Show()
    {
        Image.SetActive(true);

    }

    public void Move()
    {
        var screenPos = camera.WorldToScreenPoint(obj.position);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(Canvas, screenPos, camera, out Vector2 pos);
        image.localPosition = pos;
    }
}
