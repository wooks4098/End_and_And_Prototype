using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRevival : MonoBehaviour
{
    ObjectUIShow objectUIShow;
    [SerializeField] PlayerType playertype;
    [SerializeField] PlayerController playercontroller;
    private void Awake()
    {
        objectUIShow = GetComponent<ObjectUIShow>();
        objectUIShow.gameObject.SetActive(false);
    }

    //인풋 연결
    public void SetOnInputManager(PlayerType _playertype)
    {
        switch(_playertype)
        {
            case PlayerType.FirstPlayer:
                InputManager.Instance.OnUsePlayer2 += RevivalPlayer;
                break;
            case PlayerType.SecondPlayer:
                InputManager.Instance.OnUsePlayer1 += RevivalPlayer;

                break;
        }
    }
    //인풋 연걸 해제
    void SetOffInputManager(PlayerType _playertype)
    {
        switch (_playertype)
        {
            case PlayerType.FirstPlayer:
                InputManager.Instance.OnUsePlayer2 -= RevivalPlayer;
                break;
            case PlayerType.SecondPlayer:
                InputManager.Instance.OnUsePlayer1 -= RevivalPlayer;
                break;
        }
    }

    //플레이어가 빈사 상태가 되었을 때
    public void SetCrawl(PlayerType _playertype)
    {
        objectUIShow.gameObject.SetActive(true);

        SetOnInputManager(_playertype);
    }
    
    void RevivalPlayer(PlayerType _playerType, PlayerState _playerState)
    {
        if (GameManager.Instance.GetPlayerState(playertype)!= PlayerState.Crawl)
            return;
        if (objectUIShow.GetCanUse() == false)
            return;
        SetOffInputManager(_playerType);
        objectUIShow.UIHide(playertype);
        playercontroller.EndCrawl();
        objectUIShow.gameObject.SetActive(false);
    }

}
