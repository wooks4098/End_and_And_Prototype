using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectUI : MonoBehaviour
{
    public Camera camera;
    public Transform obj;
    public RectTransform image;
    public RectTransform Canvas;
    public Transform player;
    private void Update()
    {
        Debug.Log(camera.WorldToScreenPoint(obj.position));

        //image.transform.position = camera.WorldToScreenPoint(obj.position );
        var screenPos = camera.WorldToScreenPoint(obj.position);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(Canvas, screenPos, camera, out Vector2 pos);
        image.localPosition = pos;
        //Vector3 l_vector = player.transform.position - transform.position;
        //l_vector.y = 0;
        //transform.rotation = Quaternion.LookRotation(l_vector).normalized;

        //Canvas.LookAt(player);
        //Mathf.Clamp(Hp - Damge, 0, max);
        //Hp = Mathf.Max(Hp - Damge, 0);
    }
}
