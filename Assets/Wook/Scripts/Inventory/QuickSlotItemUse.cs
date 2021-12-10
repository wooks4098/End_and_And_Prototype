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

        //PlayerStatus 컴포넌트 가져오기
        playerStatus = GameManager.Instance.GetPlayerController(playerType).GetComponent<PlayerStatus>();
        SetInput();
    }
    //인풋 연결
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
        
        //추가할것
        //사용효과
        //쿨타임
    }

    //아이템 사용 효과
    void UseEffect(Item _item)
    {
        switch(_item.itemUseEffect)
        {
            case ItemUseEffect.HpUp:
                playerStatus.ChangeHp(_item.UseItemFigure);
                Debug.Log("체력회복");
                break;
            case ItemUseEffect.ThirstyUp:
                playerStatus.Changethirst(_item.UseItemFigure);
                Debug.Log("목마름 회복");
                break;
            case ItemUseEffect.InfectionRateDown:
                Debug.Log("감염율 하락");
                break;
        }
    }

}
