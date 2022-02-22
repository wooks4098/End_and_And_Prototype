using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 모든 UICavas를 등록하고 명령을 내림
/// </summary>
public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance { get { return instance; } }

    private void Awake()
    {
        if (null == instance)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        dialogSystem = gameObject.GetComponent<DialogSystem>();
    }
    [SerializeField] UiController P1UI;
    [SerializeField] UiController P2UI;

    [SerializeField] Image[] Player1image;
    [SerializeField] Image[] Player2image;
    [SerializeField] Text[] player1text;
    [SerializeField] Text[] player2text;

    [SerializeField] Image[] Allimage;//전체 UI image
    [SerializeField] Text[] Alltext;//전체 UI image

    DialogSystem dialogSystem;
    int i = 0;
    #region Dialog
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            DialogData test = new DialogData();
            test.name = "테스트";
            test.dialog = i.ToString();
            i++;
            dialogSystem.AddDialog(test);
        }
    }
    public void AddDialog(DialogData _dialogData)
    {
        dialogSystem.AddDialog(_dialogData);
    }

    public void DecreaseDialog(DialogType _DialogType,PlayerType _playertype)
    {
        dialogSystem.DecreaseDialog(_DialogType, _playertype);
    }


    #endregion


    public void ShowHideAllUI(bool isShow)
    {
        for (int i = 0; i < Allimage.Length; ++i)
            Allimage[i].enabled = isShow;
        for (int i = 0; i < Alltext.Length; ++i)
            Alltext[i].enabled = isShow;
    }
    public void ShowHideDivisionUI(bool isShow)
    {
        for (int i = 0; i < Player1image.Length; ++i)
            Player1image[i].enabled = isShow;
        for (int i = 0; i < player1text.Length; ++i)
            player1text[i].enabled = isShow;
        for (int i = 0; i < Player2image.Length; ++i)
            Player2image[i].enabled = isShow;
        for (int i = 0; i < player2text.Length; ++i)
            player2text[i].enabled = isShow;
    }


    #region Status 

    public void ChangeHpUi(PlayerType PlayerType, float _hp)
    {
        switch (PlayerType)
        {
            case PlayerType.FirstPlayer:
                P1UI.ChangeHpUi(_hp);
                break;
            case PlayerType.SecondPlayer:
                P2UI.ChangeHpUi(_hp);
                break;
        }
    }


    #endregion



    #region ClimbWall

    public void StartClimbWall(PlayerType PlayerType)
    {
        switch (PlayerType)
        {
            case PlayerType.FirstPlayer:
                P1UI.StartClimbWall();
                break;
            case PlayerType.SecondPlayer:
                P2UI.StartClimbWall();
                break;
        }
    }

    public void EndClimbWall(PlayerType PlayerType)
    {
        switch (PlayerType)
        {
            case PlayerType.FirstPlayer:
                P1UI.EndClimbWall();
                break;
            case PlayerType.SecondPlayer:
                P2UI.EndClimbWall();
                break;
        }
    }

    public bool isSliderTriggerCheck(PlayerType PlayerType)
    {
        switch (PlayerType)
        {
            case PlayerType.FirstPlayer:
                return P1UI.isSliderTriggerCheck();
            case PlayerType.SecondPlayer:
               return  P2UI.isSliderTriggerCheck();
        }
        return false;
    }

    #endregion

    #region RopeHold
    public void StartHoldRope(PlayerType PlayerType)
    {
        switch (PlayerType)
        {
            case PlayerType.FirstPlayer:
                P1UI.StartHoldRope();
                break;
            case PlayerType.SecondPlayer:
                P2UI.StartHoldRope();
                break;
        }
    }

    public void EndHoldRope(PlayerType PlayerType)
    {
        switch (PlayerType)
        {
            case PlayerType.FirstPlayer:
                P1UI.EndHoldRope();
                break;
            case PlayerType.SecondPlayer:
                P2UI.EndHoldRope();
                break;
        }
    }

    public void AddHoldRopeValue(PlayerType PlayerType)
    {
        switch (PlayerType)
        {
            case PlayerType.FirstPlayer:
                P1UI.AddHoldRopeValue();
                break;
            case PlayerType.SecondPlayer:
                P2UI.AddHoldRopeValue();
                break;
        }
    }

    #endregion


    #region ObjectUI

    public void ObjectUIShow(PlayerType PlayerType ,string _text)
    {
        switch(PlayerType)
        {
            case PlayerType.FirstPlayer:
                P1UI.ObjectUIShow(_text);
                break;
            case PlayerType.SecondPlayer:
                P2UI.ObjectUIShow(_text);
                break;
        }
    }

    public void ObjectUIHide(PlayerType PlayerType)
    {
        switch (PlayerType)
        {
            case PlayerType.FirstPlayer:
                P1UI.ObjectUIHide();
                break;
            case PlayerType.SecondPlayer:
                P2UI.ObjectUIHide();
                break;
        }
    }

    public void ObjectUIMove(PlayerType PlayerType, Vector3 ObjPos)
    {
        switch (PlayerType)
        {
            case PlayerType.FirstPlayer:
                P1UI.ObjectUIMove(ObjPos);
                break;
            case PlayerType.SecondPlayer:
                P2UI.ObjectUIMove(ObjPos);
                break;
        }
    }

    #endregion
}
