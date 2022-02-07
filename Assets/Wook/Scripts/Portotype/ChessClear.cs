using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessClear : MonoBehaviour
{
    [SerializeField] Transform[] SafeboxOpenDoorLeftDoor;
    [SerializeField] Transform[] SafeboxOpenDoorRightDoor;
    [SerializeField] GameObject[] Camera;
    bool Open = false;
    public void StartOpenDoor()
    {
        if (Open)
            return;
        Open = true;
        GameManager.Instance.GetPlayerTrans(PlayerType.FirstPlayer).GetComponent<PlayerStatus>().ChangeHp(-40);
        for (int i = 0; i < SafeboxOpenDoorLeftDoor.Length-1; i++)
        {
            StartCoroutine(OpenDoor(SafeboxOpenDoorLeftDoor[i], true));
            StartCoroutine(OpenDoor(SafeboxOpenDoorRightDoor[i], false));

        }
        StartCoroutine(OpenDoorZ(SafeboxOpenDoorLeftDoor[2], true));
        StartCoroutine(OpenDoorZ(SafeboxOpenDoorRightDoor[2], false));
    }

    IEnumerator OpenDoor(Transform Door, bool isLeft)
    {
        float FalltimeCheck = 0;
        float Falltime = 3;
        float MoveX;

        float DoorYPos = Door.position.x;
        //�÷��̾�ī�޶� ����
        GameManager.Instance.PlayerCameraOnOFF(PlayerType.FirstPlayer, false);
        GameManager.Instance.PlayerCameraOnOFF(PlayerType.SecondPlayer, false);
        //�÷��̾� ���� ����
        GameManager.Instance.PlayerStateChange(PlayerType.FirstPlayer, PlayerState.Wait);
        GameManager.Instance.PlayerStateChange(PlayerType.SecondPlayer, PlayerState.Wait);
        //�ƽ� ī�޶� �ѱ�
        for (int i = 0; i < Camera.Length; i++)
            Camera[i].SetActive(true);
        yield return new WaitForSeconds(0.5f);

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

        //�÷��̾�ī�޶� ����
        GameManager.Instance.PlayerCameraOnOFF(PlayerType.FirstPlayer, true);
        GameManager.Instance.PlayerCameraOnOFF(PlayerType.SecondPlayer, true);
        //�÷��̾� ���� ����
        GameManager.Instance.PlayerStateChange(PlayerType.FirstPlayer, PlayerState.Walk);
        GameManager.Instance.PlayerStateChange(PlayerType.SecondPlayer, PlayerState.Walk);
        //�ƽ� ī�޶� �ѱ�
        for (int i = 0; i < Camera.Length; i++)
            Camera[i].SetActive(false);
    }

    IEnumerator OpenDoorZ(Transform Door, bool isLeft)
    {
        float FalltimeCheck = 0;
        float Falltime = 3;
        float MoveX;

        float DoorYPos = Door.position.z;
       
        yield return new WaitForSeconds(0.5f);

        while (FalltimeCheck <= Falltime)
        {
            FalltimeCheck += Time.deltaTime;
            if (isLeft)
            {
                MoveX = Mathf.Lerp(DoorYPos, DoorYPos - 2.5f, FalltimeCheck / Falltime);
                MoveX = (Door.transform.position.z) - (MoveX);
                Door.transform.position -= new Vector3(0, 0, MoveX);

            }
            else
            {

                MoveX = Mathf.Lerp(DoorYPos, DoorYPos + 2.5f, FalltimeCheck / Falltime);
                MoveX = (MoveX) - (Door.transform.position.z);
                Door.transform.position += new Vector3(0, 0, MoveX);
            }
            yield return null;
        }
    }
}
