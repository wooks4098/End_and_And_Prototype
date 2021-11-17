using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ��������� ������ ��
/// </summary>
public class ClimbWall : MonoBehaviour
{
    ObjectUIShow objectUIShow;
    bool isPlayergoup;//�÷��̾ �ö󰬴���
    [SerializeField] bool isClimb = false;//������������

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

    //�÷��̾� Climb����
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

        //�÷��̾� �θ� ����
        //playerTrans.parent = transform; //player.gameObject.transform.parent.gameObject.transform.parent = transform;
        isClimb = true;
        //�÷��̾� ���� ����
        GameManager.Instance.PlayerStateChange(objectUIShow.GetPlayerType(), PlayerState.ClimbWall);
        //�÷��̾� ���� ����
    }
}
