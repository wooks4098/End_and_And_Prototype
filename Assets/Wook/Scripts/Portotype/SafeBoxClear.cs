using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeBoxClear : MonoBehaviour
{
    [SerializeField] Transform[] SafeboxOpenDoorLeftDoor;
    [SerializeField] Transform[] SafeboxOpenDoorRightDoor;
    [SerializeField] GameObject ChessAnswer;
    [SerializeField] GameObject[] Camera;
    [SerializeField] GameObject[] Collider;

    public void StartOpenDoor()
    {

        for (int i = 0; i < SafeboxOpenDoorLeftDoor.Length; i++)
        {
            StartCoroutine(SafeboxOpenDoor(SafeboxOpenDoorLeftDoor[i], true));
            StartCoroutine(SafeboxOpenDoor(SafeboxOpenDoorRightDoor[i], false));

        }
        //for (int i = 0; i < Collider.Length; i++)
        //    Collider[i].SetActive(false);
        UIManager.Instance.ObjectUIHide(PlayerType.FirstPlayer);
        UIManager.Instance.ObjectUIHide(PlayerType.SecondPlayer);
    }

    IEnumerator SafeboxOpenDoor(Transform Door, bool isLeft)
    {
        float FalltimeCheck = 0;
        float Falltime = 3;
        float MoveX;

        float DoorYPos = Door.position.x;
        //플레이어카메라 종료
        GameManager.Instance.PlayerCameraOnOFF(PlayerType.FirstPlayer, false);
        GameManager.Instance.PlayerCameraOnOFF(PlayerType.SecondPlayer, false);
        //플레이어 상태 변경
        GameManager.Instance.PlayerStateChange(PlayerType.FirstPlayer, PlayerState.Wait);
        GameManager.Instance.PlayerStateChange(PlayerType.SecondPlayer, PlayerState.Wait);
        //컷신 카메라 켜기
        for (int i = 0; i < Camera.Length; i++)
            Camera[i].SetActive(true);
        yield return new WaitForSeconds(0.5f);
        ChessAnswer.SetActive(true);

        while (FalltimeCheck <= Falltime)
        {
            FalltimeCheck += Time.deltaTime;
            if (isLeft)
            {
                MoveX = Mathf.Lerp(DoorYPos, DoorYPos - 2.5f, FalltimeCheck / Falltime);
                MoveX = (Door.transform.position.x) - (MoveX);
                Door.transform.position -= new Vector3(MoveX, 0, 0);

            }
            else
            {

                MoveX = Mathf.Lerp(DoorYPos, DoorYPos + 2.5f, FalltimeCheck / Falltime);
                MoveX = (MoveX) - (Door.transform.position.x);
                Door.transform.position += new Vector3(MoveX, 0, 0);
            }
            yield return null;
        }

        //플레이어카메라 종료
        GameManager.Instance.PlayerCameraOnOFF(PlayerType.FirstPlayer, true);
        GameManager.Instance.PlayerCameraOnOFF(PlayerType.SecondPlayer, true);
        //플레이어 상태 변경
        GameManager.Instance.PlayerStateChange(PlayerType.FirstPlayer, PlayerState.Walk);
        GameManager.Instance.PlayerStateChange(PlayerType.SecondPlayer, PlayerState.Walk);
        //컷신 카메라 켜기
        for (int i = 0; i < Camera.Length; i++)
            Camera[i].SetActive(false);
    }
}
