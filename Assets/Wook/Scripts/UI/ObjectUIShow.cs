using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 해당 오브젝트에 플레이어가 다가온 경우 UI매니저에게 요청
/// 해당 오브젝트를 사용할 수 있는 상태인지 확인
/// </summary>
public class ObjectUIShow : MonoBehaviour
{

    [SerializeField] PlayerType playerType;
    [SerializeField] Transform ObjectUiPos;
    [SerializeField] bool CanUse = false;// 오브젝트 사용가능한지
    [SerializeField] string objectText;

    public bool GetCanUse()
    {
        return CanUse;
    }

    public PlayerType GetPlayerType()
    {
        return playerType;
    }

    public void HideObjectUIShow()
    {
        gameObject.SetActive(false);
    }
    public void UIShow(PlayerType _playertype)
    {
        switch (playerType)
        {
            case PlayerType.FirstPlayer:
                CanUse = true;
                GameManager.Instance.PlayerObjectHitin(PlayerType.FirstPlayer, objectText);
                break;
            case PlayerType.SecondPlayer:
                CanUse = true;
                GameManager.Instance.PlayerObjectHitin(PlayerType.SecondPlayer, objectText);
                break;
        }
    }

    public void UIMove(PlayerType _playertype)
    {
        switch (_playertype)
        {
            case PlayerType.FirstPlayer:
                    GameManager.Instance.ObjectUiMove(PlayerType.FirstPlayer, ObjectUiPos.position);
                break;
            case PlayerType.SecondPlayer:
                    GameManager.Instance.ObjectUiMove(PlayerType.SecondPlayer, ObjectUiPos.position);
                break;
        }
    }

    public void UIHide(PlayerType _playertype)
    {
        switch (_playertype)
        {
            case PlayerType.FirstPlayer:
                CanUse = false;
                GameManager.Instance.PlayerObjectHitout(PlayerType.FirstPlayer);
                break;
            case PlayerType.SecondPlayer:
                CanUse = false;
                GameManager.Instance.PlayerObjectHitout(PlayerType.SecondPlayer);
                break;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player1" && playerType == PlayerType.FirstPlayer)
            UIShow(PlayerType.FirstPlayer);
        if (other.tag == "Player2" && playerType == PlayerType.SecondPlayer)
            UIShow(PlayerType.SecondPlayer);


    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player1" && playerType == PlayerType.FirstPlayer)
            UIMove(PlayerType.FirstPlayer);
        if (other.tag == "Player2" && playerType == PlayerType.SecondPlayer)
            UIMove(PlayerType.FirstPlayer);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player1" && playerType == PlayerType.FirstPlayer)
            UIHide(PlayerType.FirstPlayer);
        if (other.tag == "Player2" && playerType == PlayerType.SecondPlayer)
            UIHide(PlayerType.SecondPlayer);

    }

}
