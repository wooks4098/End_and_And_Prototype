using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlotItemUse : MonoBehaviour
{
    [SerializeField] PlayerType playerType;

    [SerializeField] GameObject G_QuickSlotsParent;
    [SerializeField] Slot[] solts;
    [SerializeField] PlayerStatus playerStatus;

    private void Start()
    {
        solts = G_QuickSlotsParent.GetComponentsInChildren<Slot>();

        //PlayerStatus ������Ʈ ��������
        playerStatus = GameManager.Instance.GetPlayerController(playerType).GetComponent<PlayerStatus>();
        SetInput();
    }
    //��ǲ ����
    void SetInput()
    {
        switch (playerType)
        {
            case PlayerType.FirstPlayer:
                InputManager.Instance.OnQuickSoltPlayer1 += UseItem;
                break;
            case PlayerType.SecondPlayer:
                InputManager.Instance.OnQuickSoltPlayer2 += UseItem;
                break;
        }

    }

    void UseItem(int _slotNumber)
    {
        UseEffect(solts[_slotNumber].item);
        solts[_slotNumber].SetSoltCount(-1);
        
        //�߰��Ұ�
        //���ȿ��
        //��Ÿ��
    }

    //������ ��� ȿ��
    void UseEffect(Item _item)
    {
        switch(_item.itemUseEffect)
        {
            case ItemUseEffect.HpUp:
                playerStatus.ChangeHp(_item.UseItemFigure);
                Debug.Log("ü��ȸ��");
                break;
            case ItemUseEffect.ThirstyUp:
                playerStatus.Changethirst(_item.UseItemFigure);
                Debug.Log("�񸶸� ȸ��");
                break;
            case ItemUseEffect.InfectionRateDown:
                Debug.Log("������ �϶�");
                break;
        }
    }

}
