using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Dialog UI�� �����ϴ� ��ũ��Ʈ
/// - ũ������
/// </summary>
public class DialogUI : MonoBehaviour
{
    [SerializeField] GameObject GDialogUI;
    RectTransform DialogUIRect;
    [SerializeField] GameObject Contensts;
    List<GameObject> DialogList;//��ȭ ����Ʈ
    [SerializeField] GameObject testtext;
    [SerializeField] int DialogCount;//���� �������� ��� ����
    [SerializeField] int basicHeight;//��ȭâ �⺻ ����
    [SerializeField] int increaseHeight;//��ȭâ ���� ��ġ
    private void Start()
    {
        DialogUIRect = GDialogUI.GetComponent<RectTransform>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            //DialogData test = new DialogData();
            //test.name = "�׽�Ʈ";
            //test.dialog = testnumber.ToString();
            //AddDialog(++testnumber, test);
        }
    }

    //��� �߰�
    public void AddDialog(DialogData _dialogData)
    {
        //��� 
        ++DialogCount;
        //��ȭâ �Ѱ� ����
        OnoFFDilaog();
        //��ȭâ ũ�� ����
        ChangeDialogSize();
        //�ڸ� ����
        GameObject Text = Instantiate(testtext, Contensts.transform);
        //��ȭ ���� ����
        Text.GetComponent<DialogText>().ChangeData(_dialogData);
    }
    //��� ����
    public void DecreaseDialog()
    {
        --DialogCount;
        ChangeDialogSize();
        OnoFFDilaog();
    }

    //��ȭâ ũ������ 
    public void ChangeDialogSize()
    {
       
        int Top = basicHeight - ((DialogCount - 1) * increaseHeight);
        
        SetHeight(DialogUIRect, Top);
        //todo
        //�ִ� ũ�� ���� �ؾ���
    }

    //Rect Height ũ������
    private void SetHeight(RectTransform _rect, float _height) 
    {
        _rect.offsetMax = new Vector2(_rect.offsetMax.x,-_height); 
    }

    //��ȭâ �Ѱ� ����
    void OnoFFDilaog()
    {
        if (DialogCount > 0)
            GDialogUI.SetActive(true);
        else
            GDialogUI.SetActive(false);
    }
}
