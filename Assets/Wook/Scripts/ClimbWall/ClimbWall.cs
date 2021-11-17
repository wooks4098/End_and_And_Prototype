using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 협동기믹중 오르는 벽
/// </summary>
public class ClimbWall : MonoBehaviour
{
    ObjectUIShow objectUIShow;
    bool isPlayergoup;//플레이어가 올라갔는지
    [SerializeField] bool isClimb = false;//오르는중인지

    private void Awake()
    {
        objectUIShow = GetComponentInChildren<ObjectUIShow>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && objectUIShow.GetCanUse() == true && isClimb == false)
        {
            PlayerPanertChange();
        }
    }

    //플레이어 Climb시작
    void PlayerPanertChange()
    {
        Transform playerTrans = GameManager.Instance.GetPlayerTrans(objectUIShow.GetPlayerType());
        Transform playerModelTrans = GameManager.Instance.GetPlayerModelTrans(objectUIShow.GetPlayerType());

        //Vector3 vec = transform.position - playerTrans.position;
        //vec = new Vector3(0, vec.y, 0);
        //Quaternion q = Quaternion.LookRotation(vec.normalized);
        //q = new Quaternion(0, q.y, 0, q.w);new Quaternion(0, -transform.rotation.y, 0, transform.rotation.w);
        playerTrans.rotation = Quaternion.Euler(0, 0, 0);
        playerModelTrans.rotation = new Quaternion(0, transform.rotation.y, 0, transform.rotation.w);

        //플레이어 부모 변경
        //playerTrans.parent = transform; //player.gameObject.transform.parent.gameObject.transform.parent = transform;
        isClimb = true;
        //플레이어 상태 변경
        GameManager.Instance.PlayerStateChange(objectUIShow.GetPlayerType(), PlayerState.ClimbWall);
        //플레이어 방향 변경
    }
}
