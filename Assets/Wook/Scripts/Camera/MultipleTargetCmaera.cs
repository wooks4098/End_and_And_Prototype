using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class MultipleTargetCmaera : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] CinemachineVirtualCamera VituralCamera;//가상 카메라
    [SerializeField] CinemachineTransposer VituralCameraTransposer;//가상 카메라 Transposer
    [SerializeField] GameObject gCamera;//카메라 오브젝트
    [SerializeField] float CameraMinPos; //카메라 최소 높이
    [SerializeField] float CameraMaxPos; //카메라 최고 높이

    [Header("Target")]
    [SerializeField] Transform[] target;//카메라에 담길 타겟 오브젝트
    [SerializeField] Transform TargetPos; //카메라가 봐야할 중심 위치
    [SerializeField] float PlayerDistance;//타겟 사이 거리

    [Space]
    [SerializeField] bool isUseMultipleTargetCamera; //멀티플 카메라를 사용중인지
    float test = 0;

    private void Awake()
    {
        VituralCameraTransposer = VituralCamera.GetCinemachineComponent<CinemachineTransposer>();
    }

    private void Update()
    {
        if(isUseMultipleTargetCamera)
        {
            //TargetPos 지정
            TargetPosChange();
            //카메라 높이 변경
            VitrualCameraHightChange();

        }


    }

    //카메라 타겟 위치 변경
    void TargetPosChange()
    {
        TargetPos.position = new Vector3((target[0].position.x + target[1].position.x) / 2,
                                            (target[0].position.y + target[1].position.y) / 2,
                                            (target[0].position.z + target[1].position.z) / 2);
    }


    //카메라 높이 변경
    void VitrualCameraHightChange()
    {
        PlayerDistance = Vector3.Distance(target[0].position, target[1].position);
        
        float ChangeHeight = VituralCameraTransposer.m_FollowOffset.y;
       
        ChangeHeight = ( PlayerDistance) * 1.14f;
        VituralCameraTransposer.m_FollowOffset.y = ChangeHeight+5;
       
    }
}
