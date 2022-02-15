using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DialogText : MonoBehaviour
{
    [SerializeField] DialogType dialogType;
    [SerializeField] PlayerType playerType;
    [SerializeField] Text text;
    [SerializeField] DialogData Data;
    [SerializeField] float HideTime;

    public void ChangeData(DialogData _data)
    {
        Data = _data;
        SetText();
    }

    public void SetText()
    {
        Data.name = Data.name;
        Data.dialog = Data.dialog;
        text.text = Data.name + "  " + Data.dialog;
        StartCoroutine(Hide());
    }

    public void ResetData()
    {
        Data.name = "";
        Data.dialog= "";
    }

    IEnumerator Hide()
    {
        yield return new WaitForSeconds(HideTime);
        ResetData();

        Destroy(this.gameObject);
        UIManager.Instance.DecreaseDialog(dialogType, playerType);
    }
}
