using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DialogData
{
    public string name; //ĳ���� �̸�
    public string dialog;//���
}
public enum DialogType
{
    All = 0,//��ü
    Separate,//����
}
public class DialogSystem : MonoBehaviour
{
    [SerializeField] DialogUI DialUi_All;
    [SerializeField] DialogUI[] DialUi_separate;
    
    
    

    //��ȭ �߰�
    public void AddDialog(DialogData _dialogData)
    {
        DialUi_All.AddDialog( _dialogData);
        //DialUi_separate[0].AddDialog(_dialogData);
        //DialUi_separate[1].AddDialog(_dialogData);
    }

    //��� ����
    public void DecreaseDialog(DialogType _DialogType,PlayerType _playertype)
    {
        switch (_DialogType)
        {
            case DialogType.All:
                DialUi_All.DecreaseDialog();

                break;
            case DialogType.Separate:
                Debug.Log((int)_playertype);
                DialUi_separate[(int)_playertype].DecreaseDialog();
                break;
        }       
    }

}
