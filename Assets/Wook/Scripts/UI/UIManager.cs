using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }
    [SerializeField] UiController P1UI;
    [SerializeField] UiController P2UI;

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

    public void ObjectUIShow(PlayerType PlayerType)
    {
        switch(PlayerType)
        {
            case PlayerType.FirstPlayer:
                P1UI.ObjectUIShow();
                break;
            case PlayerType.SecondPlayer:
                P2UI.ObjectUIShow();
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
}
