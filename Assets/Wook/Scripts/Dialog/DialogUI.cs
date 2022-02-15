using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Dialog UI를 관리하는 스크립트
/// - 크기조절
/// </summary>
public class DialogUI : MonoBehaviour
{
    [SerializeField] GameObject GDialogUI;
    RectTransform DialogUIRect;
    [SerializeField] GameObject Contensts;
    List<GameObject> DialogList;//대화 리스트
    [SerializeField] GameObject testtext;
    [SerializeField] int DialogCount;//현재 보여지는 대사 개수
    [SerializeField] int basicHeight;//대화창 기본 높이
    [SerializeField] int increaseHeight;//대화창 증가 수치
    private void Start()
    {
        DialogUIRect = GDialogUI.GetComponent<RectTransform>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            //DialogData test = new DialogData();
            //test.name = "테스트";
            //test.dialog = testnumber.ToString();
            //AddDialog(++testnumber, test);
        }
    }

    //대사 추가
    public void AddDialog(DialogData _dialogData)
    {
        //대사 
        ++DialogCount;
        //대화창 켜고 끄기
        OnoFFDilaog();
        //대화창 크기 조절
        ChangeDialogSize();
        //자막 생성
        GameObject Text = Instantiate(testtext, Contensts.transform);
        //대화 정보 변경
        Text.GetComponent<DialogText>().ChangeData(_dialogData);
    }
    //대사 제거
    public void DecreaseDialog()
    {
        --DialogCount;
        ChangeDialogSize();
        OnoFFDilaog();
    }

    //대화창 크기조절 
    public void ChangeDialogSize()
    {
       
        int Top = basicHeight - ((DialogCount - 1) * increaseHeight);
        
        SetHeight(DialogUIRect, Top);
        //todo
        //최대 크기 제한 해야함
    }

    //Rect Height 크기조절
    private void SetHeight(RectTransform _rect, float _height) 
    {
        _rect.offsetMax = new Vector2(_rect.offsetMax.x,-_height); 
    }

    //대화창 켜고 끄기
    void OnoFFDilaog()
    {
        if (DialogCount > 0)
            GDialogUI.SetActive(true);
        else
            GDialogUI.SetActive(false);
    }
}
