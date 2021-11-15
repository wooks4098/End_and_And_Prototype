using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// �������� ȹ�� ���� �� 
/// ������ ������ �������� UI
/// </summary>

public class ItemGet : MonoBehaviour
{
    [SerializeField] GameObject gItemGet;
    [SerializeField] Image itemImage;
    [SerializeField] Text itemName;
    [SerializeField] Text itemInfo;

    Coroutine cItemGetUIClose;


    public void ShowItemGetUI(Item _item)
    {
        gItemGet.SetActive(true);

        itemImage.sprite = _item.itemSprite;
        itemName.text = _item.ItemName;
        itemInfo.text = _item.itemInfo;

        //StopCoroutine(cItemGetUIClose);
        cItemGetUIClose = StartCoroutine(CloseItemGetUI());

    }

    IEnumerator CloseItemGetUI()
    {
        yield return new WaitForSeconds(1.5f);
        gItemGet.SetActive(false);
    }



}
