using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ���� UI�� �����ϴ� Ŭ����
/// ��) Player1�� UI�� ����
/// </summary>
public class UiController : MonoBehaviour
{

    //������Ʈ UI��
    public Camera camera; //�÷��̾� ī�޶�
    public RectTransform Canvas; //canvas recttransfrom
    public RectTransform image; //�̹��� ��ġ
    public GameObject Image; //������ ���ӿ�����Ʈ

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
