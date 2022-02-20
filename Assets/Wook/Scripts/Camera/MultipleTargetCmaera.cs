using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class MultipleTargetCmaera : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] CinemachineVirtualCamera VituralCamera;//���� ī�޶�
    [SerializeField] CinemachineTransposer VituralCameraTransposer;//���� ī�޶� Transposer
    [SerializeField] GameObject gCamera;//ī�޶� ������Ʈ
    [SerializeField] float CameraMinPos; //ī�޶� �ּ� ����
    [SerializeField] float CameraMaxPos; //ī�޶� �ְ� ����

    [Header("Target")]
    [SerializeField] Transform[] target;//ī�޶� ��� Ÿ�� ������Ʈ
    [SerializeField] Transform TargetPos; //ī�޶� ������ �߽� ��ġ
    [SerializeField] float PlayerDistance;//Ÿ�� ���� �Ÿ�

    [Space]
    [SerializeField] bool isUseMultipleTargetCamera; //��Ƽ�� ī�޶� ���������
    float test = 0;

    private void Awake()
    {
        VituralCameraTransposer = VituralCamera.GetCinemachineComponent<CinemachineTransposer>();
    }

    private void Update()
    {
        if(isUseMultipleTargetCamera)
        {
            //TargetPos ����
            TargetPosChange();
            //ī�޶� ���� ����
            VitrualCameraHightChange();

        }


    }

    //ī�޶� Ÿ�� ��ġ ����
    void TargetPosChange()
    {
        TargetPos.position = new Vector3((target[0].position.x + target[1].position.x) / 2,
                                            (target[0].position.y + target[1].position.y) / 2,
                                            (target[0].position.z + target[1].position.z) / 2);
    }


    //ī�޶� ���� ����
    void VitrualCameraHightChange()
    {
        PlayerDistance = Vector3.Distance(target[0].position, target[1].position);
        
        float ChangeHeight = VituralCameraTransposer.m_FollowOffset.y;
       
        ChangeHeight = ( PlayerDistance) * 1.14f;
        VituralCameraTransposer.m_FollowOffset.y = ChangeHeight+5;
       
    }
}
