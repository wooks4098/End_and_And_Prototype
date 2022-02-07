using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    [SerializeField] Transform[] SafeboxOpenDoorLeftDoor;
    [SerializeField] Transform[] SafeboxOpenDoorRightDoor;


    public void StartOpenDoor()
    {

        for (int i = 0; i < SafeboxOpenDoorLeftDoor.Length; i++)
        {
            StartCoroutine(SafeboxOpenDoor(SafeboxOpenDoorLeftDoor[i], true));
            StartCoroutine(SafeboxOpenDoor(SafeboxOpenDoorRightDoor[i], false));

        }
    }

    IEnumerator SafeboxOpenDoor(Transform Door, bool isLeft)
    {
        float FalltimeCheck = 0;
        float Falltime = 3;
        float MoveX;

        float DoorYPos = Door.position.x;
       
        while (FalltimeCheck <= Falltime)
        {
            FalltimeCheck += Time.deltaTime;
            if (isLeft)
            {
                MoveX = Mathf.Lerp(DoorYPos, DoorYPos - 2.5f, FalltimeCheck / Falltime);
                MoveX =  (Door.transform.position.x) - (MoveX);
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
    }
}
