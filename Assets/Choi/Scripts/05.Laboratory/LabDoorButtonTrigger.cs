using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabDoorButtonTrigger : MonoBehaviour
{
    // 컴포넌트
    private Animator buttonAnimator;
    [SerializeField] Animator doorAnimator;

    // 버튼에서 이미 한 번 갔다왔는지
    [SerializeField] bool hasExitFromButton = false;


    private void Awake()
    {
        buttonAnimator = GetComponentInParent<Animator>();
    }

    private void Start()
    {
        hasExitFromButton = false;
    }

    private void OnTriggerEnter(Collider _other)
    {
        // 이미 한 번 버튼을 눌렀으면
        if (hasExitFromButton) return;

        if(_other.tag == "Player1" || _other.tag == "Player2")
        {
            // 문열기
            buttonAnimator.SetBool("IsOpen", true);
            doorAnimator.SetBool("IsOpen", true);
        }
    }

    private void OnTriggerExit(Collider _other)
    {
        if (_other.tag == "Player1" || _other.tag == "Player2")
        {
            hasExitFromButton = false; // 버튼을 한 번 눌렀었음을 체크

            // 문닫기
            buttonAnimator.SetBool("IsOpen", false);
            doorAnimator.SetBool("IsOpen", false);
        }
    }
}
